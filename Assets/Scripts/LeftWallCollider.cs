using UnityEngine;
using System.Collections;

public class LeftWallCollider : MonoBehaviour {

	void OnTriggerEnter(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "PlayerRightCollider")
		{
			transform.parent.GetComponent<WallManager> ().SetBeingLookedAt (false);
		}
	}
}
