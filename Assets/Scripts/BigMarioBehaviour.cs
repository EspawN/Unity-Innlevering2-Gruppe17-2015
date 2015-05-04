using UnityEngine;
using System.Collections;

public class BigMarioBehaviour : MonoBehaviour {
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
	private int state = 2;
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
			if(goombaBehave !=null && goombaBehave.Isalive()){
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
	// mario has 3 states in this game small,big,fire -> 1,2,3
	void UpTheAnte(){
		Debug.Log("needs input for fireMario");
		state = 3;

	}
	public int getState(){
		return state;
	}
	public void setDefaultState(){
		this.state = 2;
	}
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.name == "SpecialTunnel" && Input.GetKey(KeyCode.DownArrow)) {
			transform.position = new Vector2(other.transform.position.x,transform.position.y);
			goingToTheSecretWorld = true;
		} else if(other.gameObject.name == "TakeMeHomeTunnel" && Input.GetKey(KeyCode.RightArrow)){
			goingHome = true;
		}else if (other.gameObject.name == "ConfirmedTransfer") {
			ableToMove = true;
		}else if (other.gameObject.tag == "PowerUp") {
			Debug.Log("du traff den");
			UpTheAnte();
		}
	}
	public bool AreWeGoingToTheSecretWorld(){
		return goingToTheSecretWorld;
	}
	public bool AreWeGoingHome(){
		goingToTheSecretWorld = false;
		return goingHome;
	}
	public void disableMovement(){
		Debug.Log("disabled");
		ableToMove = false;
	}
	public static bool isflipped(){
		return flipped;
	}
}
