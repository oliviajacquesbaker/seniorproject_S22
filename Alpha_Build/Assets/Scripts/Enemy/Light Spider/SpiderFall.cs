using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFall : MonoBehaviour
{
    [SerializeField]
    private _AIStats spiderStats;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Light spotlight;

    [SerializeField]
    private GameObject meleePrefab;

    bool notRun = true;

    private void LateUpdate() //Check to see if spider should fall every frame (wasteful! please try to refactor later?)
    {
        Fall();
    }

    private void Fall()
    {
        if (spiderStats.GetHealth() < 5f && notRun) //If spiders health is less than 5, and this if block has not run before then play the spider fall animation
            //and add gravity to the spider. After 3 seconds (giving it time to land) call the Switcher function.
        {
            spotlight.intensity = 0;
            spotlight.GetComponent<LightScript>().gameObject.SetActive(false);
            _animator.Play("Spider Fall");
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<BoxCollider>();
            notRun = false;
            Invoke("Switcher", 3);
        }
    }
    
    private void Switcher() //Swap the spotlight spider for the normal spider mob.
    {
        GameObject meleeSpider = Instantiate(meleePrefab, gameObject.transform.position, meleePrefab.transform.rotation);
        Destroy(gameObject);
    }

}


