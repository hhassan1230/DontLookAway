using UnityEngine;
using System.Collections;

public class Candle : MonoBehaviour {

	GameObject candleLightGameObject;
	Light candleLightLight;

	GameObject flame;
	ParticleSystem flameParticle;

	public float fadeInTime = 1f;
	public float fadeOutTime = 1f;

	float candleStartingIntensity;
	float flameStartSize;

	void Start()
	{
		candleLightGameObject = transform.FindChild("Candle Light").gameObject;
		candleLightLight = candleLightGameObject.GetComponent<Light> ();
		flame = transform.FindChild ("Flame").gameObject;
		flameParticle = flame.GetComponent<ParticleSystem> ();
		candleStartingIntensity = candleLightLight.intensity;
		flameStartSize = flameParticle.startSize;
		TurnOff ();
		Invoke ("FadeIn", 6f);
	}

	public void FadeOut()
	{
		StartCoroutine("FadeOutCoroutine");
	}

	public void FadeIn()
	{
		StartCoroutine("FadeInCoroutine");
	}

	public IEnumerator FadeOutCoroutine()
	{
		Vector3 tempFlameScale;
		for (float i = fadeOutTime; i > 0f; i -= Time.deltaTime / 1.0f) {
			candleLightLight.intensity = (i / fadeOutTime) * candleStartingIntensity;
			flameParticle.startSize = (i / fadeOutTime) * flameStartSize;
			yield return null;
		}
		flameParticle.startSize = 0f;
		candleLightLight.intensity = 0f;
	}

	public IEnumerator FadeInCoroutine()
	{
		Vector3 tempFlameScale;
		for (float i = 0f; i < fadeInTime; i += Time.deltaTime / 1.0f) {
			candleLightLight.intensity = (i / fadeInTime) * candleStartingIntensity;
			flameParticle.startSize = (i / fadeInTime) * flameStartSize;
			yield return null;
		}
		flameParticle.startSize = flameStartSize;
		candleLightLight.intensity = candleStartingIntensity;
	}

	void TurnOff()
	{
		flameParticle.startSize = 0f;
		candleLightLight.intensity = 0f;
	}

	void TurnOn()
	{
		flameParticle.startSize = flameStartSize;
		candleLightLight.intensity = candleStartingIntensity;
	}
}
