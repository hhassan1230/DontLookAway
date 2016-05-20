using UnityEngine;
using System.Collections;

public class ClickTest : MonoBehaviour {
	public GameObject script;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")){
			GameManagerScript scriptChanger = (GameManagerScript) script.GetComponent(typeof(GameManagerScript));

			scriptChanger.changePicture();
		}
	
	}
}
