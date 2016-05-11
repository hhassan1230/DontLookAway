using UnityEngine;
using System.Collections;


public class SelectByTrigger : MonoBehaviour {

	// Select count down
	float timeleft = 0;
	float selectTime = 1.0F;

	public TextMesh textBox;

	void OnTriggerEnter( Collider other){

		other.gameObject.GetComponent<Renderer>().material.color = Color.red;

		// Set start timer
		timeleft = selectTime;

	}

	void OnTriggerExit( Collider other){

		textBox.gameObject.SetActive( false ) ;

		other.gameObject.GetComponent<Renderer>().material.color = Color.blue;
	}

	void OnTriggerStay( Collider other ){

		timeleft -= Time.deltaTime;
		textBox.gameObject.SetActive( true ) ;

		textBox.text = timeleft.ToString ("0.0");

		// count down
		if (timeleft < 0) {
			// kill cube
			textBox.gameObject.SetActive( false ) ;
			Destroy(other.gameObject);
		}

		if(Input.GetKeyDown(KeyCode.Space)){

			Destroy(other.gameObject);
		}
	}
	
}
