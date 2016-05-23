using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	public static GameManagerScript instance;

	public GameObject frame;
	public Material currentPicture;
	public Material nextPicture;
	public Material[] pics;
	public GameObject moster;
	public float colliderActiveDelay = 6f;

	public GameObject justOverLeftShoulderCollider;
	public GameObject krasueSpawnPointFrontFace;
	public GameObject krasueSpawnPointBehindLeft;
	public GameObject krasueSpawnPointBehindRight;
	public GameObject krasueSpawnPointNearPortrait;

	private int count;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
//		StartCoroutine(CallChangeColor(nextPicture));
		count = 0;
		DeactivatePlayerColliders ();
		Invoke ("ActivatePlayerColliders", colliderActiveDelay);
		LightningManager.lightningDelegate += FlickerToFifthState;
		currentPicture = pics [count];
		krasueSpawnPointFrontFace = GameObject.Find ("KrasueSpawnPointFrontFace");
		krasueSpawnPointBehindLeft = GameObject.Find ("KrasueSpawnPointBehindLeft");
		krasueSpawnPointBehindRight = GameObject.Find ("KrasueSpawnPointBehindRight");
		krasueSpawnPointNearPortrait = GameObject.Find ("KrasueSpawnPointNearPortrait");

		justOverLeftShoulderCollider.SetActive (false);
	}

	public void changePicture(){
		if(count < (pics.Length - 1)){
			count++;
			currentPicture = pics [count];
			ChangeFrameColor (currentPicture);
		}
		if (count == (pics.Length - 1)) {
			Instantiate (moster, krasueSpawnPointNearPortrait.transform.position, Quaternion.identity);
			//count++;
		}
	}

	public void FlickerToFifthState(bool newState)
	{
		//this function is intended to flicker the image back between the current texture and the last
		if(newState == true)
		{
			currentPicture = pics [count];
			ChangeFrameColor (pics[pics.Length - 1]);
		}
		else
		{
			ChangeFrameColor (currentPicture);
			//uncomment this next line if you wat the picture to flicker each time the lightning flashes,
			//instead of just the first time
			LightningManager.lightningDelegate -= FlickerToFifthState;
		}
	}

	IEnumerator CallChangeColor(Material pic)
	{
		yield return new WaitForSeconds(1);		
		ChangeFrameColor(pic);
	}

	private void ChangeFrameColor(Material thePic)
	{
		ShadeChanger scriptChanger = (ShadeChanger) frame.GetComponent(typeof(ShadeChanger));
		scriptChanger.ChangeColor(thePic);

	}

	void ActivatePlayerColliders()
	{
		PlayerScript.instance.ActivatePlayerColliders ();
	}

	void DeactivatePlayerColliders()
	{
		PlayerScript.instance.DeactivatePlayerColliders ();
	}

	public void JustOverLeftShoulderColliderSetActive()
	{
		justOverLeftShoulderCollider.SetActive (true);
	}

	public void JustOverLeftShoulderColliderSetDeactive()
	{
		justOverLeftShoulderCollider.SetActive (false);
	}
}
