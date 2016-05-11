using UnityEngine;
//using UnityEditor;
using System.Collections;

public class ThrowObject : MonoBehaviour
{

	public GameObject projectile;
	public AudioClip shootSound;
	private float timerCount;
	private float BallCount = 0f;



	private float throwSpeed = 1000f;
	private AudioSource source;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;

	void Start ()
	{
		
		//BallCount = PlayerPrefs.GetFloat ("LowScore", 0.0f);
	}

	void Awake ()
	{
		//0000Debug.Log ("start"+ BallCount);
		source = GetComponent<AudioSource> ();

	}

	void OnDestroy ()
	{
		Debug.Log ("done" + BallCount);
		float savedScore = 9000;
		if (PlayerPrefs.HasKey ("LowScore")) {
			savedScore = PlayerPrefs.GetFloat ("LowScore");
		}
		if (BallCount < savedScore) {
				
			// put Ballcount in a global variable
			PlayerPrefs.SetFloat ("LowScore", BallCount);
			PlayerPrefs.Save ();
		}
	}


	void Update ()
	{

		if (Input.GetButtonDown ("Fire1")) {
			float vol = Random.Range (volLowRange, volHighRange);
			source.PlayOneShot (shootSound, vol);
			GameObject throwThis = Instantiate (projectile, transform.position, transform.rotation) as GameObject;
			throwThis.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (0, 0, throwSpeed));
			BallCount = BallCount + 1;
			Debug.Log (BallCount);

		}

	}
}