using UnityEngine;
using System.Collections;

public class gazegun : MonoBehaviour {

	public Rigidbody prefab;
	int delay = 0;
	int resetTime = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		delay--;

		if (delay < 0) {

			Rigidbody clone = Instantiate (prefab, transform.position, transform.rotation) as Rigidbody;
			clone.AddForce( transform.forward * 400);
			delay = resetTime;
		}
	}
}
