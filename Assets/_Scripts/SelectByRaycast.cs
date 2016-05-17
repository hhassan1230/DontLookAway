using UnityEngine;
using System.Collections;

public class SelectByRaycast : MonoBehaviour {



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawLine(transform.position, transform.position + transform.forward * 50.0F , Color.green , 0.1F);

		// draw a ray

		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.position + transform.forward * 50.0F , out hit, 50.0F)) {

		
			hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;


		}




		// see if it touches any thing 
		// then change color
		// maybe add a pointer


	}
}
