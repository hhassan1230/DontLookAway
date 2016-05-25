using UnityEngine;
using System.Collections;

public class PhoneManager : MonoBehaviour {
	private GameObject phone;
	private GameObject player;
	private AudioSource phoneMessage;
	// Use this for initialization
	void Start () {
		phone = GameObject.FindGameObjectWithTag ("Phone");
		player = GameObject.FindGameObjectWithTag ("MainCamera");
		phoneMessage = phone.GetComponent<AudioSource>();
	}

	public void Play911(){
		phoneMessage.Play ();
	}
}
