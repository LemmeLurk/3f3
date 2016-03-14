using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{

	public enum inputState 
	{ 
		None, 
		WalkLeft, 
		WalkRight, 
		Jump, 
		Sticking,
		Eating,
		Pooping
	}

	[HideInInspector] public inputState currentInputState;
	
	[HideInInspector] public enum facing { Right, Left }
	[HideInInspector] public facing facingDir = facing.Right;

	[HideInInspector] public bool alive = true;
	[HideInInspector] public bool sticky = false;
	[HideInInspector] public Vector3 spawnPosition;
	[HideInInspector] public Vector3 mousePosition;

	protected Transform _transform;
	protected Rigidbody2D _rigidbody;

	// edit these to tune character movement	
	private float movementVel = 2.5f; 	// run speed when not carrying the ball
	private float extraWeightVel = 2f; 	// walk speed while carrying ball

	// for the time being, act as though jump's constant arc is calculated using these 3 values
	private float jumpVel = 4f; 	// jump velocity
	private float jump2Vel = 2f; 	// double jump velocity
	private float jump3Vel = 2f; 	// double jump velocity

	private float fallVel = 1f;		// fall velocity, gravity
	private float stickVel = 3f;		// horizontal velocity of ball when passed

	private float moveVel;
	private float pVel = 0f;
	
	private int jumps = 0;
    private int maxJumps = 2; 		// set to 2 for double jump
		
	protected bool needsToPoop = false;

	// raycast stuff
	private RaycastHit2D hit;
	private Vector2 physVel = new Vector2();
	[HideInInspector] public bool grounded = false;
	private int groundMask = 1 << 8; // Ground layer mask

	public virtual void Awake()
	{
		_transform = transform;
		_rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Use this for initialization
	public virtual void Start () 
	{
		moveVel = 1;
		mousePosition = Input.mousePosition;
	}
	
	// Update is called once per frame
	public virtual void UpdateMovement() 
	{

		//special case
		// TODO: Give actual xa for the game's state
		if(false == true || alive == false) return;

		/*
		 * Let the players x position be moved with force
		 * Greater distance from mouse == greater force added to x
		 * x++ if mouse.x > player.x, x-- if mouse.x < player.x
		 */
		//float moveSpeed = 0.1f;
		float moveSpeed = 0.01f;
		mousePosition = Input.mousePosition;

		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        _transform.position = Vector2.Lerp(_transform.position, mousePosition, moveSpeed);
		mousePosition.z = _transform.position.z;
		_transform.position = Vector3.MoveTowards ( _transform.position, mousePosition, moveSpeed * Time.deltaTime);

		// teleport me to the other side of the screen when I reach the edge
		if(_transform.position.x > 4f)
		{
			_transform.position = new Vector3(-4f,_transform.position.y, 0);
		}
		if(_transform.position.x < -4f)
		{
			_transform.position = new Vector3(4f,_transform.position.y, 0);
		}

		// logic for jumping  
		if (Input.GetMouseButtonDown(0))  // 0 is the left-click
		{}
	}
	
	// ============================== FIXEDUPDATE ============================== 
	//readonly 
	public virtual void UpdatePhysics()
	{
		//if(xa.gameOver == true || alive == false) return;

		physVel = Vector2.zero;


		// Will be holding down the jump for at least 1sec -- 1x1
		if (Input.GetMouseButton(1)) {
		}

		// jump, 2x1
		// move left
		if(currentInputState == inputState.WalkLeft)
		{
			physVel.x = -moveVel;
		}
		// move right
		if(currentInputState == inputState.WalkRight)
		{
			physVel.x = moveVel;
		}

		// jump
		if(currentInputState == inputState.Jump)
		{
			// get the currentGameTime and assign it to a variable
			// call the jumpHandler(int) 
			if(jumps < maxJumps)
			{
				jumps += 1;
				if(jumps == 1)
				{
					_rigidbody.velocity = new Vector2(physVel.x, jumpVel);
				}
				else if(jumps == 2)
				{
					_rigidbody.velocity = new Vector2(physVel.x, jump2Vel);
				}
			}
		}

		// Jump handler Logic
		//  void jumpHandler( int )
		// first put the argument into a vaiable 
		// and call that vairable firstRan, store it again as currentTime
		// test if currentTime == firstRan + 1; if less than - call Jump.up()
		// if greater than 1 but less that 3 call jump.fjump1, and if equal to, 
		// and if greater than or equal to 3, call jump2( )


		// use raycasts to determine if the player is standing on the ground or not
		if (Physics2D.Raycast(new Vector2(_transform.position.x-0.1f,_transform.position.y), -Vector2.up, .26f, groundMask) 
		    || Physics2D.Raycast(new Vector2(_transform.position.x+0.1f,_transform.position.y), -Vector2.up, .26f, groundMask))
		{
			grounded = true;
			jumps = 0;
		}
		else 
		{
			grounded = false;
			_rigidbody.AddForce(-Vector3.up * fallVel);
		}

		// actually move the player
		_rigidbody.velocity = new Vector2(physVel.x, _rigidbody.velocity.y);
	}
}
