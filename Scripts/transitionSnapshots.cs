using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class transitionSnapshots : MonoBehaviour {

	public AudioMixerSnapshot snapShotToUse;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			snapShotToUse.TransitionTo(0.1f);
		}
	}
}
