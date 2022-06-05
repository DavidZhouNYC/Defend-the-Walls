using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class stunGunCollision : MonoBehaviour {

	public Slider wall_health_bar;
	public float attack_damage;
	public GameObject explosion;
	private GameObject instantiated_object_;
	public GameObject weapon_power_level_;

	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.FindGameObjectWithTag("WallHealth");
		wall_health_bar = temp.GetComponent<Slider> ();
		attack_damage = 0.01f;
	}
	
	// Update is called once per frame
	void Update () {
		if (attack_damage >= 0.3f) {
			weapon_power_level_.SetActive (true);
		}

	}
	void OnParticleCollision(GameObject other){
		if (attack_damage > .3) {
			instantiated_object_ = (GameObject)Instantiate (explosion, other.transform.position, other.transform.rotation);
			Destroy (instantiated_object_, 0.3f);
		}

		if (other.tag == "Enemy") {
			other.GetComponentInParent<EnemyBehavior> ().health -= attack_damage + 0.01f;
		} else if (other.tag == "Boss") {
			wall_health_bar.value += attack_damage;
		}

	}
}
