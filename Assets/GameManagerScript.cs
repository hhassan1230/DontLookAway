using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	public GameObject frame;
	public Material nextPicture;
	public Material[] pics;

	private int count;

	// Use this for initialization
	void Start () {
//		StartCoroutine(CallChangeColor(nextPicture));
		count = 0;
	}
	
	void Update () {
	
	}

	public void changePicture(){
		if(count < 4){
			Material texture = pics [count];
			ChangeFrameColor (texture);
			count++;
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
