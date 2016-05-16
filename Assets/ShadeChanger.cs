using UnityEngine;
using System.Collections;

public class ShadeChanger : MonoBehaviour {
	public Material newMaterial;
	// Use this for initialization
	void Start () {
		Debug.Log ("I'm working");
	}

	public void ChangeColor(Material currentMaterial)
	{
		GetComponent<Renderer>().material = currentMaterial;
	}
}
