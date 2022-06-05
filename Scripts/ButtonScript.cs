using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {
	void Start() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void restartLevel() {
		//Application.LoadLevel ("Defend the Wall");
		SceneManager.LoadScene ("Defend the Wall");
	}
}
