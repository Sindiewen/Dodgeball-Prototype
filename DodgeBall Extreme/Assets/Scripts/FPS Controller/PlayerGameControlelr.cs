using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameControlelr : MonoBehaviour
{
    [Header("Ball Checking")]
    public float ballCheckRadius;
    public LayerMask collisionMask;
    // private RaycastHit ballHitCheck;
    private Collider[] hits;
    private bool hit;
    
    // Component references
    private TeamManager team;
    
    private void Start()
    {
        team = GetComponent<TeamManager>();
    }

    private void Update()
    {
        // Initate checking incoming balls if friendly
        checkBall();
    }

    private void checkBall()
    {
        // hit = Physics.SphereCast(transform.position, ballCheckRadius, Vector3.forward, out ballHitCheck, 0f, collisionMask);
        hits = Physics.OverlapSphere(transform.position, ballCheckRadius, collisionMask);

        if (hits.Length > 0)
        {
            for(int i = 0; i < hits.Length; ++i)
            {
                // sphereController sphere = ballHitCheck.transform.GetComponent<sphereController>();
                sphereController sphere = hits[i].GetComponent<sphereController>();

                // Ball is on same team, set color to friendly  
                if(team.currentTeam == sphere.GetTeam.currentTeam) 
                {
                    sphere.setColorFriendly();
                }
                else
                {
                    sphere.setColorEnemy();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, ballCheckRadius);
    }


}
