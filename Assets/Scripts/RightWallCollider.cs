using UnityEngine;
using System.Collections;

public class RightWallCollider : MonoBehaviour {

	void OnTriggerEnter(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "PlayerLeftCollider")
		{
			transform.parent.GetComponent<WallManager> ().SetBeingLookedAt (false);
		}
	}
}
