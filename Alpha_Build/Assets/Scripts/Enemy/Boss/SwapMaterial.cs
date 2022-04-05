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
        mats[5] = disabled;
        boss.materials = mats;
        Debug.Log("Swapped");
    }
}
