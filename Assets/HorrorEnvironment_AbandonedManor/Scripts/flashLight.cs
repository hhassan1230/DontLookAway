using UnityEngine;
using System.Collections;

public class flashLight : MonoBehaviour {

	private Light fL;
	
	void Start()
	{
		fL = gameObject.GetComponent< Light >();
	}
	
	void Update()
	{
		if( Input.GetKeyDown( KeyCode.F ) )
		{
			if( fL != null ) fL.enabled = !fL.enabled;
		}
	}
}
