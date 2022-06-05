using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossBehavior : MonoBehaviour {

	public float speed_factor = 0.2f;
	public Transform start_position;
	public Transform end_position;

	public int speed = 10;
	public GameObject explosion;
	public GameObject flames;
	private GameObject instantiated_object;

	public Slider health_bar;
	private bool already_dead_;

	// Use this for initialization
	void Start () {
		GetComponent <Animation>()["Anim_Walk"].normalizedSpeed = speed_factor;

		GameObject temp = GameObject.FindGameObjectWithTag("WallHealth");
		health_bar = temp.GetComponent<Slider> ();

		already_dead_ = false;

		transform.position = start_position.position;

	}
	
	// Update is called once per frame
	void Update () {
		if (!already_dead_) {
			transform.position = Vector3.MoveTowards (start_position.position, end_position.position, speed * Time.time);
		} else {
			transform.position = start_position.position;
			already_dead_ = false;
		}
		checkIfDead ();
	}

	// This thing doesn't seem to be working
	void onLevelWasLoaded(int level) {
		if (level == 1) 
			this.transform.position = start_position.position; 
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Gate")) {
			instantiated_object = (GameObject) Instantiate(explosion, transform.position, transform.rotation);
			Destroy(instantiated_object, 2);
			Destroy(other.gameObject);

			health_bar.value = 0;

			Invoke ("gameOver", 5.0f);
		}
	}

	void gameOver() {
		//Application.LoadLevel ("Game Over");
		SceneManager.LoadScene("Game Over");
	}

	void gameWin() {
		//Application.LoadLevel ("WinGame");
		SceneManager.LoadScene ("WinGame");
	}

	void checkIfDead() {
		if (health_bar.value >= 100f && already_dead_ == false) {
			already_dead_ = true;
			speed = 0;
			speed_factor = 0;

			instantiated_object = (GameObject)Instantiate (explosion, transform.position, transform.rotation);
			Destroy (instantiated_object, 2);

			Vector3 flame_position = new Vector3(transform.position.x, 66.6f, transform.position.z);
			instantiated_object = (GameObject)Instantiate (flames, flame_position, transform.rotation);
			Destroy (instantiated_object, 7);

			Invoke ("gameWin", 8.0f);

			Destroy (this.gameObject, 8f);
		}
	}

}
