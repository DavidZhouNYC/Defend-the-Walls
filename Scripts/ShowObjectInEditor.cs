using UnityEngine;
using System.Collections;

public class ShowObjectInEditor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, 0.5F);
		Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
	}
}
