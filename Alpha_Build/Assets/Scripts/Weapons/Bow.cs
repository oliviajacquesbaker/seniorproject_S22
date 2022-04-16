using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bow : MonoBehaviour
{
    float _charge;
    public float chargeMax;
    public float chargeRate;
    public float rotationSpeed;
    private float initialTurnVelocity;
    public float targetTurnVelocity;
    public float fireDecay;
    public KeyCode fireButton;
    public Transform spawn;
    private Transform bowRotation;
    public Rigidbody arrowObj;
    private Rigidbody playerRB;
    private GameObject Player;
    private GameObject bullet;
    private ThirdPersonMovement playerSpeed;
    private CinemachineFreeLook cam;
    //public float maxRotation;
    //public float minRotation;
    private float currentBowRotation;
    public bool isAiming;
    private GameObject mainCamera;
    private GameObject aimCamera;
    private GameObject crosshair;
    private GameObject inv;
    private StateHandler state;
    private Durability durability;
    private CameraController camController;
    bool unaimed;

    public Animator anim;
    public Animator bowAnim;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip drawn, release;

    bool draw = false;
    public Transform aimPoint;
    float currentHitDistance;
    public LayerMask layerMask;
    bool rotated = false;
    bool lockedOn = false;
    GameObject target;
    void Start()
    {
        Player = GameObject.Find("Player");
        playerSpeed = Player.GetComponent<ThirdPersonMovement>();
        anim = Player.GetComponent<Animator>();
        bowAnim = GetComponent<Animator>();
        bowRotation = gameObject.transform;
        cam = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        initialTurnVelocity = playerSpeed.turnSmoothTime;
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        crosshair = GameObject.Find("Crosshair");
        state = GameObject.Find("Main Camera").GetComponent<StateHandler>();
        durability = gameObject.GetComponent<Durability>();
        camController = Player.GetComponent<CameraController>();
        playerRB = Player.GetComponent<Rigidbody>();
        unaimed = true;
        currentHitDistance = 100f;
    }

    void Update()
    {
        if (!state.InvOpen()) // check if inventory is not open
        {
            if (Input.GetKey(fireButton))
            {
                Aim();

                if (_charge < chargeMax)
                {
                    ChargeBow();
                }

            }

            if (Input.GetKeyUp(fireButton))
            {
                Fire();
                StopAiming();
            }
        }
    }

    void ChargeBow()
    {
        _charge += Time.deltaTime * chargeRate;
        isAiming = true;
    }

    void Fire()
    {

        source.clip = release;
        source.Play();
        draw = false;

        anim.SetTrigger("ReleaseArrow");
        bowAnim.SetTrigger("ReleaseArrow");
        Rigidbody arrow = Instantiate(arrowObj, spawn.transform.position, spawn.transform.rotation * Quaternion.Euler(270f, 0f, 0f)) as Rigidbody;
        if (lockedOn && !target.GetComponentInParent<Boss>())
        {
            spawn.LookAt(target.transform);
        }
        arrow.AddForce(spawn.forward * _charge, ForceMode.Impulse);
        _charge = 0;
        durability.currDurability -= fireDecay;
        isAiming = false;
    }

    void Aim()
    {
        if (!draw)
        {
            source.clip = drawn;
            source.Play();
            draw = true;
        }
        if (unaimed)
        {
            anim.SetBool("PutDownBow", false);
            anim.SetTrigger("DrawBow");
            bowAnim.SetTrigger("DrawBow");
            unaimed = false;
        }

        RotatePlayer();
        rotated = true;
        camController.Aim();
        Player.transform.Rotate(0.0f, Input.GetAxis("Mouse X"), 0.0f);
        //spawn.transform.Rotate(Input.GetAxis("Mouse Y") * -1f, 0.0f, 0.0f);
        spawn.transform.rotation = aimPoint.transform.rotation;
        CastAimRay();
    }

    void StopAiming()
    {
        bowAnim.SetTrigger("ReleaseArrow");
        anim.SetBool("PutDownBow", true);
        spawn.transform.rotation = GameObject.Find("Follow Target").transform.rotation;
        camController.StopAim();
        unaimed = true;
        rotated = false;
    }

    void RotatePlayer()
    {
        if (!rotated)
        {
            Player.transform.localEulerAngles = new Vector3(Player.transform.rotation.x, cam.m_XAxis.Value, Player.transform.rotation.z);
        }
    }

    void CastAimRay()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        //float range = 100f;

        if (Physics.SphereCast(ray, 2f, out hit, 50f, layerMask))
        {
            target = hit.transform.gameObject;
            //Debug.Log("Aiming at " + hit.transform.gameObject.name);
            var objectHit = hit.transform.gameObject;
            currentHitDistance = hit.distance;
            if (hit.transform.gameObject.GetComponent<_AIStats>() || hit.transform.gameObject.GetComponent<StatsLinker>() || hit.transform.gameObject.GetComponent<LightShatter>())
            {
                camController.ToggleTargetCrosshair();
                Debug.Log("Aiming at " + hit.transform.gameObject.name);
                lockedOn = true;
                //Player.transform.LookAt(objectHit.transform);
                //RotateTowardsEnemy(hit.transform);
            }
            else
            {
                camController.ToggleNormalCrosshair();
            }
        }
        else
        {
            currentHitDistance = 100;
        }
    }

    void RotateTowardsEnemy(Transform target)
    {
        Vector3 targetDirection = target.position - Player.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(Player.transform.forward, targetDirection, rotationSpeed * Time.deltaTime, 0.0f);
        Player.transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void OnDrawGizmosSelected()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        Gizmos.color = Color.red;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * currentHitDistance, Color.green);
        Gizmos.DrawWireSphere(ray.origin + ray.direction * currentHitDistance, 2f);
    }
    // IEnumerator CrosshairDelay(float seconds)
    // {
    //     yield return new WaitForSeconds(seconds);
    //     crosshair.SetActive(true);
    // }

}
