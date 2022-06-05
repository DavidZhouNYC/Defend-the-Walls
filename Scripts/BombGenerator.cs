using UnityEngine;
using System.Collections;

public class BombGenerator : MonoBehaviour {

	public GameObject the_bomb;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("DropBomb", 1f, 7f);
	}

	void DropBomb() {
		Instantiate(the_bomb, new Vector3(Random.Range(0f, 250f), transform.position.y, Random.Range(0f, 250f)), transform.rotation);
	}
}