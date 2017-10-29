using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticPushButton : HapticBox {

	private bool buttonOnOff = false;
	private bool buttonChangeFlag = false;  
	private AudioSource audioSource;
	public Light dirLight;

	override protected void Awake () {
		stiffness = 100.0f;
		damping = 40.0f;
		texturePatchSize = 0.0f;
		textureDamping = 0.0f;
		textureDampFreq = 0.0f;
		audioSource = GetComponent<AudioSource>();
	}

	
	override protected Vector3 CalcDepth (Collider other) {
		Vector3 depth = base.CalcDepth(other);
		if (this.direction [2] == 1.0f && depth[2] < -0.6) {
			// Debug.Log("Pushing button now");
			// Debug.Log (depth);
			// Debug.Log("Button is pressed");
			buttonChangeFlag = true;
		}
		return depth;
	}

	protected override void OnTriggerExit ()
	{
		base.OnTriggerExit ();
		// Debug.Log ("Need to swith button status");
		if (buttonChangeFlag) {
			switchButton ();
			buttonChangeFlag = false;
			// Debug.Log (this.gameObject.transform.position);
			// Debug.Log (this.gameObject.transform.localScale);
		}
	}

	void switchButton(){
		buttonOnOff = ! buttonOnOff;
		Vector3 newPos = this.gameObject.transform.position;
		Vector3 newScale = this.gameObject.transform.localScale;

		if (buttonOnOff) {
			newPos.z = 7.25f;
			newScale.z = 1.5f;
			gameObject.GetComponent<Renderer> ().material.color = Color.green;
			dirLight.intensity = 1.0f;
		} else {
			newPos.z = 7.0f;
			newScale.z = 2.0f;
			gameObject.GetComponent<Renderer> ().material.color = Color.red;
			dirLight.intensity = 0.0f;
		}
		this.gameObject.transform.localScale = newScale;
		this.gameObject.transform.position = newPos;
		audioSource.Play();
	}
}
