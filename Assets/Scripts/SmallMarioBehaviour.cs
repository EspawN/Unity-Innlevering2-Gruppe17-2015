using UnityEngine;
using System.Collections;

public class SmallMarioBehaviour : MonoBehaviour {
	float stdSpeed = 0.9f;
	float walkSpeed = 0.9f;
	public KeyCode runButton;
	float runningSpeed = 1.8f;
	bool walking;
	bool idle;
	public float jumpingPower = 3.7f;
	Animator anim;
	Rigidbody2D rb;
	bool facingRight = true;
	bool flipped;
	float moveDir;
	private int state = 1;	
	public Sprite deadSprite;
	private SpriteRenderer spriteRenderer;	
	private float enemyBounceOfForce = 2.2f;
	private bool LerpingToSecretLevel;
	private bool LerpingHome;
	private bool goingHome;
	// groundCheck
	bool notJumping;
	public Transform groundCheck;
	public LayerMask ground;
	// groundCheck end
	// Sound
	public AudioClip jumpSound;
	private AudioSource audSource;
	//Sound end
	bool overLapping = false;
	Material defaultMaterial;
	bool opacityChangeDone = false;

	bool goingToTheSecretWorld = false;

	bool ableToMove = true;

	public Material transparentMaterial;
	void Awake () {
		DontDestroyOnLoad (this);
		DontDestroyOnLoad (this.gameObject);
		audSource = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		defaultMaterial = spriteRenderer.material;
	}
	void Update(){
		moveDir = Input.GetAxis ("Horizontal");
		if (Input.GetButtonDown ("Jump") && notJumping) {
			audSource.Play ();
			rb.velocity = transform.up * jumpingPower;
			notJumping = false;
		}
	}
	void FixedUpdate(){
		notJumping = Physics2D.OverlapCircle (groundCheck.position, 0.1f, ground);
		moveDir = Input.GetAxis ("Horizontal");
		if(ableToMove)
		rb.velocity = new Vector2 (moveDir * stdSpeed,rb.velocity.y);
		if (Input.GetKey (runButton) && notJumping && Mathf.Abs(moveDir) > 0) {
			stdSpeed = runningSpeed;
			// addded this test to maintain velocity while jumping
		} else if(notJumping) {
			stdSpeed = walkSpeed;
		}// added this test so it keeps current velocity
		DoAnimation ();
	}
	void DoAnimation(){
		anim.SetBool("isJumping",!notJumping);
		if (notJumping) {
			CheckForMoveDirection (moveDir);
			anim.SetFloat ("isWalking", Mathf.Abs (moveDir * stdSpeed));
		} 
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
	// collisions with gombas and powerups are central
	void OnCollisionEnter2D(Collision2D other){
		GoombaBehaviour goombaBehave;
		GameObject source = other.gameObject;
		if (source.name == "Goomba" || source.name == "Goomba(Clone)") {
				goombaBehave = source.GetComponent<GoombaBehaviour>();
			if(goombaBehave.Isalive()){
				KillPlayer();
			} else{
				// bounce of enemy
				rb.velocity = transform.up*enemyBounceOfForce;
			}
		} else if (other.gameObject.tag == "PowerUp") {
			UpTheAnte();
		}
	}
	private void KillPlayer(){
		this.anim.enabled = false;
		this.spriteRenderer.sprite = deadSprite;
		rb.velocity = transform.up*jumpingPower;
	}
	// mario has 3 states in this game small,big,fire -> 1,2,3
	void UpTheAnte(){
		overLapping = true;
		audSource.clip = jumpSound;
		audSource.Play ();
		Invoke ("SetState", 0.6f);

	}
	public int getState(){
		return state;
	}
	public void SetState(){
		rb.isKinematic = false;
		overLapping = false;
		state = 2;
	}
	public void setDefaultState(){
		this.state = 1;
	}
	public void setOpacityOnBigMarioKIll(){
		spriteRenderer.material = transparentMaterial;
		Invoke ("setOpacityToNormal", 2);
	}
	public void setOpacityToNormal(){
		spriteRenderer.material = defaultMaterial;
		opacityChangeDone = true;
		gameObject.layer = 8;
	}
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.name == "SpecialTunnel" && Input.GetKey (KeyCode.DownArrow)) {
			transform.position = new Vector2 (other.transform.position.x, transform.position.y);
			goingToTheSecretWorld = true;
		} else if (other.gameObject.name == "TakeMeHomeTunnel" && Input.GetKey (KeyCode.RightArrow)) {
			goingHome = true;
		} else if (other.gameObject.name == "ConfirmedTransfer") {
			ableToMove = true;
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
}
