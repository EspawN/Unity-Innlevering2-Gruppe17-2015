using UnityEngine;
using System.Collections;

public class CoinBoxHit : MonoBehaviour {
	public Sprite deadSprite;
	public GameObject powerPrefab;
	// default 1 power i box
	public int powersInBox = 1;
	private int powersInstantiated = 0;
	private SpriteRenderer spriteRenderer;
	private Animator anim;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnTriggerExit2D(Collider2D other){
		if (powersInstantiated < powersInBox) {
				Instantiate (powerPrefab, new Vector2 (transform.position.x, transform.position.y + 0.01f), transform.rotation);
			powersInstantiated++;
		} if(powersInstantiated == powersInBox) {
			if(anim != null)
				anim.enabled = false;
			spriteRenderer.sprite = deadSprite;
		}
	}
}
