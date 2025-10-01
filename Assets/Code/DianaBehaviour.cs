using UnityEngine;

public class DianaBehaviour : MonoBehaviour
{
    public GameObject diana;
    public GameObject puntoA;
    public GameObject puntoB;
    Vector3 targetPosition;
    public bool isMoving = false;
    bool playerInside = false;
    float speed = 2.0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (isMoving)
        {
            //diana que se mueve
            targetPosition = puntoB.transform.position;
        }
        else
        {
            //diana que no se mueve
            targetPosition = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered the trigger area.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInside)
        {
            //se mueve
            diana.transform.position = Vector3.MoveTowards(diana.transform.position, targetPosition, speed * Time.deltaTime);
            //se comprueva que ha llegado al punto B

            if (Vector3.Distance(diana.transform.position, targetPosition))
            {

            }
        }
       
        
    }
}
