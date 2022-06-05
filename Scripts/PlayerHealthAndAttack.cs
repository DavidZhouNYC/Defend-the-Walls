using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthAndAttack : MonoBehaviour {
	private uint money_;
	public Text money_text;

	public bool beingAttacked;
	public float timeOfLastAttack;
	public Image deathScreen;
	public Color startColor;
	public Color endColor;
	private bool dead;
	private float fractionToEndColor;
	private float speed =0.8f;
	//public GameObject endGameText;
	public ParticleSystem stunSystem;
	public Slider energySlider;
	private float fractionOfEnergy = 1;
	public float energyDepletionSpeed;
	public GameObject sliderBar;
	private float timeToRegainHealth = 0.5f;

	public GameObject playerCamera;
	public GameObject tutorialMessage;
	public GameObject rechargeMessage;
	public GameObject purchaseWeaponUpgradeMessage;
	public GameObject entireSlider;
	public GameObject purchaseRepairText;
	public Slider wallHealthSlider;

	//public GameObject doorSwitchMessage;

	public AudioSource gunSource;
	private float startTimeForFire;
	public AudioSource hurtSource;
	public AudioClip[] hurtClips = new AudioClip[5];

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		money_ = 5000;
		money_text.text = "Gold: " + money_.ToString ();

		Invoke ("tutorialNumber2", 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
		manageHealth ();
		checkForAttack ();
		checkForRechargeAndDoorSwitch ();

		money_text.text = "Gold: " + money_.ToString ();
	}

	void manageHealth(){
		if (!dead) {
			if(timeOfLastAttack + timeToRegainHealth < Time.time){
				beingAttacked = false;

			}

			if (beingAttacked) {
				//take damage
				takeDamage ();
			} else if (deathScreen.color != startColor) {

				//heal
				heal ();
			}

		} else {
			Time.timeScale = 0;
			//deathScreen.color = Color.black;
			//endGameText.SetActive(true);
			rechargeMessage.SetActive(false);
			entireSlider.SetActive(false);
			purchaseWeaponUpgradeMessage.SetActive (false);
			purchaseRepairText.SetActive (false);

			SceneManager.LoadScene ("Game Over");
		}


	}

	void takeDamage(){
		fractionToEndColor = Mathf.Min (fractionToEndColor + speed * Time.deltaTime/3, 1);
		deathScreen.color = Color.Lerp (startColor, endColor, fractionToEndColor);

		if (fractionToEndColor == 1) {
			dead = true;
		} else if (!hurtSource.isPlaying) {
			hurtSource.clip = hurtClips[Random.Range(0,hurtClips.Length)];
			hurtSource.Play();

		}

	}

	void heal(){
		fractionToEndColor = Mathf.Max (fractionToEndColor - speed * Time.deltaTime, 0);
		deathScreen.color = Color.Lerp (startColor, endColor, fractionToEndColor);

	}

	void checkForAttack(){
		if (Input.GetMouseButton (0)) {
			stunSystem.emissionRate = 10;
			fractionOfEnergy = Mathf.Max(fractionOfEnergy - energyDepletionSpeed * Time.deltaTime,0);
			energySlider.value = fractionOfEnergy;
			if(fractionOfEnergy == 0){
				stunSystem.emissionRate = 0;
				sliderBar.SetActive(false);
			}
			else {
				if(!gunSource.isPlaying){
					gunSource.Play();
					startTimeForFire = Time.time;
				}
				else if(Time.time - startTimeForFire >= 0.2f){
					gunSource.Play();
					startTimeForFire = Time.time;
				}

			}


		} else {
			stunSystem.emissionRate = 0;
		}

	}

	void checkForRechargeAndDoorSwitch(){
		if(rechargeMessage.activeSelf){
			rechargeMessage.SetActive(false);
			purchaseWeaponUpgradeMessage.SetActive (false);
			purchaseRepairText.SetActive (false);
		}
		/*
		if(doorSwitchMessage.activeSelf){
			doorSwitchMessage.SetActive(false);
		}*/

		RaycastHit hit;
		if(Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 3.0f)){
			if (hit.collider.tag == "StunRechargeStation") {
				AudioSource rechargeSource = hit.collider.GetComponent<AudioSource> ();

				if (!rechargeMessage.activeSelf) {
					rechargeMessage.SetActive (true);
					purchaseWeaponUpgradeMessage.SetActive (true);
					purchaseRepairText.SetActive (true);
				}
				if (Input.GetKey (KeyCode.F)) {
					//recharge stun gun
					rechargeStungun ();

					if (energySlider.value < 1 && rechargeSource.isPlaying == false) {
						rechargeSource.Play ();
					} else if (energySlider.value == 1 && rechargeSource.isPlaying == true) {
						rechargeSource.Stop ();
					}
				} else if (rechargeSource.isPlaying) {
					rechargeSource.Stop ();
				}

				if (Input.GetKeyDown (KeyCode.G)) {
					if (money_ >= 5000) {
						stunSystem.startSize += 2f;
						money_ -= 5000;
						stunSystem.GetComponent<stunGunCollision> ().attack_damage += 0.1f;
						energySlider.value = 1f;
					}
				}

				if (Input.GetKeyDown (KeyCode.R)) {
					if (money_ >= 2000 && wallHealthSlider.value < 50f) {
						wallHealthSlider.value = 50f;
						money_ -= 2000;
					}
				}

			}
				
			/*
			if(hit.collider.tag == "Switch"){
				DoorSwitch switchFound = hit.collider.GetComponent<DoorSwitch>();
				if(!switchFound.doorState){
					if(!doorSwitchMessage.activeSelf){
						doorSwitchMessage.SetActive(true);
					}
					if(Input.GetKey(KeyCode.F)){
						switchFound.doorState = true;
						switchFound.doorToControl.changeLockedStatus(false);
						switchFound.switchAnim.SetBool("on", true);
						switchFound.switchSource.Play();

					}
				}
			}*/
		}
	}

	void rechargeStungun(){
		if (!sliderBar.activeSelf) {
			sliderBar.SetActive(true);
		}

		fractionOfEnergy = Mathf.Min (fractionOfEnergy + energyDepletionSpeed * Time.deltaTime * 4, 1);
		energySlider.value = fractionOfEnergy;


	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Loot")) {
			money_ += 1000;
			Destroy(other.gameObject);
		}
	}

	void tutorialNumber2() {
		tutorialMessage.GetComponent<Text> ().text = "Defend the wall at all cost!";
		tutorialMessage.GetComponent<Text> ().color = Color.red;
		Invoke ("tutorialNumber3", 5.0f);
	}

	void tutorialNumber3() {
		tutorialMessage.GetComponent<Text> ().text = "!! DEFEAT THE TITAN !!";
		Invoke ("tutorialDisappear", 5.0f);
	}

	void tutorialDisappear() {
		tutorialMessage.SetActive (false);
	}
}
