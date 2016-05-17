using UnityEngine;
using System.Collections;

public class BottomWallCollider : MonoBehaviour {

	void OnTriggerEnter(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "PlayerTopCollider")
		{
			transform.parent.GetComponent<WallManager> ().SetBeingLookedAt (false);
		}
	}
}
