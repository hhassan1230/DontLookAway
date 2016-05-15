using UnityEngine;
using System.Collections;

public class ShadeChanger : MonoBehaviour {
	public Material newMaterial;
	// Use this for initialization
	void Start () {
		Debug.Log ("I'm working");
//		ChangeColor (newMaterial);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeColor(Material currentMaterial)
	{
		GetComponent<Renderer>().material = currentMaterial;

	}
}
