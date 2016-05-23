using UnityEngine;
using System.Collections;

public class LightningManager : MonoBehaviour {

	public static LightningManager instance;

	public delegate void LightningDelegate(bool newState = false);
	public static LightningDelegate lightningDelegate;

	public AudioClip[] thunderSoundArray;
	AudioSource audioSource;

	float lightningCountdown;
	float thunderCountdown;
	float thunderVolumeIntensity;

	float minLightningIntensity = 2f;
	float maxLightningIntensity = 6f;

	GameObject lightningGameObject;
	Light lightningLight;


	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		lightningGameObject = GameObject.Find ("LightningDirectionalLight");
		lightningLight = lightningGameObject.GetComponent<Light>();
		lightningGameObject.SetActive (false);
		audioSource = GetComponent<AudioSource> ();
		ResetLightningCountdown ();
		Invoke ("TurnOnLightning", lightningCountdown);
	}

	void TurnOnLightning()
	{
		float lightningIntensity = Random.Range (minLightningIntensity, maxLightningIntensity);
		lightningGameObject.SetActive (true);
		lightningLight.intensity = lightningIntensity;
		thunderVolumeIntensity = lightningIntensity / maxLightningIntensity;
		thunderCountdown = (maxLightningIntensity / lightningIntensity);// - 1f;
		if(thunderCountdown < 0f)
		{
			thunderCountdown = 0f;
		}
		if(lightningDelegate != null)
		{
			lightningDelegate (true);	//this may or may not have a function in it that triggers something else in the room
		}
		Invoke("PlayThunder", thunderCountdown);
		Invoke ("TurnOffLightning", 0.5f);
	}

	void TurnOffLightning()
	{
		lightningGameObject.SetActive (false);
		if (lightningDelegate != null) 
		{
			lightningDelegate (false);
		}
		ResetLightningCountdown ();
		Invoke ("TurnOnLightning", lightningCountdown);
	}

	void StopRandomLightningSequence()
	{
		CancelInvoke ("SetRandomLightningSequence");
	}

	void ResetLightningCountdown()
	{
		lightningCountdown = Random.Range (5f, 15f);
	}

	void PlayThunder()
	{
		//play a random thunder audio clip at a variable volume in relation to the brigthness of the lightning
		//also must include a delay in playing the sound based on the same value
		AudioClip randomClip = thunderSoundArray[Random.Range(0, thunderSoundArray.Length)];
		audioSource.PlayOneShot (randomClip, thunderVolumeIntensity);
	}
}
