using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addforcetest : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody>();
        rb.AddRelativeForce(new Vector3(0, 1000, 2500));
    }
}
