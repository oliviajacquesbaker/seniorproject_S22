using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private bool playerDet = false;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Collider detCol;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!detCol.enabled) { return; }

        if (other.CompareTag("Player"))
        {
            playerDet = true;
        }
    }

    public void SetDet()
    {
        playerDet = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDet = false;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerDet)
        {
            //Follow player!
            transform.LookAt(player.transform);

            float step = speed * Time.deltaTime;

            //Move toward Player!
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);

            //Animation stuff.

            _animator.SetBool("PlayerDetected", true);
        }
        else if(!playerDet)
        {
            _animator.SetBool("PlayerDetected", false);
        }
    }
}
