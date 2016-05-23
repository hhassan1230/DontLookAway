using UnityEngine;
using System.Collections;

public class BlinkManager : MonoBehaviour {

	private GameObject topLid;
	private GameObject bottomLid;
	private bool blinking = false;
	private float originalTimerContainer;


	public float blinkTimer = 10f;

	// Use this for initialization
	void Start () 
	{
		originalTimerContainer = blinkTimer;
		topLid = GameObject.FindWithTag ("TopBlinker");
		bottomLid = GameObject.FindWithTag ("BottomBlinker");
	}
	
	// Update is called once per frame
	void Update () 
	{
		blinkTimer -= Time.deltaTime;

		if (blinkTimer <= 0)
		{
			blinkTimer = originalTimerContainer;
			blinking = true;
		}
		if(blinking)
		{
			blinking = false;
			topLid.GetComponent<Animator>().Play ("TopBlink");
			bottomLid.GetComponent<Animator>().Play ("BottomBlink");
		}

	}
}
