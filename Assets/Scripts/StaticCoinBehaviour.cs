using UnityEngine;
using System.Collections;

public class StaticCoinBehaviour : MonoBehaviour {
	
	// Update is called once per frame
	void OnTriggerEnter2D(){
		Destroy (gameObject);
	}
}
