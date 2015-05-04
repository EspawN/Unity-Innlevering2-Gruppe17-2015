using UnityEngine;
using System.Collections;

public class QuestionBoxHit : MonoBehaviour {
	public Sprite deadSprite;
	public GameObject mushromPrefab;
	public GameObject firePrefab;
	//public GameObject secondPowerPrefab;
	// default 1 power i box
	public int powersInBox = 1;
	private int powersInstantiated = 0;
	private SpriteRenderer spriteRenderer;
	private Animator anim;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		if(GetComponent<Animator>() != null)
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnTriggerExit2D(Collider2D other){
		GameObject prefab;
		if (other.name.Equals ("MarioSmall"))
			prefab = mushromPrefab;
		else
			prefab = firePrefab;
			if (powersInstantiated < powersInBox) {
				Instantiate (prefab, new Vector2 (transform.position.x, transform.position.y + 0.01f), transform.rotation);
			powersInstantiated++;
		} if(powersInstantiated == powersInBox) {
			if(anim != null)
			anim.enabled = false;
			spriteRenderer.sprite = deadSprite;
		}
	}
}
