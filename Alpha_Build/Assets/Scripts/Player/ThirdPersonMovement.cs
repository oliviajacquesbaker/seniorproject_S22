using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public Transform cam;
    private GameObject Player;
    public float speed = 6f;
    public float rotationSpeed = 1f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public bool enabled;
    public Animator anim;
    public KeyCode runkey;
    private CharacterController controller;
    private Rigidbody rb;
    private _PlayerStats playerStats;
    public bool isGrounded;
    private float playerHeight = 2.23f;
    //public LayerMask layerMask;
    private Vector3 velocity;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip step1, step2, step3, step4, step5;


    void Start()
    {
        Player = GameObject.Find("Player");
        enabled = true;
        anim = GetComponent<Animator>();
        rb = Player.GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        playerStats = GetComponent<_PlayerStats>();
    }

    void Update()
    {
        if (!playerStats.GetAlive()) { return; }

        // if (controller.isGrounded)
        // {
        //     Debug.Log("Touching the floor");
        // }

        // if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight + 0.1f))
        // {
        //     Debug.Log("Hitting " + hit.transform.gameObject.name);
        //     //if (hit.transform.gameObject.tag == "Player") return;
        //     Debug.Log("Grounded");
        //     isGrounded = true;
        // }
        // else
        // {
        //     Debug.Log("Not grounded");
        //     isGrounded = false;
        // }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight + 0.1f);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        //gravity
        velocity.y += -9.8f * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float multiply = 1;
            if (Input.GetKey(runkey) && !anim.GetBool("ShieldEnabled") && enabled)
            {
                multiply = 2.5f;
                anim.SetBool("Running", true);
                anim.SetBool("Walking", false);
            }
            else
            {
                anim.SetBool("Walking", true);
                anim.SetBool("Running", false);
            }
            if (anim.GetBool("ShieldUp")) multiply = 0.5f;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            if (enabled)
            {
                //MovePlayer();
                // if (!isGrounded)
                // {
                //     //moveDirection.y -= 10f * Time.deltaTime;
                //     //controller.Move(moveDirection * Time.deltaTime);
                //     transform.Translate(Vector3.down * Time.deltaTime);
                // }
                // else
                // {
                controller.Move(moveDirection.normalized * multiply * speed * Time.deltaTime);
                //}
                //rb.velocity = new Vector3(speed, 0, speed * -1);
                //transform.Translate(multiply * moveDirection * speed * Time.deltaTime, Space.World);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
        }


        //MovementSound();
    }


    /*private void MovementSound()
    {
        AnimatorStateInfo animInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (anim.GetBool("Walking"))
        {
            Debug.Log(animInfo.normalizedTime);
            if(animInfo.normalizedTime == 0 || (animInfo.normalizedTime > .5 && animInfo.normalizedTime < .6))
            {
                source.Stop();
                Debug.Log("Walking");
                source.clip = step1;
                source.Play();
            }
        }

    }*/

    public void FootStep()
    {
        int randChoice = Random.Range(1, 6);
        switch (randChoice)
        {
            case 1:
                source.clip = step1;
                break;
            case 2:
                source.clip = step2;
                break;
            case 3:
                source.clip = step3;
                break;
            case 4:
                source.clip = step4;
                break;
            case 5:
                source.clip = step5;
                break;
        }

        source.Play();
    }

    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = new Vector3(speed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = new Vector3(speed * -1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector3(0, 0, speed * -1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector3(0, 0, speed);
        }
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }

}
