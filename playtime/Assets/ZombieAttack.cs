using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{


    public void Attack (int damageAmount)
    {
        FindObjectOfType<AudioManager>().Play("PlayerHit");
        Player.health -= damageAmount;

    }
    public void update ()
    {
        
    }
}
