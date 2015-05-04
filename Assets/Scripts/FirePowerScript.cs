using UnityEngine;
using System.Collections;

public class FirePowerScript : MonoBehaviour {
	private Vector3 startPos;
	private Vector3 endPos;
	private float startTime;
	private float journeyLength;
	public float speed = 0.1f;
	private float initialMovement;
	private bool initializationDone;
	//private float thrust = 3f;
	//float oppositeVelocity;
	
	private CircleCollider2D box;
	void Awake(){
		box = GetComponent<CircleCollider2D> ();
		initialMovement = box.radius*2;
		startPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
		endPos = new Vector3 (transform.position.x, (transform.position.y + initialMovement), transform.position.z);
	}
	void Start(){
		startTime = Time.time;
		journeyLength = Vector3.Distance (startPos, endPos);
	}
	void Update(){
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		// added the initialdone because of bug that loops this infinite
		if (transform.position == endPos && !initializationDone) {
			initializationDone = true;
		} else if (!initializationDone) {
			transform.position = Vector3.Lerp (startPos, endPos, fracJourney);
		}
	}
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player" && initializationDone) {
			// remember to play animation
			Destroy(gameObject);
		}
	}
}
