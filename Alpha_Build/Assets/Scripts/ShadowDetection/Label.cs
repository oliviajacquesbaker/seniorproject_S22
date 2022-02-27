using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    public Vector3 pos;
    Vector3 rot;
    public GameObject labelPrefab;
    public GameObject thisLabel;
    Camera cam;

    public void SetTransforms(int anchorX, int anchorY)
    {
        cam = GameObject.FindWithTag("shadow_cam").GetComponent<Camera>();
        Vector3 test = cam.ScreenToWorldPoint(new Vector3(anchorX, anchorY, 0.01f));
        pos = new Vector3(test.x, 0.5f, test.z);
        rot = new Vector3(0, 90, 0);
    }

    public void SetLabelPrefab(GameObject prefab)
    {
        labelPrefab = prefab;
    }

    public void SetInScene()
    {
        thisLabel = Instantiate(labelPrefab, pos, Quaternion.Euler(rot));
    }

    void UpdateInScene()
    {
        thisLabel.transform.localRotation = Quaternion.Euler(rot);
    }

    public void RemoveFromScene()
    {
        Destroy(thisLabel);
    }
}
