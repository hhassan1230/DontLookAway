using UnityEngine;
using System.Collections;

public class SenseSelfGaze : MonoBehaviour {

	bool isLookedAt;
	TextMesh textMesh;
	BoxCollider boxCollider;

	void Awake()
	{
		isLookedAt = false;
		textMesh = GetComponent<TextMesh> ();
		boxCollider = GetComponent<BoxCollider> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void AdvanceState()
	{
		textMesh.text = Random.Range (0, 100).ToString();
	}
}
