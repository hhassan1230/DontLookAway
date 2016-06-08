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
		transform.LookAt (playerObject.transform.position);
		Quaternion tempRotation = transform.rotation;
		tempRotation.y -= 90f;
		transform.rotation = tempRotation;
	}
}
