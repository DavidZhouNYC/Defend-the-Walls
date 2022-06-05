using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {

	public DoorControl doorToControl;
	[HideInInspector]
	public bool doorState = false;
	public Animator switchAnim;
	public AudioSource switchSource;


}
