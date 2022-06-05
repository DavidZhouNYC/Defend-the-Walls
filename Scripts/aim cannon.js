#pragma strict

// pragma strict means that you will explicitly state what each variable is
// we did that in processing so it's nothing new. but in javascript you don't
// necessarily have to declare a variable type, but it's good practice to do so

// for example, this line creates a variable named 'projectile' that is of type 'Transform'
// meaning it stores a position, rotation, and scale
// in the editor we can place anything here that has a 'transform' which is basically anything!
public var projectile : Rigidbody;

// where are we going to shoot FROM? this is another transform. Unity wouldn't know automatically
// that we want to fire out of the end of the cannon. We have to tell it where that is.
public var shootFrom : Transform;

// what speed will the bullet fire out of the cannon?
public var bulletSpeed : float = 1000;

function Start () {
	// nothing to set up
}

function Update () {

	// get mouse input store to vars
	var canX = Input.GetAxis("Mouse X");
	var canY = Input.GetAxis("Mouse Y");
	
	// rotate the cannon based on those variables
	// Space.World makes sure we rotate based on the global coordinate system as opposed to the
	// relative transform of the object in question. We'll talk about this later.
	transform.Rotate(0, canX, canY, Space.World);
	
	// is mouse firin'?
	if (Input.GetButtonDown("Fire1")) {
		// Instantiate the projectile at the position and rotation of this transform
		//var clone : Transform;
		// the instantiate function creates an instance of a prefab, in our case the 'projectile'
		// it requires a position and rotation at which to instantiate said object
		//clone = Instantiate(projectile, shootFrom.transform.position, shootFrom.transform.rotation);
		// Add force to the cloned object in the object's forward direction
		//clone.GetComponent.<Rigidbody>().AddForce(shootFrom.transform.forward * bulletSpeed);

		var shot : Rigidbody = Instantiate(projectile, shootFrom.position, shootFrom.rotation) as Rigidbody;
        shot.AddForce(shootFrom.forward * bulletSpeed);
		
		// finally, make sure that we destroy the projectile after 3 seconds
		// this helps avoid lag from too many game objects being around
		//Destroy(clone.gameObject, 3);
	}
}