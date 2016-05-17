using UnityEngine;
using System.Collections;

public class ReactToOtherGaze : MonoBehaviour {

	bool isLookedAt;
	TextMesh textMesh;

	void Awake()
	{
		isLookedAt = false;
		textMesh = GetComponent<TextMesh> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AdvanceState()
	{
		if(isLookedAt == true)
		{
			textMesh.text = Random.Range (0, 100).ToString();
		}
	}
}
