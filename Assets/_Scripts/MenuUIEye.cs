using UnityEngine;
using System.Collections;

public class MenuUIEye : MonoBehaviour {

	public float tossRange = 300;
	private GameObject currentObjInView = null;

	public delegate void OnGrab();
	static public event OnGrab OnRollOverEvent;
	static public event OnGrab OnRollOffEvent;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		checkPlayerSightRay ();

		const int lmb = 0; //left mouse button
		bool bInputFireHook = (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(lmb) );
		bool bInputReleaseHook = (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(lmb) );

		if(bInputFireHook && currentObjInView){

			SightButton script = currentObjInView.GetComponent<SightButton>();
			script.OnTapDown();
		}else if(bInputReleaseHook && currentObjInView){

			SightButton script = currentObjInView.GetComponent<SightButton>();
			script.OnTapUp();
		}
	}
	
	bool checkPlayerSightRay(){
		Ray ray;
		RaycastHit hit;
		
		ray = new Ray(transform.position, transform.forward);
		
		if (Physics.Raycast (ray, out hit, tossRange)) {	
			
			// selectible object in sight
			// get object a highlight

			if(isSelectableObj(hit.collider.gameObject)){

				if(currentObjInView != hit.collider.gameObject){
	
					currentObjInView = hit.collider.gameObject;
					SightButton script = currentObjInView.GetComponent<SightButton>();
					script.RollOver();
					//OnRollOverEvent();
				}
			}else{

				//OnRollOffEvent();
				SightButton script = currentObjInView.GetComponent<SightButton>();
				script.RollOut();
				
				currentObjInView = null;
			}
			
			return true; // HIT
			
		} else {
			
			if( currentObjInView){
			
				//OnRollOffEvent();
				SightButton script = currentObjInView.GetComponent<SightButton>();
				script.RollOut();
			
				currentObjInView = null;
			}
	
			return false; // NO HIT
			// no selectible object
		}
	}

	// DOES the object have the correct script
	bool isSelectableObj(GameObject go){
		SightButton script = go.GetComponent<SightButton>();
		if (script != null) {
			//do stuff with script
			return true;
		} else {
			return false;
		}
	}
}

 
