using UnityEngine;
using System.Collections;

public class JustOverLeftShoulderCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void TriggerNextAction()
	{
		//here you will call the next action
		GameManagerScript.instance.changePicture();
	}
}
