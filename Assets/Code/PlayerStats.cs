using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class PlayerStats : MonoBehaviour
{
    public int shield;
    public int life;
    public int ammo;


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
            if (this.gameObject.CompareTag("Life") && !(GameObject.Find("Player").GetComponent<PlayerController>().GetLife() >= GameObject.Find("Player").GetComponent<PlayerController>().GetMaxLife()))
            {
                GameObject.Find("Player").GetComponent<PlayerController>().curar(life);
                Destroy(this.gameObject);
            }
            if (this.gameObject.CompareTag("Shield") && !(GameObject.Find("Player").GetComponent<PlayerController>().GetShield() >= GameObject.Find("Player").GetComponent<PlayerController>().GetMaxShield()))
            {
                GameObject.Find("Player").GetComponent<PlayerController>().recibirEscudo(shield);
                Destroy(this.gameObject);
            }
            if (this.gameObject.CompareTag("Ammo"))
            {
                GameObject.Find("Player").GetComponent<BulletBehavior>().addAmmo(ammo);
                Destroy(this.gameObject);
            }
        }
    }
}
