using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class PlayerStats : MonoBehaviour
{
    PlayerController player;
    int shieldPoints = 20;
    int lifePoints = 20;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Life") && !(GameObject.Find("Player").GetComponent<PlayerController>().GetCurrentLife() >= player.maxLife))
            {
                player.Curar(lifePoints);
                Destroy(this.gameObject);
            }
            if (this.gameObject.CompareTag("Shield") && !(GameObject.Find("Player").GetComponent<PlayerController>().GetCurrentShield() >= player.maxShield))
            {
                player.RecibirEscudo(shieldPoints);
                Destroy(this.gameObject);
            }
            if (this.gameObject.CompareTag("Ammo"))
            {
                //player.addammo;
                Destroy(this.gameObject);
            }
        }
    }
}
