using UnityEngine;
using System.Collections;

public class RotateGO : MonoBehaviour {


	// Update is called once per frame
	void Update () {



		transform.Rotate(Vector3.left * 60.0f * Time.deltaTime);
	}
}
