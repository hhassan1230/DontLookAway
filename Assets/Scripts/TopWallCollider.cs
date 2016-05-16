using UnityEngine;
using System.Collections;

public class TopWallCollider : MonoBehaviour {

	/*
	 * This script will be placed on the matching collider of the wall
	 * it senses which player collider makes contact with it
	 * If the player's opposite collider makes contact with it, then
	 * this script will send a signal to its parent (the wall object) and tell that it is not being looked at anymore
	 */

	void OnTriggerEnter(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "PlayerBottomCollider")
		{
			transform.parent.GetComponent<WallManager> ().SetBeingLookedAt (false);
		}
	}
}
