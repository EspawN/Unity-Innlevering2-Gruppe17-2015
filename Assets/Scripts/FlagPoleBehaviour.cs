using UnityEngine;
using System.Collections;

public class FlagPoleBehaviour : MonoBehaviour {
	private Animator anim;
	bool idle = true;
	void Awake(){
		anim = GetComponent<Animator> ();
	}

	void OnTriggerStay2D(){
			anim.SetBool ("marioAt", true);
		Invoke ("SetFalse", 0.1f);
		Debug.Log("triggered");
	}
	void SetFalse(){
		anim.SetBool("marioAt",false);
	}
}
