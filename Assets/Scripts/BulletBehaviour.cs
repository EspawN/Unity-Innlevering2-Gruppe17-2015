using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour { 
	private float force = 4f;
	private Rigidbody2D rb;
	private bool shot;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		Destroy (gameObject, 1);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//transform.Translate (Vector2.right * force);

		if (FireMarioBehaviour.isflipped())
			force *= -1;
		if (!shot) {
			//rb.AddForce (Vector2.right * force);
			rb.velocity = new Vector2 (1*force, -0.3f*4);
		}
		shot = true;
	}
	void OnCollisionEnter2D(Collision2D other){
		Debug.Log("collision");
		if (other.gameObject.tag == "Enemy") {
			Destroy (gameObject);
			Destroy (other.gameObject);
		}
	}
}
