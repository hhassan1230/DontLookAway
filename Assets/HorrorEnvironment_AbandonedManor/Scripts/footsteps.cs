using UnityEngine;
using System.Collections;

public class footsteps : MonoBehaviour {
	
	public AudioSource a;
	private CharacterController controller;
	
	private float footstepTime = 1f;
	
	void Start()
	{
		controller = gameObject.GetComponent< CharacterController >();
	}
	
	void Update()
	{
		if( controller.velocity.magnitude > 0.2f && !a.isPlaying && footstepTime <= 0f )
		{
			a.Play();
			footstepTime = 1f;
		}
		
		footstepTime -= Time.fixedDeltaTime;
	}
}
