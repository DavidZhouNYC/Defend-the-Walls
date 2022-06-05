#pragma strict

public var speedFactor : float;
public var startPosition : Transform;
public var endPosition : Transform;
public var speed : float;
public var explosion : GameObject;
public var max_health : float;
private var health_ : float;

function Start () {
	GetComponent.<Animation>()["Anim_Walk"].normalizedSpeed = speedFactor;
	health_ = max_health;
}

function Update() {
	transform.position = Vector3.MoveTowards(startPosition.position, endPosition.position, speed * Time.time);
}

function OnTriggerEnter(other : Collider) {
	if (other.gameObject.CompareTag ("Gate")) {
		var explosion = Instantiate(explosion, transform.position, transform.rotation);
		Destroy(explosion, 2.0);
		Destroy(other.gameObject);
	}
}
