using UnityEngine;
using System.Collections;

public class EyeLidManager : MonoBehaviour {
	private BlinkManager blinkManagerScript;
	private GameManagerScript gameManagerScript;

	private GameObject scriptsManager;
	// Use this for initialization
	void Start () {
		scriptsManager = GameObject.Find ("Scripts");
		print (scriptsManager);
		gameManagerScript = scriptsManager.GetComponent<GameManagerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LidClose(){
		gameManagerScript.LidClose ();
	}
}
