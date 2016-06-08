using UnityEngine;
using System.Collections;

public class ShadeChanger : MonoBehaviour {
	public Material newMaterial;

	public void ChangeColor(Material currentMaterial)
	{
		GetComponent<Renderer>().material = currentMaterial;
	}
}
