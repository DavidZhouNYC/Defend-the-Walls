using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public GameObject explosion;
	public GameObject loot;
	private GameObject instantiated_object_;

	private NavMeshAgent navAgent;
	private GameObject[] waypoints;

	private GameObject currentTarget;
	private int indexOfClosest;

	public enum State{patrol,pursue,disabled};

	public State state;
	//private GameObject[] targets;
	private GameObject target;
	public float escapeDistance;

	private bool turnRight;
	public GameObject robotHead;
	public GameObject[] positionToLookFrom;
	public Animator enemyAnim;
	public float attackRange;
	public GameObject flameLight;
	public ParticleSystem[] particlesForFlame;

	public float maxHealth;
	[HideInInspector]
	public float health;

	public GameObject[] lights;
	public Renderer renderForEnemy;
	public Texture2D offEmmision;
	public Texture2D onEmmision;

	public AudioSource enemyMovementSource;
	public AudioSource enemyAttackSource;

	private GameObject enemy_generator_;

	public float speed;
	// Use this for initialization
	void Start () {
		navAgent = GetComponent<NavMeshAgent> ();
		enemy_generator_ = GameObject.FindGameObjectWithTag("EnemyGenerator");

		waypoints = GameObject.FindGameObjectsWithTag ("Waypoint");
		currentTarget = waypoints [findIndexOfClosest ()];
		//targets = GameObject.FindGameObjectsWithTag ("Player");
		//target = targets [findIndexOfClosestTarget ()];
		target = GameObject.FindGameObjectWithTag("Player");
		state = State.patrol;
		turnRight = randomBool ();
		toggleFlame(false);
		health = maxHealth;
		enemyMovementSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.patrol) {
			patrol ();
		} else if (state == State.pursue) {
			pursue ();
		} else if (state == State.disabled) {
			tiltWhenNeeded();
			enemyMovementSource.Stop();
		}

		//target = targets [findIndexOfClosestTarget ()];
	}

	int findIndexOfClosest(){
		float closestDistance = int.MaxValue;

		for (int i = 0; i < waypoints.Length; i++) {
			if(Vector3.Distance(this.transform.position,waypoints[i].transform.position) < closestDistance){
				indexOfClosest = i;
				closestDistance = Vector3.Distance(this.transform.position,waypoints[i].transform.position);
			}
		}
		return indexOfClosest;

	}

	/*
	int findIndexOfClosestTarget() {
		float closestDistance = int.MaxValue;
		for (int i = 0; i < targets.Length; i++) {
			if (Vector3.Distance (this.transform.position, targets [i].transform.position) < closestDistance) {
				indexOfClosest = i;
				closestDistance = Vector3.Distance (this.transform.position, targets [i].transform.position);
			}
		}

		return indexOfClosest;
	}*/

	void patrol(){
		adjustAnimationSpeed ();
		tiltWhenNeeded ();
		turnHead ();
		checkToSeeIfMoving ();
		if (scanForTarget ()) {
			state = State.pursue;

		}
		// Reached the wall
		if (Vector3.Distance (this.transform.position, currentTarget.transform.position) <= navAgent.stoppingDistance) {
			toggleFlame (true);

			/*
			if(waypoints.Length > indexOfClosest +1){
				indexOfClosest++;
				currentTarget = waypoints[indexOfClosest];
			}
			else {
				indexOfClosest = 0;
				currentTarget = waypoints[indexOfClosest];

			}*/

		}
		navAgent.SetDestination (currentTarget.transform.position);
		checkIfDead ();
	}

	void pursue(){
		adjustAnimationSpeed ();
		tiltWhenNeeded ();
		lookAtTarget ();
		navAgent.SetDestination (target.transform.position);
		checkToSeeIfMoving ();

		Vector3 heightAdjustedTarget = new Vector3 (target.transform.position.x, robotHead.transform.position.y, target.transform.position.z);
		Vector3 directionToTarget = heightAdjustedTarget - robotHead.transform.position;

		if (Vector3.Distance (this.transform.position, target.transform.position) > escapeDistance) {
			state = State.patrol;
			currentTarget = waypoints [findIndexOfClosest ()];
		} else if (Vector3.Distance (this.transform.position, target.transform.position) <= attackRange && Mathf.Abs (Vector3.Dot (directionToTarget, robotHead.transform.right)) < 0.3 && flameLight.activeSelf == false) {
			//toggle flame on
			toggleFlame(true);
		} else if (Vector3.Distance (this.transform.position, target.transform.position) > attackRange) {
			//toggle flame off
			toggleFlame(false);
		}
		checkIfDead ();
	}

	bool scanForTarget(){
		for (int i = 0; i < positionToLookFrom.Length; i++) {
			Ray[] raysForSearch = new Ray[3];

			Vector3 noAngle = positionToLookFrom[i].transform.forward;
			Quaternion spreadAngle = Quaternion.AngleAxis (-20, new Vector3 (0, 1, 0));
			Vector3 negativeDirection = spreadAngle * noAngle;
			spreadAngle = Quaternion.AngleAxis (20, new Vector3 (0, 1, 0));
			Vector3 positiveDirection = spreadAngle * noAngle;

			Debug.DrawLine (positionToLookFrom[i].transform.position, positionToLookFrom[i].transform.position + noAngle * 20);
			Debug.DrawLine (positionToLookFrom[i].transform.position, positionToLookFrom[i].transform.position + positiveDirection * 20);
			Debug.DrawLine (positionToLookFrom[i].transform.position, positionToLookFrom[i].transform.position + negativeDirection * 20);


			raysForSearch [0] = new Ray (positionToLookFrom[i].transform.position, noAngle);
			raysForSearch [1] = new Ray (positionToLookFrom[i].transform.position, negativeDirection);
			raysForSearch [2] = new Ray (positionToLookFrom[i].transform.position, positiveDirection);

			foreach (Ray r in raysForSearch) {
				RaycastHit hit;
				if (Physics.Raycast (r, out hit, 20)) {
					if (hit.transform.tag == "Player") {
						return true;

					}
				}

			}

		}
		return false;

	}

	bool randomBool(){
		if (Random.value > 0.5) {
			return true;
		}
		return false;
	}

	void turnHead(){
		if (turnRight) {
			if(robotHead.transform.localEulerAngles.z < 90){
				turnRight = false;
			}

			robotHead.transform.Rotate(-Vector3.forward*Time.deltaTime*speed);

		} else {
			if(robotHead.transform.localEulerAngles.z > 270){
				turnRight = true;
			}
			robotHead.transform.Rotate(Vector3.forward*Time.deltaTime*speed);

		}

	}

	void lookAtTarget(){

		Vector3 heightAdjustedTarget = new Vector3 (target.transform.position.x, robotHead.transform.position.y, target.transform.position.z);
		Vector3 directionToTarget = heightAdjustedTarget - robotHead.transform.position;

		if (Vector3.Dot (directionToTarget, robotHead.transform.right) > 0.1) {
			robotHead.transform.Rotate(-Vector3.forward);
		}
		else if (Vector3.Dot (directionToTarget, robotHead.transform.right) < -0.1) {
			robotHead.transform.Rotate(Vector3.forward);
		}
	}

	void tiltWhenNeeded(){
		bool overRamp = false;
		RaycastHit hit;
		if (Physics.Raycast (transform.position + transform.forward/4, Vector3.down, out hit)) {
			Vector3 fHit = hit.point;
			if(hit.collider.tag == "Ramp"){
				overRamp = true;
			}
			if (Physics.Raycast (transform.position - transform.forward/4, Vector3.down, out hit)) {
				if(hit.collider.tag == "Ramp"){
					overRamp = true;
				}

				Vector3 bHit = hit.point;
				Vector3 tempForward = fHit - bHit;
				if(Mathf.Abs(Vector3.Angle(transform.forward, tempForward)) < 20 && overRamp){
					//print (Mathf.Abs(Vector3.Angle(transform.forward, tempForward)));
					transform.forward = fHit - bHit;
				}
			}
		}

	}

	void adjustAnimationSpeed(){
		//print (navAgent.velocity.magnitude);
		enemyAnim.speed = navAgent.velocity.magnitude / 3;
	}

	void toggleFlame(bool onState){
		if (onState) {
			for(int i = 0; i < particlesForFlame.Length; i++){
				particlesForFlame[i].Play();
			}
			flameLight.SetActive(true);
			enemyAttackSource.Play();


		} else {
			for(int i = 0; i < particlesForFlame.Length; i++){
				particlesForFlame[i].Stop();
			}
			flameLight.SetActive(false);
			enemyAttackSource.Stop();

		}

	}

	void checkIfDead(){
		if (health <= 0f && state != State.disabled) {
			state = State.disabled;
			instantiated_object_ = (GameObject) Instantiate (explosion, transform.position, transform.rotation);
			Destroy (instantiated_object_, 1f);

			StartCoroutine("disable");
		}
	}

	IEnumerator disable(){
		navAgent.Stop ();
		enemyAnim.speed = 0;
		toggleFlame (false);
		for (int i = 0; i < lights.Length; i++) {
			lights[i].SetActive(false);
		}
		renderForEnemy.material.SetTexture ("_EmissionMap", offEmmision);
		//explosionSource.Stop;
		yield return new WaitForSeconds (5);
		instantiated_object_ = (GameObject) Instantiate (explosion, transform.position, transform.rotation);
		Destroy (instantiated_object_, 1f);

		instantiated_object_ = (GameObject) Instantiate (loot, new Vector3(transform.position.x, 14, transform.position.z), transform.rotation);
		Destroy (instantiated_object_, 30f);

		enemy_generator_.GetComponent<EnemyGenerator> ().enemy_count--;

		//explosionSource.Stop;
		Destroy (this.gameObject);

		/* // Code to make it come back to life.
		enemyMovementSource.Play ();
		state = State.patrol;
		navAgent.Resume ();
		health = maxHealth;
		for (int i = 0; i < lights.Length; i++) {
			lights[i].SetActive(true);
		}
		renderForEnemy.material.SetTexture ("_EmissionMap", onEmmision);
		*/

	}

	void checkToSeeIfMoving(){
		if (navAgent.velocity.magnitude < 0.3f) {
			enemyMovementSource.Stop ();
		} else if (!enemyMovementSource.isPlaying) {
			enemyMovementSource.Play();

		}
	}
	
}
