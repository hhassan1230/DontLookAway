using UnityEngine;
using System.Collections;

public class WallManager : MonoBehaviour {

	bool isVisible;
	GameObject textGameObject;
	TextMesh textMesh;

	void Awake()
	{
		isVisible = false;
		textGameObject = transform.FindChild ("New Text").gameObject;
		textMesh = textGameObject.GetComponent<TextMesh> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetBeingLookedAt(bool newIsVisible)
	{
		if(isVisible == true && newIsVisible == false)	//if the wall was being looked at, but now is not being looked at
		{
			AdvanceWallState ();
		}
		isVisible = newIsVisible;
	}

	public void AdvanceWallState()
	{
		textMesh.text = Random.Range (0, 100).ToString();
	}
}
