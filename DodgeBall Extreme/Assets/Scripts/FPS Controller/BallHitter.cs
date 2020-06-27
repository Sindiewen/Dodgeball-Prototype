using UnityEngine;

public class BallHitter : MonoBehaviour
{
    // public Cube col;
    public int ballHitStrengthForward = 500;
    public int ballHitStrengthUp = 250;
    public float maxDistance;
    public float cooldown;
    private float cooldownTimer;

    public BoxCollider col;
    private PlayerGameControlelr pgc;
    private RaycastHit raycastHit;
    private bool hit;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
        pgc = GetComponentInParent<PlayerGameControlelr>();
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }


    public void ballHitInputPassthrough()
    {
        hit = Physics.BoxCast(col.bounds.center, col.size, transform.forward, out raycastHit, transform.parent.rotation, maxDistance);
        if (hit && cooldownTimer <= 0)
        {
            cooldownTimer = cooldown;
            Debug.Log("Player " + transform.name + " Hit object " + raycastHit.collider.name);
            Rigidbody sphereRB = raycastHit.transform.GetComponent<Rigidbody>();
            sphereController sphere = raycastHit.transform.GetComponent<sphereController>();

            // Calculate hit
            sphereRB.velocity = Vector3.zero;
            sphereRB.angularVelocity = Vector3.zero;
            float yHitDir = 0;
            if(transform.forward.y >= 0)
            {
                yHitDir = (transform.forward.y + 3) * ballHitStrengthUp;
            }
            else
            {
                yHitDir = (transform.forward.y * 2) * ballHitStrengthUp;
            }

            // Add force to ball to hit it away
            sphereRB.AddForce(new Vector3(transform.forward.x * ballHitStrengthForward, yHitDir, transform.forward.z * ballHitStrengthForward));

            // determine if friendly or enemy
            if (pgc.getTeam.currentTeam == sphere.GetTeam.currentTeam)
            {
                // If friendly, heal 
                if (pgc.HP < pgc.maxHP)
                {
                    pgc.HP += 1;
                    Debug.Log("Punched friendly ball, healing by 1");
                }
            }
            else
            {
                // if enemy, take 1 hit and turn ball friendly
                pgc.HP -= 1;
                if (sphere.GetTeam.currentTeam == TeamManager.Team.BLU)
                {
                    sphere.setTeam("red");
                }
                else
                {
                    sphere.setTeam("blu");
                }
                Debug.Log("Punched enemy ball, hit by 1, switched ball to friendly side");
            }
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
