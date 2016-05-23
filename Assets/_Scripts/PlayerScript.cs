using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

	public static PlayerScript instance;

	GameObject topCollider;
	GameObject rightCollider;
	GameObject bottomCollider;
	GameObject leftCollider;
	List<GameObject> colliderList;


	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		colliderList = new List<GameObject> ();
		topCollider = transform.FindChild ("PlayerTopCollider").gameObject;
		rightCollider = transform.FindChild ("PlayerRightCollider").gameObject;
		bottomCollider = transform.FindChild ("PlayerBottomCollider").gameObject;
		leftCollider = transform.FindChild ("PlayerLeftCollider").gameObject;

		colliderList.Add (topCollider);
		colliderList.Add (rightCollider);
		colliderList.Add (bottomCollider);
		colliderList.Add (leftCollider);
	}

	public void ActivatePlayerColliders()
	{
		for(int i = 0; i < colliderList.Count; i++)
		{
			colliderList [i].SetActive (true);
		}
	}

	public void DeactivatePlayerColliders()
	{
		for(int i = 0; i < colliderList.Count; i++)
		{
			colliderList [i].SetActive (false);
		}
	}

}
