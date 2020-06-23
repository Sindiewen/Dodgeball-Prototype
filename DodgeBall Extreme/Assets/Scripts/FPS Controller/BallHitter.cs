using UnityEngine;

public class BallHitter : MonoBehaviour
{
    public int ballHitStrengthForward = 500;
    public int ballHitStrengthUp = 250;
    public float maxDistance;

    public BoxCollider col;
    private RaycastHit raycastHit;
    private bool hit;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }


    public void ballHitInputPassthrough()
    {

        // TODO: Look into determining hit direction by creating a ring around the ball and determine the force direction based on the position of the player to hit the ball
        // when the player hits the ball, and activates thefunvtion to hit the ball in the ball class itself.
        if (hit)
        {
            Debug.Log("Player " + transform.name + " Hit object " + raycastHit.collider.name);
            Rigidbody sphereRB = raycastHit.transform.GetComponent<Rigidbody>();
            sphereRB.AddRelativeForce(new Vector3(0, ballHitStrengthUp, ballHitStrengthForward));
        }
    }

    private void Update()
    {
        hit = Physics.BoxCast(col.bounds.center, col.size, transform.forward, out raycastHit, transform.parent.rotation, maxDistance);
        if(hit)
        {
            raycastHit.transform.eulerAngles = new Vector3(raycastHit.transform.rotation.eulerAngles.x, transform.parent.eulerAngles.y, raycastHit.transform.eulerAngles.z);
        }
    }



    //Draw the BoxCast as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Check if there has been a hit yet
        if (hit)
        {
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, transform.forward * raycastHit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + transform.forward * raycastHit.distance, transform.localScale);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, transform.forward * maxDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + transform.forward * maxDistance, transform.localScale);
        }
    }
}
