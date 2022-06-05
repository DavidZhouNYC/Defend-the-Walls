using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

	public GameObject the_enemy;
	public int enemy_count;

	private GameObject instantiated_object_;

	// Use this for initialization
	void Start () {
		enemy_count = 0;
		// does something every Y seconds, starting after X seconds
		InvokeRepeating ("DropEnemy", 0.0f, 5.0f);
	}

	void DropEnemy() {
		if (enemy_count < 15) {
			// Create a random X position from which to drop the enemy.
			float x_position = Random.Range (190, 250);
			// same for Z
			float z_position = Random.Range (45, 212);

			// Create a new vector 3 with the coordinates
			Vector3 enemy_position = new Vector3(x_position, transform.position.y, z_position);
			instantiated_object_ = (GameObject) Instantiate (the_enemy, enemy_position, transform.rotation);
			instantiated_object_.transform.localScale = Vector3.one * (Random.Range (1f, 2f));

			enemy_count++;
		}
	}
}