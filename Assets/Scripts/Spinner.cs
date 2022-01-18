using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField]
    float yRot = 0.3f;

    // Update is called once per frame
    void Update()
    {
        SpinSpinner();
    }

    void SpinSpinner()
    {
        transform.Rotate(0, yRot, 0);
    }
}
