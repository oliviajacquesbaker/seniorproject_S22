using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    float initY;
    [SerializeField]
    GameObject toFollow;

    //just want this thing to follow but not rotate w parent
    void Start()
    {
        initY = this.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Transform parent = toFollow.transform;
        this.gameObject.transform.position = new Vector3(parent.position.x, initY, parent.position.z);
    }
}
