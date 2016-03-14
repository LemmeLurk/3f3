using UnityEngine;
using System.Collections;

public class xa : MonoBehaviour 
{
	// this script creates a bunch of static public variables that can be seen by all the other scripts in the game
	public static Player1 player1;

	public static bool gameOver = false;


	void Start()
	{
		// cache these so they can be accessed by other scripts
		player1 = GameObject.Find("Player1").GetComponent<Player1>();
	}
}
