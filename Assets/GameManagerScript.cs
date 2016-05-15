using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {
	public GameObject frame;
	public Material nextPicture;

	// Use this for initialization
	void Start () {
		StartCoroutine(CallChangeColor(nextPicture));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator CallChangeColor(Material pic)
	{
		yield return new WaitForSeconds(2);		
		ChangeFrameColor(pic);
	}

	private void ChangeFrameColor(Material thePic)
	{
		ShadeChanger scriptChanger = (ShadeChanger) frame.GetComponent(typeof(ShadeChanger));
		scriptChanger.ChangeColor(thePic);
	}
}
