using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	public GameObject winGameScreen;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			Time.timeScale = 0;
			winGameScreen.SetActive(true);
		}

	}
}
