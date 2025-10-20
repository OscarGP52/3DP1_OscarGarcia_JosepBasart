using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class TriggerActivator : MonoBehaviour
{
    public List<GameObject> objects;
    public UIsystem UI;
    private int y = 0;
    private bool activated = false;
    private float timer = 0f;
    private float ControlTime = 0f;
    public float intervalTime = 5f;
    public float MaxTime = 30f;

    private void Start()
    {
        // desactiva todos los objetos al inicio
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }
    }
    private void Update()
    {
        // mire el estado del primer objeto de la lista
        //cuando el primero se desactive, activar el siguiente
        if (activated)
        {
            timer += Time.deltaTime;
            ControlTime += Time.deltaTime;
            UI.UpdateTimer(timer);

            if (y < objects.Count && !objects[y].activeInHierarchy && timer <= MaxTime)
            {
                y++;

                if (y < objects.Count)
                {
                    Next();
                }
            }
            if (ControlTime > intervalTime)
            {
                ControlTime = 0f;
                if (y < objects.Count)
                {
                    objects[y].SetActive(false);
                    y++;
                    Next();
                }
            }
            if (timer > MaxTime)
            {
                UI.StopTimer();
                foreach (GameObject obj in objects)
                {
                    obj.SetActive(false);
                }
                y = 0;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // acttiva el primero de la lista
        if (other.CompareTag("Player"))
        {
            UI.UpdateScore(0);
            activated = true;
            objects[y].SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activated = false;
            foreach (GameObject obj in objects)
            {
                obj.SetActive(false);
            }
            UI.EraseScore();
            UI.EraseTimer();
            timer = 0f;
            ControlTime = 0f;
            y = 0;
        }
    }

    private void Next()
    {
        ControlTime = 0f;
        objects[y].SetActive(true);
    }
}

