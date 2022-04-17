using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMaterial : MonoBehaviour
{
    [SerializeField]
    Material disabled;
    [SerializeField]
    SkinnedMeshRenderer boss;

    public void Swap()
    {
        var mats = boss.materials;
        bool hit = false;
        for(int i=0; i < mats.Length; ++i)
        {
            Debug.Log(mats[i].name);
            if(mats[i].name == "glowing (Instance)")
            {
                if(hit) mats[i] = disabled;
                hit = true;
            }
        }
        boss.materials = mats;
        Debug.Log("Swapped");
    }
}
