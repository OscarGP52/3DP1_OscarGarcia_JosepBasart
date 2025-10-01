using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class TriggerActivator : MonoBehaviour
{
    public List<GameObject> objects;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject d in objects) { d.SetActive(false); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject go in objects) { go.SetActive(true); }
        }
    }

    // Update is called once per frame
    void Update()
    {

     
    }
}

