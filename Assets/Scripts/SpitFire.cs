using UnityEngine;
using System.Collections;

public class SpitFire : MonoBehaviour {
	public Transform bullet;
	public float timeBetweenShots = 0.2f;
	private float shootingTime;
	private float bulletNr;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z) && Time.time > shootingTime) {
			bulletNr++;
			if(bulletNr < 2){
			shootingTime = Time.time+timeBetweenShots;
			} else{
				shootingTime = Time.time + timeBetweenShots*2;
				bulletNr = 0;
			}
			Instantiate(bullet,transform.position,transform.rotation);
		}
	}
}
