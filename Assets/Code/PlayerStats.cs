using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class PlayerStats : MonoBehaviour
{
    PlayerController player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("Life") && !(GameObject.Find("Player").GetComponent<PlayerController>().GetCurrrentLife() >= player.maxLife))
            {
                player.Curar(player.lifePoints);
                Destroy(this.gameObject);
            }
            if (this.gameObject.CompareTag("Shield") && !(GameObject.Find("Player").GetComponent<PlayerController>().GetCurrentShield() >= player.maxShield))
            {
                player.RecibirEscudo(player.shieldPoints);
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
