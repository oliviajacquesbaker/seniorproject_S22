using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _raytest : MonoBehaviour
{
    //thickness of SphereCast
    [SerializeField]
    float thickness = 1f;
    [SerializeField]
    bool debugMode = false;

    private float currentHitDistance;
    private Vector3 collision = Vector3.zero;
    [SerializeField]
    private GameObject lastHit;
    RaycastHit hit;

    public Vector3 origin;
    public Vector3 direction;

    void Update()
    {

        origin = transform.position + new Vector3(0, 0, 0);
        direction = transform.TransformDirection(Vector3.forward);
        if (Physics.SphereCast(origin,thickness,direction, out hit))
        {
            lastHit = hit.transform.gameObject;

            collision = hit.point;

            currentHitDistance = hit.distance;

            if (lastHit.tag == "Player")
            {
                //_playerStats player = hit.transform.GetComponent<_playerStats>();

                //player.DetractHealthLight();
            }

        }
    }

    //Debugging
    private void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = Color.red;

            //Shows point of contact
            Gizmos.DrawWireSphere(collision, 0.2f);

            //Shows area of contact
            Gizmos.DrawWireSphere(origin + direction * currentHitDistance, thickness);
        }
    }
}
