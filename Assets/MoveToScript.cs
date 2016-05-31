using UnityEngine;
using System.Collections;

public class MoveToScript : MonoBehaviour {
	public GameObject startPoint;
	public GameObject endPoint;
	public float speed;
	private RotateKnife knifeScript;


	private Vector3 startLocation;
	private Vector3 endLocation;
	private float step;
	private bool atLoction;

	// Use this for initialization
	void Start () {
		atLoction = false;
		startLocation = startPoint.transform.position;
		endLocation = endPoint.transform.position;
		this.gameObject.transform.position = startLocation;
		this.gameObject.transform.rotation = startPoint.transform.rotation;
		knifeScript = this.gameObject.GetComponent<RotateKnife> ();
	}
	
	// Update is called once per frame
	void Update () {
		step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, endLocation, step);
		if (transform.position == endLocation) {
			atLoction = true;
		}
		if (atLoction) {
			knifeScript.StopKnife ();
		}
	}
}
