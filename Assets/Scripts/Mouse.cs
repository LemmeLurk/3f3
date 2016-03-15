using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour {

	private Vector3 position;

	private bool buttonPress;

	public Mouse () {
		// initialize mouse position
		position = Input.mousePosition;

		buttonPress = false;
	}

	public Vector3 getPosition() {
		return Input.mousePosition;
	}

	public Vector3 getWorldPoint() {
		return Camera.main.ScreenToWorldPoint (position);
	}

	// create mouse click function - returns bool
	public bool isPressed() {
		return buttonPress;
	}
}
