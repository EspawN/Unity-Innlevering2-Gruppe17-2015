using UnityEngine;
using System.Collections;

public class SpawnMultipleGombas : MonoBehaviour {
	public GameObject gomba;
	public Transform []transforms;
	//this
	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			for (int i = 0; i < transforms.Length; i++) {
				Instantiate (gomba, transforms [i].position, Quaternion.identity);
			}
		}
		Destroy (this.gameObject);
	}
}
