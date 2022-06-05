using UnityEngine;
using System.Collections;

public class DoorControl : MonoBehaviour {

	public Animator doorAnimator;
	public bool locked;
	public Renderer doorRender;

	public Texture2D lockedColor;
	public Texture2D lockedEmmision;
	public Texture2D unlockedColor;
	public Texture2D unlockedEmmision;
	public AudioSource doorSource;

	// Use this for initialization
	void Start () {
		changeLockedStatus (locked);
	}


	void OnTriggerStay(Collider other){
		if (!locked) {
			if(other.tag == "Player" || other.tag == "Enemy"){
				if(doorAnimator.GetBool("open") == false){
					doorSource.Play();
				}
				doorAnimator.SetBool("open", true);
		
			}

		}

	}

	void OnTriggerExit(Collider other){
		if (!locked) {
			if(other.tag == "Player" || other.tag == "Enemy"){
				if(doorAnimator.GetBool("open") == true){
					doorSource.Play();
				}
				doorAnimator.SetBool("open", false);

			}
			
		}

	}

	public void changeLockedStatus(bool lockedStatus){
		locked = lockedStatus;
		if (locked) {
			doorRender.material.SetTexture ("_EmissionMap", lockedEmmision);

			doorRender.material.SetTexture ("_MainTex", lockedColor);
			print ("should be setting to the locked images");

		} else {
			{
				doorRender.material.SetTexture("_EmissionMap",unlockedEmmision);
				
				doorRender.material.SetTexture("_MainTex",unlockedColor);
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
