using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPipe : MonoBehaviour
{
    bool rotated = false;
    [SerializeField]
    IdentifyShadows id;
    [SerializeField]
    GameObject childPipe;
    void Update()
    {
        if (!rotated && Input.GetKeyDown(KeyCode.P))
        {
            this.gameObject.transform.Rotate(0, 5, 105);
            childPipe.transform.localEulerAngles = new Vector3(0, 0, 0);
            id.DetectShadows(ShadowType.rect);
            rotated = true;
        }
    }
}
