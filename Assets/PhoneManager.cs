using UnityEngine;
using System.Collections;

public class PhoneManager : MonoBehaviour {
	private GameObject phone;
	private GameObject player;
	private AudioSource phoneMessage;
	private Transform t;
	private Transform transformPlayer;
	private bool inReach;
	private bool hasBeenPlay;
	private float distance;

	[SerializeField]
	private float range = 10.0f;
	// Use this for initialization

	private void Awake()
	{
		t = GameObject.FindGameObjectWithTag ("Phone").transform;
		transformPlayer = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	void Start () {
		inReach = false;
		hasBeenPlay = false;
		phone = GameObject.FindGameObjectWithTag ("Phone");
		player = GameObject.FindGameObjectWithTag ("MainCamera");
		phoneMessage = phone.GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (transformPlayer) {
			print (player.name + " is " + Distance ().ToString () + " units from " + t.name);
			distance = Distance ();
			if (distance <= 2.9f){
				print("Im in range");
				inReach = true;
			}
		} else {
			print("Player not found!");
		}
	}

	public void Play911(){
		if (inReach) {
			if (!hasBeenPlay){
				phoneMessage.Play ();
				hasBeenPlay = true;
			}
		} else {
			print ("I can't reach that!");
		}
	}

	private float Distance()
	{
		return Vector3.Distance(t.position, transformPlayer.position);
	}
}
