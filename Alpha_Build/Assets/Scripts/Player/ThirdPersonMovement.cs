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

    void Start()
    {
        Player = GameObject.Find("Player");
        enabled = true;
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float multiply = 1;
            if (Input.GetKey(runkey) && !anim.GetBool("ShieldEnabled"))
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
            if (enabled)
            {
                transform.Translate(multiply * moveDirection * speed * Time.deltaTime, Space.World);
            }
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
        }

    }
}
