using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	float speed = 50;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKey(KeyCode.A)){
			// move left
			this.transform.position += Vector3.left * speed * Time.deltaTime;
		
		}else if(Input.GetKey(KeyCode.D)){
			// move right
			this.transform.position += Vector3.right * speed * Time.deltaTime;
		
		}else if(Input.GetKey(KeyCode.W)){
			// move UP 
			this.transform.position += Vector3.forward * speed * Time.deltaTime;
		
		}else if(Input.GetKey(KeyCode.S)){

			// move Down
			this.transform.position += Vector3.back * speed * Time.deltaTime;
		}
	}
}
