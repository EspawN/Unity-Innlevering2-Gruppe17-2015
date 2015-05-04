using UnityEngine;
using System.Collections;

public class GoombaBehaviour : MonoBehaviour {
	public Sprite deadSprite;
	private SpriteRenderer spriteRender;
	private float speed = -0.009f;
	private bool alive = true;
	private BoxCollider2D box;
	private Animator anim;
	bool stillAlive;

	// Use this for initialization
	void Awake () {
		spriteRender = GetComponent <SpriteRenderer>();
		box = GetComponent<BoxCollider2D> ();
		anim = GetComponent <Animator>();
		if (stillAlive)
			DestroyImmediate (this);
		stillAlive = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (alive) {
			transform.position = new Vector2 (transform.position.x + speed, transform.position.y);
		}
	}
	void OnCollisionEnter2D(Collision2D other){
		 if (other.gameObject.tag != "ground") {
			speed *= -1;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			alive = false;
			Invoke("KillEnemy",0.1f);
		}
	}
	void KillEnemy(){
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.mass = 1000;
		box.size = new Vector2 (box.size.x, box.size.y / 2);
		anim.enabled = false;
		spriteRender.sprite = deadSprite;
		Invoke("DestroyEnemy",0.3f);
	}
	public bool Isalive(){
		return alive;
	}
	void DestroyEnemy(){
		Destroy(gameObject);
		Destroy(this);
	}
}
