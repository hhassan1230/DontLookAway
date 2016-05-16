using UnityEngine;
using System.Collections;

public class CenterWallCollider : MonoBehaviour {

	void OnTriggerEnter(Collider otherCollider)
	{
		if(	otherCollider.gameObject.name == "PlayerTopCollider" 	||
			otherCollider.gameObject.name == "PlayerBottomCollider" ||
			otherCollider.gameObject.name == "PlayerLeftCollider" 	||
			otherCollider.gameObject.name == "PlayerRightCollider")
		{
			transform.parent.GetComponent<WallManager> ().SetBeingLookedAt (true);
		}
	}
}