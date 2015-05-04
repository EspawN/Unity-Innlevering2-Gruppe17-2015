using UnityEngine;
using System.Collections;

public class CoinBehaviour : MonoBehaviour {
	Rigidbody2D rb;
	public float force = 100;
	private float lifeTime = 0.8f;

	// Use this for initialization
	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		//Object.Destroy (rb, 0.3f);
		Destroy (gameObject, lifeTime);

	}
	void Start () {
		rb.AddForce (new Vector2 (0, force));
	}
}
