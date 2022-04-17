using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttack : MonoBehaviour
{
    private bool inside = false;

    [SerializeField]
    private Light lightTrue;

    [SerializeField]
    private List<GameObject> loadingSlices;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Material loadMaterial, clearMaterial;

    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    private AudioSource source;

    private int counter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            InvokeRepeating("StartLoad", 0f, .5f);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            CancelInvoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartLoad()
    {
        loadingSlices[counter].GetComponent<MeshRenderer>().material = loadMaterial;
        lightTrue.intensity += .1f;
        counter++;

        if (counter == 21) {

            FireProj();

            counter = 0;

            for (int i = 0; i < 21; i++)
            {
                loadingSlices[i].GetComponent<MeshRenderer>().material = clearMaterial;
            }
            lightTrue.intensity = 0f;
            CancelInvoke();
            InvokeRepeating("StartLoad", 1.5f, .5f);
        }
    }

    [SerializeField]
    private float projSpeed;
    [SerializeField]
    private Projectile projectilePrefab;

    private void FireProj()
    {
        source.Play();

        var position = spawnPoint.transform.position + spawnPoint.transform.forward;
       
        var rotation = spawnPoint.transform.rotation;
       
        var projectile = Instantiate(projectilePrefab, position, rotation);

        Vector3 lookAtPos = player.transform.position;

        projectile.Fire(projSpeed, Vector3.Normalize(spawnPoint.transform.forward));
    }

}
