using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBooster : MonoBehaviour
{
    [SerializeField]
    BoosterMob companion;
    [SerializeField]
    BoosterMob self;

    public bool isMainBooster;


    void Start()
    {
        if (!isMainBooster)
        {
            self.enabled = false;
            companion.Double();
        }
    }

    public void Died()
    {
        self.ReduceBoss();
        if (!isMainBooster)
        {
            self.enabled = true;
            companion.UnDouble();
        }
        else
        {
            companion.enabled = true;
            self.UnDouble();
        }
    }
}
