using UnityEngine;
using System.Collections;

public class SelectByKeyPress : MonoBehaviour {

	void OnTriggerEnter( Collider other){

		other.gameObject.GetComponent<Renderer>().material.color = Color.red;
	}

	void OnTriggerExit( Collider other){

		other.gameObject.GetComponent<Renderer>().material.color = Color.blue;
	}

	void OnTriggerStay( Collider other ){

		if(Input.GetKeyDown(KeyCode.Space)){

			Destroy(other.gameObject);
		}
	}
	
}
