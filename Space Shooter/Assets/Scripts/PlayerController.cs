using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Boundary
{
	public float xmin, xmax, zmin, zmax;
}

public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	public float speed = 10;
	public float tilt=4;
	public Boundary boundary;
	public GameObject shot;
	// public Transform shotSpawn;
	public float fireRate;
	private float nextFire=0;
	private AudioSource audioSource;
	/// Scales the robot position to more closely appoximate the Unity workspace for this game.
	private const float positionScale = 30.0f;
	private const float xPosOffset = 8.0f;
	private const float zPosOffset = -5.0f;

	/// Robot state
	private Vector3 tool_position;
	private Vector3 tool_velocity = Vector3.zero;
	private Vector3 tool_force = Vector3.zero;
	public float damping = 1;
	private bool dampingFlag = false;
	public GUIText dampingText;

	/// Handles communication with the robot. Use exactly one RobotConnection object in Unity
	/// program, and make sure to set it up in the first scene that will be active.
	Barrett.UnityInterface.RobotConnection robot;
	void Awake () {
		robot = GameObject.Find ("RobotConnection").GetComponent<Barrett.UnityInterface.RobotConnection> ();
		Debug.Log (robot);
		dampingText.text = "Damping is OFF. Press D to toggle.";
	}



	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
	}

	void Update()
	{
		if (Time.time >= nextFire)
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, rb.position, rb.rotation);
			audioSource.Play ();
		}

		if (Input.GetKeyDown (KeyCode.D))
		{
			toggleDamping ();
		}
//		if (Input.GetKey (KeyCode.Space)&& Time.time >= nextFire)
//		{
//			nextFire = Time.time + fireRate;
//			// GameObject clone = Instantiate (projectile, rb.position, rb.rotation) as GameOBject;
//			Instantiate (shot, rb.position, rb.rotation);
//			audioSource.Play ();
//		}
	}

	void FixedUpdate()
	{
		tool_position = positionScale * robot.GetToolPosition ();
		tool_velocity = positionScale * robot.GetToolVelocity ();
		transform.position = tool_position;
		rb.position = new Vector3 
		(
				Mathf.Clamp(rb.position.x + xPosOffset, boundary.xmin, boundary.xmax),
				0.0f,
				Mathf.Clamp(rb.position.z + zPosOffset, boundary.zmin, boundary.zmax)
		);
		rb.rotation = Quaternion.Euler (0, 0, tool_velocity[0]*-tilt);
		if (dampingFlag) {
			tool_force = -damping * tool_velocity;
		} else {
			tool_force = Vector3.zero;
		}
		robot.SetToolForce (tool_force / positionScale);
	}

	void toggleDamping() {
		dampingFlag = !dampingFlag;
		if (dampingFlag) {
			dampingText.text = "Damping is ON. Press D to toggle.";
		} else {
			dampingText.text = "Damping is OFF. Press D to toggle.";
		}
	}


}
