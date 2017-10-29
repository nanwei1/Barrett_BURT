using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A haptic sphere derived from the generic haptic object class. Make sure your
/// object has a SphereCollider that is set as a Trigger. Also make sure it is
/// tagged with the HapticObject tag.
/// </summary>
public class HapticSphereTouchLight : HapticSphere {

	private Light lt;
	private bool isOn = false;
	public GUIText lightText;
	private AudioSource audioSource;

	void Start() {
		lt = GetComponent<Light>();
		lt.color = Color.black;
		lightText.text = "Light's Off. Touch the sphere in the center to turn on.";
		audioSource = GetComponent<AudioSource>();
	}

	/// <summary>
	/// Sets the default stiffness and damping for this object.
	/// </summary>
	protected override void Awake () {
		stiffness = 100.0f;
		damping = 30.0f;
	}

	override protected void CalcForce (Collider player) {
		base.CalcForce (player);
	}

//	override protected void OnTriggerExit () {
//		base.OnTriggerExit ();
//		gameObject.GetComponent<Renderer>().material.color = Color.white;
//	}

	protected void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("HapticCursor")) {
			SwitchOnOff ();
		}

		if (isOn) {
			gameObject.GetComponent<Renderer> ().material.color = Color.green;
			lt.color = Color.white;
			lightText.text = "Light's On. Touch the sphere in the center to turn off.";
			audioSource.Play();
		} else {
			gameObject.GetComponent<Renderer> ().material.color = Color.grey;
			lt.color = Color.black;
			lightText.text = "Light's Off. Touch the sphere in the center to turn on.";
			audioSource.Pause();
		}
	}

	void SwitchOnOff (){
		isOn = !isOn;
	}
}
