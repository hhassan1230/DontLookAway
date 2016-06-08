/*
  This class is in charge of containing objects that share the same shader. (even tho the AddObject(OptimizableObject) method doesnt
  care if the shader of the objects[] matches with the shader of the class.

  Later on, ObjSorter sorts the objects inside each OptimizableShader to match the shader.

  Created by:
  Juan Sebastian Munoz Arango
  naruse@gmail.com
  All rights reserved
*/
namespace ProDrawCall {

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class OptimizableShader {
    private string shaderName = "";
    public string ShaderName { get { return shaderName; } }

    /*
      If a shader is standard, it means that the null setted shader defines for this shader
      are not used / disabled when the shader is working.

      This means basically that we _HAVE_ to avoid using any shader define that is null-setted
      on the shader.
     */
    private bool standardShader = false;


    public void SetCombineAllMeshes(bool val) {
        for(int i = 0; i < combineMeshesFlags.Count; i++)
                combineMeshesFlags[i] = val;
    }

    private List<bool> combineMeshesFlags;
    public List<bool> CombineMeshesFlags {
        get { return combineMeshesFlags; }
        set { combineMeshesFlags = value; }
    }
    private List<OptimizableObject> objects;
    public List<OptimizableObject> Objects { get { return objects; } }

    private const int NO_CACHED = -1;//const that represents that an int is not cached (used for caching atlas sizes)
    private int cacheAtlasSizeReuseTextures = NO_CACHED;
    private int cacheAtlasSizeNoReuseTextures = NO_CACHED;

    private List<Tuple<GameObject, int>> optimizedObjects;//contains the optimized objects, only filled after "OptimizeShader()" is called.
    //the int in the tuple is the InstanceID for the original object, this is needed for building the hierarchy.
    public List<Tuple<GameObject, int>> GetOptimizedObjects() { return optimizedObjects; }

    //creates a new optimizable shader and initializes it with an initial object.
    //the initial object is needed as we need to parse the standard shader in case there is one.
    public OptimizableShader(string name, OptimizableObject initialObject, bool combineObject) {
        optimizedObjects = new List<Tuple<GameObject, int>>();
        shaderName = "";//by default
        objects = new List<OptimizableObject>();
        combineMeshesFlags = new List<bool>();

        cacheAtlasSizeNoReuseTextures = NO_CACHED;
        cacheAtlasSizeReuseTextures = NO_CACHED;


        //if the initial object is not null this means we have to check if the initial object has a standard shader
        if(initialObject != null) {
            AddObject(initialObject, combineObject);
            if(Utils.IsShaderStandard(name)) {
                standardShader = true;
                shaderName = Utils.ParseStandardShaderName(initialObject.ObjectMaterial);
            } else {//shader is a normal shader, no need to parse its name
                standardShader = false;
                shaderName = name;
            }
        }
    }

    // adds an object to the objects list.
    // WARNING: this doesnt care if the optimizable object obj matches the shader name
    // of this object.
    // Later on the ObjSorter.cs->SortObjects() organizes them to match
    public void AddObject(OptimizableObject obj, bool combineObj) {
        objects.Add(obj);
        combineMeshesFlags.Add(combineObj);
    }

    public void RemoveObjectAt(int index) {
        objects.RemoveAt(index);
        combineMeshesFlags.RemoveAt(index);
    }

    public void SetObjectAtIndex(int index, OptimizableObject obj, bool combineObj) {
        objects[index] = obj;
        combineMeshesFlags[index] = combineObj;
    }

    public void OptimizeShader(bool reuseTextures, bool generatePrefabs, bool generatePowerOf2Atlases) {
        optimizedObjects.Clear();
        
        if(shaderName == "")//unknown shader doesnt need to be optimed
            return;
        int currentAtlasSize = CalculateAproxAtlasSize(reuseTextures, generatePowerOf2Atlases);
        if((objects.Count > 1 || //more than 1 obj or 1 obj with multiple mat
            (objects.Count == 1 && objects[0] != null && objects[0].ObjHasMoreThanOneMaterial)) &&
           currentAtlasSize < Constants.MaxAtlasSize) { //check the generated atlas size doesnt exceed max supported texture size

            Node resultNode = null;//nodes for the tree for atlasing

            Atlasser generatedAtlas = new Atlasser(currentAtlasSize, currentAtlasSize, generatePowerOf2Atlases);
            int resizeTimes = 1;

            TextureReuseManager textureReuseManager = new TextureReuseManager();

            for(int j = objects.Count-1; j >= 0; j--) {//start from the largest to the shortest textures
                if(objects[j].ObjHasMoreThanOneMaterial)//before atlassing multiple materials obj, combine it.
                    objects[j].ProcessAndCombineMaterials();

                Vector2 textureToAtlasSize = objects[j].TextureSize;
                if(reuseTextures) {
                    //if texture is not registered already
                    if(!textureReuseManager.TextureRefExists(objects[j])) {
                        //generate a node
                        resultNode = generatedAtlas.Insert(Mathf.RoundToInt((textureToAtlasSize.x != Constants.NULLV2.x) ? textureToAtlasSize.x : Constants.NullTextureSize),
                                                           Mathf.RoundToInt((textureToAtlasSize.y != Constants.NULLV2.y) ? textureToAtlasSize.y : Constants.NullTextureSize));
                        if(resultNode != null) //save node if fits in atlas
                            textureReuseManager.AddTextureRef(objects[j], resultNode.NodeRect, j);
                    }
                } else {
                    resultNode = generatedAtlas.Insert(Mathf.RoundToInt((textureToAtlasSize.x != Constants.NULLV2.x) ? textureToAtlasSize.x : Constants.NullTextureSize),
                                                       Mathf.RoundToInt((textureToAtlasSize.y != Constants.NULLV2.y) ? textureToAtlasSize.y : Constants.NullTextureSize));
                }
                if(resultNode == null) {
                    int resizedAtlasSize = currentAtlasSize + Mathf.RoundToInt((float)currentAtlasSize * Constants.AtlasResizeFactor * resizeTimes);
                    if(generatePowerOf2Atlases) {
                        resizedAtlasSize = Mathf.NextPowerOfTwo(resizedAtlasSize);
                    }
                    generatedAtlas = new Atlasser(resizedAtlasSize, resizedAtlasSize, generatePowerOf2Atlases);
                    j = objects.Count;//Count and not .Count-1 bc at the end of the loop it will be substracted j-- and we want to start from Count-1

                    textureReuseManager.ClearTextureRefs();
                    resizeTimes++;
                }
            }
            Material atlasMaterial = CreateAtlasMaterialAndTexture(generatedAtlas, shaderName, textureReuseManager);

            OptimizeDrawCalls(ref atlasMaterial,
                              generatedAtlas.GetAtlasSize().x,
                              generatedAtlas.GetAtlasSize().y,
                              generatedAtlas.TexturePositions,
                              reuseTextures,
                              textureReuseManager,
                              generatePrefabs);
            //after the game object has been organized, remove the combined game objects.
            for(int i = 0; i < objects.Count; i++) {
                if(objects[i].ObjWasCombined)
                    objects[i].ClearCombinedObject();
            }
        }
    }

    private void OptimizeDrawCalls(ref Material atlasMaterial,  float atlasWidth, float atlasHeight, List<Rect> texturePos, bool reuseTextures, TextureReuseManager texReuseMgr, bool generatePrefabsForObjects) {
        GameObject trash = new GameObject("Trash");//stores unnecesary objects that might be cloned and are children of objects
        List<GameObject> meshesToCombine = new List<GameObject>();

        // // // when generating prefabs // // //
			string folderToSavePrefabs = (PersistenceHandler.Instance.PathToSaveOptimizedObjs != "") ?
					PersistenceHandler.Instance.PathToSaveOptimizedObjs + Path.DirectorySeparatorChar + Utils.GetCurrentSceneName() :
					EditorApplication.currentScene;
			if(generatePrefabsForObjects) {
             	if(EditorApplication.currentScene == "") { //scene is not saved yet.
                	folderToSavePrefabs = Constants.NonSavedSceneFolderName + ".unity";
            	}
            	folderToSavePrefabs = folderToSavePrefabs.Substring(0, folderToSavePrefabs.Length-6) + "-Atlas";//remove the ".unity"
            	folderToSavePrefabs += Path.DirectorySeparatorChar + "Prefabs";
            	if(!Directory.Exists(folderToSavePrefabs)) {
                	Directory.CreateDirectory(folderToSavePrefabs);
                	AssetDatabase.Refresh();
            	}
        	}
        ///////////////////////////////////////////

        for(int i = 0; i < objects.Count; i++) {
            string optimizedObjStrID = objects[i].GameObj.name + Constants.OptimizedObjIdentifier;
            if(objects[i].UsesSkinnedMeshRenderer)
                objects[i].GameObj.GetComponent<SkinnedMeshRenderer>().enabled = true;//activate renderers for instantiating
            else
                objects[i].GameObj.GetComponent<MeshRenderer>().enabled = true;

            GameObject instance = GameObject.Instantiate(objects[i].GameObj,
                                                         objects[i].GameObj.transform.position,
                                                         objects[i].GameObj.transform.rotation) as GameObject;
            Undo.RegisterCreatedObjectUndo(instance,"CreateObj" + optimizedObjStrID);

            //remove children of the created instance.
            Transform[] children = instance.GetComponentsInChildren<Transform>();
            for(int j = 0; j < children.Length; j++)
                children[j].transform.parent = trash.transform;

            instance.transform.parent = objects[i].GameObj.transform.parent;
            instance.transform.localScale = objects[i].GameObj.transform.localScale;
            if(objects[i].UsesSkinnedMeshRenderer)
                instance.GetComponent<SkinnedMeshRenderer>().sharedMaterial = atlasMaterial;
            else
                instance.GetComponent<MeshRenderer>().sharedMaterial = atlasMaterial;

            instance.name = optimizedObjStrID;
            if(objects[i].UsesSkinnedMeshRenderer)
                instance.GetComponent<SkinnedMeshRenderer>().sharedMesh = Utils.CopyMesh(objects[i].GameObj.GetComponent<SkinnedMeshRenderer>().sharedMesh);
            else
                instance.GetComponent<MeshFilter>().sharedMesh = Utils.CopyMesh(objects[i].GameObj.GetComponent<MeshFilter>().sharedMesh);

            // ************************************ Remap uvs ***************************************** //
            Mesh remappedMesh = objects[i].UsesSkinnedMeshRenderer ? instance.GetComponent<SkinnedMeshRenderer>().sharedMesh : instance.GetComponent<MeshFilter>().sharedMesh;
            Vector2[] remappedUVs = remappedMesh.uv;//objects[i].UsesSkinnedMeshRenderer ? instance.GetComponent<SkinnedMeshRenderer>().sharedMesh.uv : instance.GetComponent<MeshFilter>().sharedMesh.uv;
            Vector2[] remappedUVs2 = remappedMesh.uv2;
            Vector2[] remappedUVs3 = remappedMesh.uv3;
            Vector2[] remappedUVs4 = remappedMesh.uv4;
            bool hasUv2Channel = remappedUVs2.Length > 0;
            bool hasUv3Channel = remappedUVs3.Length > 0;
            bool hasUv4Channel = remappedUVs4.Length > 0;

            bool generatedTexture = objects[i].MainTexture == null;

            for(int j = 0; j < remappedUVs.Length; j++) {
                if(reuseTextures) {
                    if(SettingsMenuGUI.Instance.ModifyMainUV)
                        remappedUVs[j] = Utils.ReMapUV(remappedUVs[j],
                                                       atlasWidth,
                                                       atlasHeight,
                                                       texReuseMgr.GetTextureRefPosition(objects[i]),
                                                       instance.name, generatedTexture);
                    if(hasUv2Channel && SettingsMenuGUI.Instance.ModifyUV2)
                        remappedUVs2[j] = Utils.ReMapUV(remappedUVs2[j], atlasWidth, atlasHeight, texReuseMgr.GetTextureRefPosition(objects[i]), instance.name, generatedTexture);
                    if(hasUv3Channel && SettingsMenuGUI.Instance.ModifyUV3)
                        remappedUVs3[j] = Utils.ReMapUV(remappedUVs3[j], atlasWidth, atlasHeight, texReuseMgr.GetTextureRefPosition(objects[i]), instance.name, generatedTexture);
                    if(hasUv4Channel && SettingsMenuGUI.Instance.ModifyUV4)
                        remappedUVs4[j] = Utils.ReMapUV(remappedUVs4[j], atlasWidth, atlasHeight, texReuseMgr.GetTextureRefPosition(objects[i]), instance.name, generatedTexture);
                } else {
                    if(SettingsMenuGUI.Instance.ModifyMainUV)
                        remappedUVs[j] = Utils.ReMapUV(remappedUVs[j], atlasWidth, atlasHeight, texturePos[i], instance.name, generatedTexture);
                    if(hasUv2Channel && SettingsMenuGUI.Instance.ModifyUV2)
                        remappedUVs2[j] = Utils.ReMapUV(remappedUVs2[j], atlasWidth, atlasHeight, texturePos[i], instance.name, generatedTexture);
                    if(hasUv3Channel && SettingsMenuGUI.Instance.ModifyUV3)
                        remappedUVs3[j] = Utils.ReMapUV(remappedUVs3[j], atlasWidth, atlasHeight, texturePos[i], instance.name, generatedTexture);
                    if(hasUv4Channel && SettingsMenuGUI.Instance.ModifyUV4)
                        remappedUVs4[j] = Utils.ReMapUV(remappedUVs4[j], atlasWidth, atlasHeight, texturePos[i], instance.name, generatedTexture);
                }
            }
            remappedMesh.uv = remappedUVs;
            if(hasUv2Channel) remappedMesh.uv2 = remappedUVs2;
            if(hasUv3Channel) remappedMesh.uv3 = remappedUVs3;
            if(hasUv4Channel) remappedMesh.uv4 = remappedUVs4;

            if(objects[i].UsesSkinnedMeshRenderer) {
                instance.GetComponent<SkinnedMeshRenderer>().sharedMesh = remappedMesh;
                Undo.RecordObject(objects[i].GameObj.GetComponent<SkinnedMeshRenderer>(), "Active Obj");
            } else {
                instance.GetComponent<MeshFilter>().sharedMesh = remappedMesh;
                Undo.RecordObject(objects[i].GameObj.GetComponent<MeshRenderer>(), "Active Obj");
            }

            if(combineMeshesFlags[i]) {//if the object is marked for combining
                meshesToCombine.Add(instance);
            }

            //if the gameObject has multiple materials, search for the original one (the uncombined) in order to deactivate it
            if(objects[i].ObjWasCombined) {
                if(objects[i].UsesSkinnedMeshRenderer)
                    objects[i].UncombinedObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
                else
                    objects[i].UncombinedObject.GetComponent<MeshRenderer>().enabled = false;
            } else {
                if(objects[i].UsesSkinnedMeshRenderer)
                    objects[i].GameObj.GetComponent<SkinnedMeshRenderer>().enabled = false;
                else
                    objects[i].GameObj.GetComponent<MeshRenderer>().enabled = false;
            }

            if(generatePrefabsForObjects && !combineMeshesFlags[i]) {//lets not generate a prefab for an object that is marked for combine
                string prefabName = Utils.GetValiName(instance.name) + " " + instance.GetInstanceID();
                string assetPath = folderToSavePrefabs + Path.DirectorySeparatorChar + prefabName;
                Utils.GeneratePrefab(instance, assetPath, prefabName, objects[i].UsesSkinnedMeshRenderer);

            }
            //useful only when building hierarchies
            //instanceID of the transform as we are comparing against parent transforms when building hierachies
            int originalOptimizedObjectInstanceID = objects[i].ObjWasCombined ? objects[i].UncombinedObject.transform.GetInstanceID() : objects[i].GameObj.transform.GetInstanceID();
            optimizedObjects.Add(new Tuple<GameObject, int>(instance, originalOptimizedObjectInstanceID));
        }
        if(meshesToCombine.Count > 1) {
            List<GameObject> combinedObjects = Utils.CombineObjects(meshesToCombine, atlasMaterial);
            for(int i = 0; i < meshesToCombine.Count; i++)
                GameObject.DestroyImmediate(meshesToCombine[i]);
            for(int i = 0; i < combinedObjects.Count; i++) {
                combinedObjects[i].name = "Combined " + i + " " + shaderName + Constants.OptimizedObjIdentifier;
                string prefabName = Utils.GetValiName(combinedObjects[i].name) + " " + combinedObjects[i].GetInstanceID();
                string assetPath = folderToSavePrefabs + Path.DirectorySeparatorChar + prefabName;

                if(generatePrefabsForObjects) {
                    Utils.GeneratePrefab(combinedObjects[i], assetPath, prefabName, false);
                }
            }
        }

        GameObject.DestroyImmediate(trash);
    }

    private Material CreateAtlasMaterialAndTexture(Atlasser generatedAtlas, string shaderToAtlas, TextureReuseManager textureReuseManager) {
        string fileName = ((ObjectsGUI.CustomAtlasName == "") ? "Atlas " : (ObjectsGUI.CustomAtlasName + " ")) + shaderToAtlas.Replace('/','_');
		string folderToSaveAssets = (PersistenceHandler.Instance.PathToSaveOptimizedObjs != "") ?
										PersistenceHandler.Instance.PathToSaveOptimizedObjs + Path.DirectorySeparatorChar + Utils.GetCurrentSceneName() :
										EditorApplication.currentScene;
		if(EditorApplication.currentScene == "") { //scene is not saved yet.
            folderToSaveAssets = Constants.NonSavedSceneFolderName + ".unity";
            Debug.LogWarning("WARNING: Scene has not been saved, saving baked objects to: " + Constants.NonSavedSceneFolderName + " folder");
        }

        folderToSaveAssets = folderToSaveAssets.Substring(0, folderToSaveAssets.Length-6) + "-Atlas";//remove the ".unity" and add "-Atlas"
        if(!Directory.Exists(folderToSaveAssets)) {
            Directory.CreateDirectory(folderToSaveAssets);
            AssetDatabase.ImportAsset(folderToSaveAssets);
        }
        string atlasTexturePath = folderToSaveAssets + Path.DirectorySeparatorChar + fileName;
        //create the material in the project and set the shader material to shaderToAtlas
        Material atlasMaterial = new Material(Shader.Find(standardShader ? Utils.ExtractStandardShaderOriginalName(shaderToAtlas) : shaderToAtlas));
        //save the material to the project view
        AssetDatabase.CreateAsset(atlasMaterial, atlasTexturePath + "Mat.mat");
        AssetDatabase.ImportAsset(atlasTexturePath + "Mat.mat");
        //load a reference from the project view to the material (this is done to be able to set the texture to the material in the project view)
        atlasMaterial = (Material) AssetDatabase.LoadAssetAtPath(atlasTexturePath + "Mat.mat", typeof(Material));

        List<string> shaderDefines;
        if(standardShader) {
            shaderDefines = ShaderManager.Instance.GetShaderTexturesDefines(shaderToAtlas, false, objects[0].ObjectMaterial);//we need the 1rst object in the list to know what textures are used.
        } else
            shaderDefines = ShaderManager.Instance.GetShaderTexturesDefines(shaderToAtlas);

        for(int k = 0; k < shaderDefines.Count; k++) {//go trough each property of the shader.
            List<Texture2D> texturesOfShader = GetTexturesToAtlasForShaderDefine(shaderDefines[k]);//Get thtextures for the property shderDefines[k] to atlas them
            List<Vector2> scales = GetScalesToAtlasForShaderDefine(shaderDefines[k]);
            List<Vector2> offsets = GetOffsetsToAtlasForShaderDefine(shaderDefines[k]);
            if(SettingsMenuGUI.Instance.ReuseTextures) {
                texturesOfShader = Utils.FilterTexsByIndex(texturesOfShader, textureReuseManager.GetTextureIndexes());
                scales = Utils.FilterVec2ByIndex(scales, textureReuseManager.GetTextureIndexes());
                offsets = Utils.FilterVec2ByIndex(offsets, textureReuseManager.GetTextureIndexes());
            }
            generatedAtlas.SaveAtlasToFile(atlasTexturePath + k.ToString() + ".png", texturesOfShader, scales, offsets);//save the atlas with the retrieved textures
            AssetDatabase.ImportAsset(atlasTexturePath + k.ToString() + ".png");
            Texture2D tex = (Texture2D) AssetDatabase.LoadAssetAtPath(atlasTexturePath + k.ToString() + ".png", typeof(Texture2D));

            atlasMaterial.SetTexture(shaderDefines[k], //set property shderDefines[k] for shader shaderToAtlas
                                     tex);
        }
        return atlasMaterial;
    }

    //this method returns a list of texture2D by the textures defines of the shader of each object.
    private List<Texture2D> GetTexturesToAtlasForShaderDefine(string shaderDefine) {
        List<Texture2D> textures = new List<Texture2D>();
        for(int i = 0; i < objects.Count; i++) {//for each object lets get the shaderDefine texture.
            Texture2D texToAdd = ShaderManager.Instance.GetTextureForObjectSpecificShaderDefine(objects[i].ObjectMaterial, shaderDefine, true/*if null generate texture*/);
            textures.Add(texToAdd);
        }
        return textures;
    }

    private List<Vector2> GetScalesToAtlasForShaderDefine(string shaderDefine) {
        List<Vector2> scales = new List<Vector2>();
        for(int i = 0; i < objects.Count; i++) {//for each object lets get the shaderDefine texture.
            Vector2 scale = ShaderManager.Instance.GetScaleForObjectSpecificShaderDefine(objects[i].ObjectMaterial, shaderDefine);
            scales.Add(scale);
        }
        return scales;
    }
    private List<Vector2> GetOffsetsToAtlasForShaderDefine(string shaderDefine) {
        List<Vector2> offsets = new List<Vector2>();
        for(int i = 0; i < objects.Count; i++) {//for each object lets get the shaderDefine texture.
            Vector2 offset = ShaderManager.Instance.GetOffsetForObjectSpecificShaderDefine(objects[i].ObjectMaterial, shaderDefine);
            offsets.Add(offset);
        }
        return offsets;
    }

    public void ForceCalculateAproxAtlasSize() {
        cacheAtlasSizeReuseTextures = NO_CACHED;
        cacheAtlasSizeNoReuseTextures = NO_CACHED;
    }

    //calculates aprox atlas sizes with and without reusing textures
    //cacheAtlasSizeReuseTextures = NO_CACHED;
    //cacheAtlasSizeNoReuseTextures = NO_CACHED;
    public int CalculateAproxAtlasSize(bool reuseTextures, bool usePowerOf2Atlasses) {
        int aproxAtlasSize = 0;
        if(shaderName == "")//we dont need to calculate atlas size on non-optimizable objects
            return aproxAtlasSize;

        if(reuseTextures) {
            if(cacheAtlasSizeReuseTextures == NO_CACHED) {
                //atlas size reuse textures
                TextureReuseManager textureReuseManager = new TextureReuseManager();
                for(int i = 0; i < objects.Count; i++) {
                    if(objects[i] != null) {
                        if(!textureReuseManager.TextureRefExists(objects[i])) {
                            textureReuseManager.AddTextureRef(objects[i]);
                            aproxAtlasSize += objects[i].TextureArea;
                        }
                    }
                }
                cacheAtlasSizeReuseTextures = Mathf.RoundToInt(Mathf.Sqrt(aproxAtlasSize));
            }
            return usePowerOf2Atlasses ? Mathf.NextPowerOfTwo(cacheAtlasSizeReuseTextures) : cacheAtlasSizeReuseTextures;
        } else {
            if(cacheAtlasSizeNoReuseTextures == NO_CACHED) {
                //atlas size without reusing textures
                for(int i = 0; i < objects.Count; i++) {
                    if(objects[i] != null)
                        aproxAtlasSize += objects[i].TextureArea;
                }
                cacheAtlasSizeNoReuseTextures = Mathf.RoundToInt(Mathf.Sqrt(aproxAtlasSize));
            }
            return usePowerOf2Atlasses ? Mathf.NextPowerOfTwo(cacheAtlasSizeNoReuseTextures) : cacheAtlasSizeNoReuseTextures;
        }
    }
}
}