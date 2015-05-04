using UnityEngine;
using System.Collections;

// this Script spawns the first gomba when crossing triggerpoint
public class SpawnGomba : MonoBehaviour {
	public GameObject gomba;
	private static bool spawned;
		
	void OnTriggerEnter2D(){
			CreateGomba ();
		Destroy (this.gameObject);
	}
	private void CreateGomba(){
		Instantiate (gomba,gomba.transform.position,Quaternion.identity);
	}
}
