#pragma strict

public var TheExplosion : GameObject;
public var Sparks : GameObject; // The Particle Effect of the Fire
public var fire_light : GameObject; // The Light emitted from the Fire
public var explosion_sound : AudioClip;


function Start () {

}

function Update () {

}

function OnCollisionEnter() {
	// destroy this game object
	Destroy(this.gameObject);

	// create an explosion there!
	//var explosion : GameObject;
	var explosion = Instantiate(TheExplosion, transform.position, transform.rotation);
	AudioSource.PlayClipAtPoint(explosion_sound, transform.position);
	var spark = Instantiate(Sparks, transform.position, transform.rotation);
	var fire_shine = Instantiate(fire_light, transform.position, transform.rotation);   // Declares it as a variable so we can destory it later

	// check the player position
	var playerPos = gameObject.FindWithTag("Player");

	// What is the distance beyween the player and the bomb at this moment
	var distance = Vector3.Distance(playerPos.transform.position, transform.position);
	
	if (distance < 10) {
		// load the losing screen
		//Application.LoadLevel("LoseScene");
	}

    // Destroys the point light that the fire generates.
	Destroy(fire_shine, 8.0);
	Destroy(explosion, 0.2);
	Destroy(spark, 15.0);
}