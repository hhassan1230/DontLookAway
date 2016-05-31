using UnityEngine;
using System.Collections;

public class RotateKnife : MonoBehaviour {
	public int spinSpeed; 

	private bool stop;
	// Use this for initialization
	void Start () {
		stop = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!stop) {
			transform.Rotate(spinSpeed*Time.deltaTime,0,0);
		}

	}

	public void StopKnife(){
		stop = true;
	}
}
