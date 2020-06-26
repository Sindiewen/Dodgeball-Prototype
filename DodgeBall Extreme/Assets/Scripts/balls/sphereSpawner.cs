using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereSpawner : MonoBehaviour
{
    public List<Vector3> spawnLocs;
    public GameObject ballsToSpawn;
    private GameObject ballHolder;
    public Vector2 ballSpawnIntervalRange;
    private int currentSpawnedBalls = 0;
    public int maxBallsInScene = 6;

    private void Start()
    {
        ballHolder = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {   
        // If the number of balls the child has is less then the max that should be in the scene, create balls
        if (currentSpawnedBalls < maxBallsInScene)
        {
            currentSpawnedBalls += 1;
            Invoke("createBall", Random.Range(ballSpawnIntervalRange.x, ballSpawnIntervalRange.y));
        }
    }

    private void createBall()
    {
        int spawnPos = (int)Random.Range(0, spawnLocs.Count);
        GameObject clone = Instantiate(ballsToSpawn, transform.position + spawnLocs[spawnPos], Quaternion.identity);
        sphereController sphere = clone.GetComponent<sphereController>();
        if(spawnPos % 2 == 0)
        {
            sphere.setTeam("blu");
        }
        else
        {
            sphere.setTeam("red");
        }
    }


    private void OnDrawGizmos()
    {
        for(int i = 0; i < spawnLocs.Count; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + spawnLocs[i], 1);
        }
    }
}
