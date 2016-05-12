using UnityEngine;
using System.Collections;

public class SightButton : MonoBehaviour {


	public string levelName = "lvl_conveyor_belt_of_doom" ;
	


	private Vector3 zeroOffset = new Vector3 (0, 0, 0);
	private Vector3 activeOffset  = new Vector3(0.0f,0.0f,-0.3f);
	private Vector3 lerpVec = new Vector3(0,0,0);
	private Vector3 initPos;
	private Vector3 offset = new Vector3(0,0,0);

	public TextMesh title;

	// Use this for initialization
	void Start () {
		initPos = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		offset = Vector3.Lerp (offset, lerpVec, 3.0f * Time.deltaTime);
		this.transform.localPosition = initPos + offset;

		title.text = levelName;
	}

	public void RollOver(){

		GetComponent<Renderer> ().material.color = Color.red;
		lerpVec = activeOffset;

	}

	public void RollOut(){

		GetComponent<Renderer> ().material.color = Color.blue;
		lerpVec = zeroOffset;
	}

	public void OnTapDown(){

		GetComponent<Renderer> ().material.color = Color.yellow;


	}

	public void OnTapUp(){
		GetComponent<Renderer> ().material.color = Color.magenta;
		loadScene (levelName);
	}

	private void loadScene( string sceneName){

		if (sceneName.ToLower () == "reload") {
			Application.LoadLevel (Application.loadedLevel);
		} else {
			Application.LoadLevel (sceneName);
		}
	}
}
