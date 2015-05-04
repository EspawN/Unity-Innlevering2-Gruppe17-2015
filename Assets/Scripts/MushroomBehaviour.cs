using UnityEngine;
using System.Collections;

public class MushroomBehaviour : MonoBehaviour {
	private Vector3 startPos;
	private Vector3 endPos;
	private float startTime;
	private float journeyLength;
	public float speed = 1f;
	private float initialMovement;
	private bool initializationDone;
	private float thrust = 2.3f;
	private bool forceAdded;
	float oppositeVelocity;

	private CircleCollider2D box;
	private Rigidbody2D rb;
	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		box = GetComponent<CircleCollider2D> ();
		initialMovement = (box.radius*2)+0.09f;
		startPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		endPos = new Vector3 (transform.position.x, (transform.position.y + initialMovement), transform.position.z);
	}
	void Start(){
		startTime = Time.time;
		journeyLength = Vector3.Distance (startPos, endPos);
	}
	void Update(){
		oppositeVelocity = -rb.velocity.x;
	}
	void FixedUpdate(){
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			if (!initializationDone) {
			transform.position = Vector3.Lerp (startPos, endPos, fracJourney);
			initializationDone = true;
		}
		if (initializationDone && !forceAdded) {
			Debug.Log("force added");
			rb.AddForce(new Vector2(thrust,-1));
			forceAdded = true;
		} 
	}
	void OnCollisionEnter2D(Collision2D other){
		Debug.Log (other.gameObject.name);
		if (other.gameObject.tag != "ground" || other.gameObject.tag == "enemy") {
			rb.velocity = new Vector2(oppositeVelocity,rb.velocity.y);
		}
		if (other.gameObject.tag == "Player" && forceAdded) {
			// remember to play animation
			Destroy(gameObject);
		}
	}
}
