using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlameThrowerCollision : MonoBehaviour {
	public Slider health_bar;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.FindGameObjectWithTag("WallHealth");
		health_bar = temp.GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject other){
		if (other.tag == "Player") {
			PlayerHealthAndAttack healthScript = other.GetComponent<PlayerHealthAndAttack> ();
			healthScript.beingAttacked = true;
			healthScript.timeOfLastAttack = Time.time;
			print ("hit player");
		} else if (other.tag == "Gate") {
			health_bar.value -= 0.02f;
			if (health_bar.value == 0) {
				SceneManager.LoadScene ("Game Over");
			}
		}

	}
}
