using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocateShadows : MonoBehaviour
{
    [SerializeField]
    IdentifyShadows id;
    Color[] currShadowImg;
    ShadowType currAvailShadow;
    Vector3 pastPos;

    enum ShadowType
    {
        circle,
        oval,
        rect,
        square,
        line,
        unknown,
        none
    }

    void Start()
    {
        pastPos = this.gameObject.transform.position;
        currShadowImg = id.DetectShadows();
        GetCurrentAvailableshadow();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currAvailShadow);
        /*float dist = Vector3.Distance(this.gameObject.transform.position, pastPos);
        Debug.Log(dist);
        if (dist > 4)
        {
            Debug.Log("-----------------------------------------------------------------------------------------");
            pastPos = this.gameObject.transform.position;
            currShadowImg = id.DetectShadows();
            GetCurrentAvailableshadow();
        }*/
    }

    ShadowType GetCurrentAvailableshadow()
    {

        int ind = currShadowImg.Length / 2; 
        while(ind < currShadowImg.Length)
        {
            if (currShadowImg[ind].g == 255 && currShadowImg[ind].r == 255 && currShadowImg[ind].b == 255) continue;
            else if (currShadowImg[ind].g == 10 && currShadowImg[ind].r == 10) return ShadowType.oval;
            else if (currShadowImg[ind].g == 10 && currShadowImg[ind].b == 10) return ShadowType.line;
            else if (currShadowImg[ind].r == 10 && currShadowImg[ind].b == 10) return ShadowType.unknown;
            else if (currShadowImg[ind].g == 10) return ShadowType.square;
            else if (currShadowImg[ind].b == 10) return ShadowType.rect;
            else if (currShadowImg[ind].r == 100) return ShadowType.circle;
            ind += Screen.width;
        }

        return ShadowType.none;
    } 
}
