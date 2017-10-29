using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A haptic box derived from the generic haptic object class. Make sure your
/// object has a BoxCollider that is set as a Trigger. Also make sure it is
/// tagged with the HapticObject tag.
///
/// Note that the player can "pop through" the box if the player penetrates
/// the box past the center.
/// </summary>
public class HapticBox : HapticObject {

	/// The (unsigned) direction of the force, based on which side the player
	/// entered the box from.
	protected Vector3 direction = Vector3.zero;
	private Vector3 tan_direction = Vector3.zero;
	public float texturePatchSize;
	public float textureDamping;
	public float textureDampFreq;
	/// <summary>
	/// Sets the default stiffness and damping for this object.
	/// </summary>
	virtual protected void Awake () {
		stiffness = 300.0f;
		damping = 60.0f;
		texturePatchSize = 0.025f;
		textureDamping = 8.0f;
		textureDampFreq = 5.0f;
	}

	/// <summary>
	/// Raises the trigger enter event. In this case, we need to know which side
	/// the player entered the box on and save that until the player exits the box.
	/// </summary>
	/// <param name="player">The collider associated with the player object.</param>
	void OnTriggerEnter (Collider other) {
		Vector3 depth = CalcDepth (other);

		// Find the index of the closest side.
		int index = 0;
		float minDepth = Mathf.Abs (depth [0]);
		for (int i = 0; i < 3; i++) {
			if (Mathf.Abs (depth [i]) < minDepth) {
				minDepth = Mathf.Abs (depth [i]);
				index = i;
			}
		}
		// Debug.Log (index);

		/// Define a unit vector for the direction of the force. This does not need to
		/// keep track of the sign because that information is captured in the depth.
		direction = Vector3.zero;
		direction [index] = 1.0f;
		tan_direction = Vector3.one;
		tan_direction [index] = 0.0f;
	}

	/// <summary>
	/// Calculates the force based on the collider information from the Player object.
	/// </summary>
	/// <param name="player">The collider associated with the player object.</param>
	override protected void CalcForce (Collider other) {
		if (other.gameObject.CompareTag ("HapticCursor")) {
			Vector3 depth = CalcDepth (other);

			/// Calculate the stiffness force (pushes outward). Allows pop-through.
			force = stiffness * Vector3.Dot (depth, direction) * direction;

			/// Get the velocity of the Player object and add the damping force, which
			/// is needed for stability. This pushes against velocity in the direction
			/// of the closest wall (+ or -).
			Vector3 playerVelocity;
			if (other.gameObject.name.Equals ("PlayerLeft")) {
				playerVelocity = other.gameObject.GetComponent<RobotController> ().GetVelocity ();
			} else {
				playerVelocity = other.gameObject.GetComponent<RobotControllerRight> ().GetVelocity ();
			}
			force += -damping * Vector3.Dot (playerVelocity, direction) * direction;

			//// add force to texture rendering
			Vector3 playerPos = other.gameObject.transform.position;
			Vector3 thisPos = this.gameObject.transform.position;
			Vector3 distFromCenter = playerPos - thisPos;
			float projOnTan = Mathf.Abs (Vector3.Dot (distFromCenter, tan_direction));
			//		if (Mathf.Floor (projOnTan / texturePatchSize) % 2 > 0) {
			//			force += -textureDamping * Vector3.Dot (playerVelocity, tan_direction) * tan_direction;
			//		}
			force += -textureDamping * Vector3.Dot (playerVelocity, tan_direction) * tan_direction * Mathf.Sin (projOnTan * 2 * Mathf.PI * textureDampFreq);
		}
	}

	/// <summary>
	/// Calculates the penetration depth.
	/// 
	/// Assumes that the collider size is set equal to the box size, i.e., the Size
	/// parameter under the Box Collider is set to (1, 1, 1). If not, you can access
	/// the scale of the Box Collider with
	///   player.GetComponent<BoxCollider> ().size
	/// and multiply boxSize element-wise.
	/// 
	/// Also assumes that the player is a sphere.
	/// </summary>
	/// <returns>The depth.</returns>
	/// <param name="player">The collider associated with the player object.</param>
	virtual protected Vector3 CalcDepth (Collider other) {
		Vector3 depth = Vector3.zero;
		if (other.gameObject.CompareTag ("HapticCursor")) {
			/// Get the box size (distances from box center to each face).
			Vector3 boxSize = this.gameObject.transform.localScale / 2.0f;

			/// Get the player size.
			Vector3 playerDims = other.gameObject.transform.localScale;
			float playerRad = other.GetComponent<SphereCollider> ().radius *
			                  Mathf.Max (playerDims.x, playerDims.y, playerDims.z);

			/// Get the vector from the center of this object to the center of the player
			/// object.
			Vector3 playerPos = other.gameObject.transform.position;
			Vector3 thisPos = this.gameObject.transform.position;
			Vector3 distFromCenter = playerPos - thisPos;

			/// Handle the case where the player object is near the center of the box,
			/// which could cause some strange behavior otherwise.
			if (distFromCenter.magnitude < playerRad) {
				playerRad = distFromCenter.magnitude;
			}

			/// Calculate the depth in each dimension. Depth is signed to indicate
			/// which side of center.
			for (int i = 0; i < 3; i++) {
				depth [i] = Mathf.Sign (distFromCenter [i]) *
				(boxSize [i] + playerRad) - distFromCenter [i];
			}
		}

		return depth;
	}
}
