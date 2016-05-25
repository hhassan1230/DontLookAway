using UnityEngine;
using System.Collections;

public class KrasueScript : MonoBehaviour {

	GameObject playerObject;

	void Start()
	{
		playerObject = null;
		playerObject = GameObject.Find ("Main Camera");
		if(playerObject != null)
		{
			LookAtPlayer ();
		}
	}

	void OnEnable()
	{
		if(playerObject != null)
		{
			LookAtPlayer ();
		}
	}

	void LookAtPlayer()
	{
		Debug.Log ("Before: " + transform.rotation.ToString());
		transform.LookAt (playerObject.transform.position);
		Debug.Log ("After: " + transform.rotation.ToString());
		Debug.Log ("Finished LookAtPlayer");
	}
}
