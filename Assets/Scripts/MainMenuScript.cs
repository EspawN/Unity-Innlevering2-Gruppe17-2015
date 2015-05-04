using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Audio Variables
	public AudioClip MainMenuMusic;


	void Awake () {

	}
	
	public void OnePlayer () {
		Application.LoadLevel("Main_Scene");
	}
		
	public void HiScore () {
		Application.LoadLevel("High_Scores");
	}

	public void Back () {

		Application.LoadLevel ("Main_Menu");
	}

	public void Exit (){
		Application.Quit ();
	}
}
