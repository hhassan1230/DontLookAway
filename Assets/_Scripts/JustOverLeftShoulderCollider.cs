using UnityEngine;
using System.Collections;

public class JustOverLeftShoulderCollider : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "PlayerLeftCollider" || other.gameObject.name == "PlayerRightCollider")
		{
			GameManagerScript.instance.changePicture();
			gameObject.SetActive (false);
		}
	}
}