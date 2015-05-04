using UnityEngine;
using System.Collections;

public class FireMarioBehaviour : MonoBehaviour {
	float stdSpeed = 0.9f;
	float walkSpeed = 0.9f;
	float runningSpeed = 1.8f;
	bool goingToTheSecretWorld;
	bool goingHome;
	bool ableToMove = true;
	
	public KeyCode runButton;
	Animator anim;
	Rigidbody2D rb;
	bool facingRight = true;
	public static bool flipped;
	float moveDir;
	// state 2 = large
	private int state = 3;
	private GoombaBehaviour goombaBehave;
	
	// groundCheck and Jumping
	bool notJumping;
	public float jumpingPower = 3.7f;
	public Transform groundCheck;
	public LayerMask ground;
	private float enemyBounceOfForce = 2.2f;
	// groundCheck end
	// Sound
	public AudioClip jumpSound;
	private AudioSource audSource;
	//Sound end
	
	void Awake () {
		// flipped is static so to make sure that it gets correct this test is implemeneted
		if (flipped) {
			FlipPlayer();
		}
		audSource = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		setDefaultState ();
	}
	void Update(){
		moveDir = Input.GetAxis ("Horizontal");
		notJumping = Physics2D.OverlapCircle (groundCheck.position, 0.1f, ground);
		if (Input.GetKey (runButton) && notJumping && Mathf.Abs(moveDir) > 0) {
			stdSpeed = runningSpeed;
		} else if(notJumping) {
			stdSpeed = walkSpeed;
		}
		if (Input.GetButtonDown ("Jump") && notJumping) {
			audSource.Play();
			rb.velocity = transform.up*jumpingPower;
			notJumping = false;
		}
		DoAnimation ();
	}
	void DoAnimation(){
		anim.SetBool("isJumping",!notJumping);
		if (notJumping) {
			CheckForMoveDirection (moveDir);
			// hvis farten er større enn abs 0.9 da løper vi
			anim.SetFloat ("isWalking", Mathf.Abs (moveDir * stdSpeed));
		} 
	}
	void FixedUpdate () {
		Debug.Log ("state: " + state);
		if(ableToMove)
			rb.velocity = new Vector2 (moveDir * stdSpeed,rb.velocity.y);
	}
	void CheckForMoveDirection(float move){
		if (move < 0) {
			if(facingRight && !flipped)
				FlipPlayer();
			flipped = true;
		}
		if (move > 0 && flipped) {
			FlipPlayer();
			flipped = false;
		}
	}
	
	void FlipPlayer(){
		transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
	}
	void OnCollisionEnter2D(Collision2D other){
		GameObject source = other.gameObject;
		if (source.tag == "Enemy") {
			goombaBehave = source.GetComponent<GoombaBehaviour>();
			if(goombaBehave.Isalive()){
				KillPlayer();
			} else{
				// bounce of enemy
				rb.velocity = transform.up*enemyBounceOfForce;
			}
		} 
	}
	void KillPlayer(){
		state = 1;
		Debug.Log ("state" + state);
	}
	public int getState(){
		Debug.Log ("getting state");
		return state;
	}
	public void setDefaultState(){
		this.state = 3;
	}

	public void disableMovement(){
		Debug.Log("disabled");
		ableToMove = false;
	}
	public static bool isflipped(){
		return flipped;
	}
}