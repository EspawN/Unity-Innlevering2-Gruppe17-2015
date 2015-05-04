using UnityEngine;
using System.Collections;

public class controltest : MonoBehaviour {
	// all that has to do with changes
	public GameObject lilleMario;
	public GameObject storeMario;
	public GameObject fireMario;			
	private SmallMarioBehaviour lilleMarioScript;
	private BigMarioBehaviour storeMarioScript;
	private FireMarioBehaviour fireMarioScript;
	private GameObject currentPlayer;
	private float currentState;
	// all that has to do with clamping
	private Camera cam;
	private float mariosBackwardBoundsLimit;
	// all that has to do with secretWorld entry and exit
	private Rigidbody2D mariosRB;
	private Color blueColor;
	private Vector3 lerpPointToSecret;
	public Transform entrySpotForSecretWorld;
	public Transform exitSpotForSecretWorld;
	public Transform cameraPositionInSecretWorld;
	private bool inTheSecretWorld;
	private bool wentHome;
	private bool wentToSecretWorld;
	private bool lerpToSecret;
	private bool lerpHome;

	void Awake(){
		cam = Camera.main;
		blueColor = cam.backgroundColor;
		float height = cam.orthographicSize;
		mariosBackwardBoundsLimit = height * cam.aspect - 0.1f;
	}
	void Start(){
		lilleMarioScript = lilleMario.GetComponent<SmallMarioBehaviour> ();
		storeMarioScript = storeMario.GetComponent<BigMarioBehaviour> ();
		fireMarioScript = fireMario.GetComponent<FireMarioBehaviour> ();
		currentPlayer = GetCurrentActive ();
	}

	void Update(){	
		currentPlayer = GetCurrentActive ();
		mariosRB = currentPlayer.GetComponent<Rigidbody2D> ();
		ClampPosition ();
		CheckForChangesInState ();
		SetCorrectPlayer ();
		if (Input.GetKey (KeyCode.T)) {
			Application.LoadLevel(Application.loadedLevel);
		}
		SetTransformPositions ();
		SecretWorldHandling ();
		}
		public BigMarioBehaviour getBigMariosScript(){
			return storeMarioScript;
		}
		public  SmallMarioBehaviour getSmallMariosScript(){
			return lilleMarioScript;
		}
	public FireMarioBehaviour getFireMariosScript(){
		return fireMarioScript;
	}
		private void SetTransformPositions(){
			lilleMario.transform.position = GetCurrentActive().transform.position;
			storeMario.transform.position = GetCurrentActive().transform.position;
				fireMario.transform.position = GetCurrentActive().transform.position;
		}
	private GameObject GetCurrentActive(){
		if (lilleMario.activeInHierarchy)
			return lilleMario;
		else if (storeMario.activeInHierarchy)
			return storeMario;
			return fireMario;
	}
	private void CheckForChangesInState(){
		if (GetCurrentActive () == lilleMario)
			currentState = lilleMarioScript.getState ();
		else if (GetCurrentActive () == storeMario) {
			currentState = storeMarioScript.getState ();
			if (currentState == 1) {
				lilleMarioScript.setOpacityOnBigMarioKIll ();
				lilleMario.gameObject.layer = 10;
			}
		} else {
			currentState = fireMarioScript.getState ();
			if (currentState == 1) {
				lilleMarioScript.setOpacityOnBigMarioKIll ();
				lilleMario.gameObject.layer = 10;
			}
		}
	}
	private void SetCorrectPlayer(){
		if (currentState == 1) {
			SetDefaultStates();
			lilleMario.SetActive (true);
			storeMario.SetActive (false);
			fireMario.SetActive (false);
		} else if (currentState == 2) {
			SetDefaultStates();
			storeMario.SetActive (true);
			lilleMario.SetActive (false);
			fireMario.SetActive (false);
		} else {
			SetDefaultStates();
			fireMario.SetActive(true);
			storeMario.SetActive(false);
			lilleMario.SetActive(false);
		}
	}
	private void SetDefaultStates (){
		lilleMarioScript.setDefaultState ();
		storeMarioScript.setDefaultState ();
		fireMarioScript.setDefaultState ();
	}
	private void ClampPosition(){
		float camPos = Camera.main.transform.position.x;
		Rigidbody2D rb = currentPlayer.GetComponent<Rigidbody2D> ();
		rb.position = new Vector2 (Mathf.Clamp (rb.position.x, camPos - mariosBackwardBoundsLimit, camPos + 100),rb.transform.position.y);
	}

	// rest of the script has to do with the secret world
	private void SecretWorldHandling(){
		ConfigurationForSecretWorld ();
		if (lerpToSecret) {
			LerpDown ();
		}
		if (lerpHome) {
			LerpRight();
		}
	}
	
	private void ConfigurationForSecretWorld(){	
		if (wentHome) {
			cam.backgroundColor = blueColor;
			cam.transform.position = new Vector3(cam.transform.position.x,0.88f,-10);
		}
		else if (AreWeGoingToTheSecretWorld () && !wentToSecretWorld) {
			if(Input.GetKey(KeyCode.DownArrow)){
				lerpToSecret = true;
				Invoke("MovePlayerToSecretWorld",2);
				wentToSecretWorld = true;
			}
		} else if (AreWeGoingHome ()) {
			if(Input.GetKey(KeyCode.RightArrow)){
				lerpPointToSecret = new Vector3(currentPlayer.gameObject.transform.position.x + 2f,currentPlayer.gameObject.transform.position.y);
				lerpHome = true;
				Invoke("MovePlayerHome",2);
			}
		}
	}
	private bool AreWeGoingToTheSecretWorld(){
		if (lilleMarioScript.AreWeGoingToTheSecretWorld ()) {
			return true;
		}
		else if (storeMarioScript.AreWeGoingToTheSecretWorld()) {
			return true;
		}
		return false;
	}
	private bool AreWeGoingHome(){
		if (lilleMarioScript.AreWeGoingHome ()) {
			return true;
		} else if (storeMarioScript.AreWeGoingHome()) {
			return true;
		}
		return false;
	}
	private void MovePlayerToSecretWorld(){
		lerpPointToSecret = new Vector3(currentPlayer.gameObject.transform.position.x,currentPlayer.gameObject.transform.position.y - 0.2f);
		currentPlayer.gameObject.transform.position = entrySpotForSecretWorld.position;
		cam.backgroundColor = Color.black;
		cam.transform.position = cameraPositionInSecretWorld.position;
	}
	private void MovePlayerHome(){
		currentPlayer.gameObject.transform.position = exitSpotForSecretWorld.position;
		if(currentPlayer.gameObject.transform.position == exitSpotForSecretWorld.position)
			wentHome = true;
	}
	private void LerpDown(){
		lilleMarioScript.disableMovement ();
		storeMarioScript.disableMovement ();
		mariosRB.isKinematic = true;
		currentPlayer.gameObject.transform.position = 
			Vector3.Lerp (currentPlayer.gameObject.transform.position,
			              new Vector3 (currentPlayer.gameObject.transform.position.x, 
			             lerpPointToSecret.y), 0.5f * Time.deltaTime);
		if (currentPlayer.gameObject.transform.position.y < lerpPointToSecret.y) {
			lerpToSecret = false;
			mariosRB.isKinematic = false;
		}
	}
	private void LerpRight(){
		mariosRB.isKinematic = true;
		currentPlayer.gameObject.transform.position = 
			Vector3.Lerp (currentPlayer.gameObject.transform.position,
			              new Vector3 (lerpPointToSecret.x, 
			             currentPlayer.gameObject.transform.position.y), 0.5f * Time.deltaTime);
		if (currentPlayer.gameObject.transform.position.x > lerpPointToSecret.x) {
			lerpHome = false;
			mariosRB.isKinematic = false;
		}
	}
}
