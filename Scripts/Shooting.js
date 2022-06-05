#pragma strict
 
public var projectile : Rigidbody;
public var shotPos : Transform;
public var shotForce = 1000f;
public var moveSpeed = 10f;
     
function Start () {
 
}
 
function Update () {
    if(Input.GetButtonUp ("Fire1"))
        {
            var shot : Rigidbody = Instantiate(projectile,shotPos.position,shotPos.rotation) as Rigidbody;
            shot.AddForce(shotPos.forward * shotForce);
        }
}