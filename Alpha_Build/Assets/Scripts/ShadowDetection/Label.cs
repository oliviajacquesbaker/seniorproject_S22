using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    public GameObject labelPrefab;
    public GameObject thisLabel;
    public GameObject objectCastingShadow;

    private Vector3 pos;
    private Vector3 rot;
    private Camera cam;
    private float roughShadowArea;

    private void Update()
    {
        if (thisLabel)
        {
            thisLabel.transform.LookAt(new Vector3(Camera.main.transform.position.x, thisLabel.transform.position.y, Camera.main.transform.position.z));
        }
    }

    public void SetTransforms(int anchorX, int anchorY)
    {
        Vector3 test = cam.ScreenToWorldPoint(new Vector3(anchorX * 4, anchorY * 4, 0.01f));
        pos = new Vector3(test.x, 0.5f, test.z);
        rot = new Vector3(0, 90, 0);
        FindNearestShadowCaster();
    }

    public void SetArea(int minX, int maxX, int minY, int maxY)
    {
        cam = GameObject.FindWithTag("shadow_cam").GetComponent<Camera>();
        float across = (cam.ScreenToWorldPoint(new Vector3(minX, minY, 0.01f)) - cam.ScreenToWorldPoint(new Vector3(maxX, maxY, 0.01f))).magnitude;
        roughShadowArea = across * across;
    }

    public void FindNearestShadowCaster()
    {
        float minDist = 100000;
        Collider[] hitColliders = Physics.OverlapSphere(pos, roughShadowArea * 1.2f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "ShadowCaster")
            {
                float dist = (new Vector3(hitCollider.gameObject.transform.position.x, 0, hitCollider.gameObject.transform.position.z) - pos).magnitude;
                //Debug.Log(dist);
                if (dist < minDist)
                {
                    minDist = dist;
                    objectCastingShadow = hitCollider.gameObject;
                }
            }

        }
    }

    public void SetLabelPrefab(GameObject prefab)
    {
        labelPrefab = prefab;
    }

    public void SetInScene()
    {
        thisLabel = Instantiate(labelPrefab, pos, Quaternion.Euler(rot));
        if (thisLabel.GetComponentInChildren<ShadowResponse>()) thisLabel.GetComponentInChildren<ShadowResponse>().SetLabel(this);
    }

    public void CollectShadow()
    {
        if (objectCastingShadow) objectCastingShadow.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        RemoveFromScene();
    }

    public void RemoveFromScene()
    {
        Destroy(thisLabel);
    }

}
