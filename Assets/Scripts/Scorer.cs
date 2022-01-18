using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    int collisions = 0;

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag != "Bonked")
        {
            collisions++;

            Debug.Log("You have bumped into obstacles " + collisions + " times!");
        }

        collision.gameObject.tag = "Bonked";
    }
}
