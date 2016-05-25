using UnityEngine;
using System.Collections;

public class ClickTest : MonoBehaviour {
	public GameObject script;
	private GameManagerScript scriptChanger;
	private PhoneManager phoneScript;

	// Use this for initialization
	void Start () {
		scriptChanger = (GameManagerScript) script.GetComponent(typeof(GameManagerScript));
		phoneScript = (PhoneManager) script.GetComponent(typeof(PhoneManager));
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")){
//			GameManagerScript scriptChanger = (GameManagerScript) script.GetComponent(typeof(GameManagerScript));
			phoneScript.Play911();
			scriptChanger.changePicture();
		}
	
	}
}
