using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	public GameObject frame;
	public Material currentPicture;
	public Material nextPicture;
	public Material[] pics;
	public GameObject moster;

	private int count;

	// Use this for initialization
	void Start () {
//		StartCoroutine(CallChangeColor(nextPicture));
		count = 0;
		LightningManager.lightningDelegate += FlickerToFifthState;
	}

	public void changePicture(){
		if(count < (pics.Length - 1)){
			currentPicture = pics [count];
			ChangeFrameColor (currentPicture);
			count++;
		}
		if (count == (pics.Length - 1)) {
			Instantiate (moster, new Vector3 (-2.73f, 1f, -0.1f), Quaternion.identity);
			count++;
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
}
