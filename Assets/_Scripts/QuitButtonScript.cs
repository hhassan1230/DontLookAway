using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuitButtonScript : MonoBehaviour {

	float totalTime;
	float requiredTime;
	bool isLooking;

	void Start()
	{
		totalTime = 0f;
		requiredTime = 2f;
	}

	void Update()
	{
		if(isLooking == true)
		{
			totalTime += Time.deltaTime;
			Debug.Log ("totalTime is " + totalTime);
			if(totalTime >= requiredTime)
			{
				totalTime = 0f;
				QuitGame ();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "PlayerGazeCollider")
		{
			Debug.Log ("OnTriggerEnter");
			isLooking = true;
		}
	}

	void OnTriggerLeave(Collider other)
	{
		if(other.gameObject.name == "PlayerGazeCollider")
		{
			Debug.Log ("OnTriggerLeave");
			isLooking = false;
			totalTime = 0f;
		}
	}

	public void QuitGame()
	{
		Debug.Log ("Quitting game");
		Application.Quit ();
	}

}
