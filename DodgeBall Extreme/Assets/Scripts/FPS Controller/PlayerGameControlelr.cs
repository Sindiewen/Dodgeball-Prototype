using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameControlelr : MonoBehaviour
{
    [Header("Ball Checking")]
    public float ballCheckRadius;
    public LayerMask collisionMask;
    private Collider[] hits;
    private bool hit;


    [Header("Player health")]
    public float hitRadius;
    public Vector3 hitPoint0;
    public Vector3 hitPoint1;
    private Collider[] hitsPlayer;
    private bool hitPlayer;
    public int HP;
    public int maxHP;
    public float healCooldown;          // because ovelap sphere checks every frame, set it so that you heal after a set time, not every frame. also for taking damage
    private float healCooldownTimer;    // for cooldown above
    
    
    // Component references
    private TeamObject team;
    
    private void Start()
    {
        team = GetComponent<TeamObject>();
    }

    private void Update()
    {
        // Initate checking incoming balls if friendly
        checkBallIncoming();

        // Checking if ball hits player
        checkBallHitPlayer();

        if (healCooldownTimer > 0)
        {
            healCooldownTimer -= Time.deltaTime;
        }
        if (HP <= 0)
        {
            playerDeath();
        }
    }

    private void checkBallIncoming()
    {
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

    private void checkBallHitPlayer()
    {
        hitsPlayer = Physics.OverlapCapsule(transform.position + hitPoint0, transform.position + hitPoint1, hitRadius, collisionMask);
        if (hitsPlayer.Length > 0 && healCooldownTimer <= 0)
        {
            healCooldownTimer = healCooldown;
            for(int i = 0; i < hitsPlayer.Length; ++i)
            {
                sphereController sphere = hitsPlayer[i].GetComponent<sphereController>();
                if(team.currentTeam != sphere.GetTeam.currentTeam)
                {
                    Debug.Log("Enemy Ball " + hitsPlayer[i].gameObject.name + " has destroyed " + gameObject.name);
                    playerDeath();
                }
                else
                {
                    Debug.Log("Friendly Ball " + hitsPlayer[i].gameObject.name + " has hit " + gameObject.name + ". Healing by 1");
                    if(HP < maxHP)
                    {
                        HP += 1;
                    }
                }
            }
        }
    }
    

    private void playerDeath()
    {
        Debug.Log("Player has died");
        HP = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, ballCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + hitPoint0, hitRadius);
        Gizmos.DrawWireSphere(transform.position + hitPoint1, hitRadius);
        // Gizmos.DrawWireCube(transform.position + hitRadius, new Vector3(transform.position.x + hitRadius, transform.position + (hitPoint0.y + hitPoint1.y),  )
        
    }


    public TeamObject getTeam
    {
        get { return team; }
    }

}
