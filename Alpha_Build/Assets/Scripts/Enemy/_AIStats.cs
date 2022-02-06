using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _AIStats : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;
    [SerializeField]
    private MobType type = MobType.Patrol;

    void Start()
    {
       // health = 100f; 
       // ^ swapped this to be up where you declare it, so that it shows as a default val in the serializable field area
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float diffHealth)
    {
        health = diffHealth;
    }    

    public void Heal(float addHealth)
    {
        health += addHealth;
    }

    public void Damage(float removeHealth)
    {
        health -= removeHealth;
        if (health <= 0) Kill();
    }

    public void Kill()
    {
       /* switch (type){
            case MobType.Booster:
                gameObject.GetComponent<BoosterMob>().OnDeath();
                break;
            default:
                Debug.Log("set up similar functions in the other mob types :)");
                break;
        }*/

    }
}