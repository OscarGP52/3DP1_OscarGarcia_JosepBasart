using TMPro;
using UnityEngine;

public class DianaBehaviour : MonoBehaviour
{
    public GameObject puntoA;
    public GameObject puntoB;
    Vector3 targetPosition;
    public bool isMoving;
    public float speed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPosition = puntoB.transform.position;
        this.gameObject.transform.position = puntoA.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            //se mueve
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, targetPosition, speed * Time.deltaTime);
            //se comprueva que ha llegado al punto B
            if (Vector3.Distance(this.gameObject.transform.position, targetPosition) <= 0.5f)
            {
                if (transform.position == puntoB.transform.position)
                {
                    targetPosition = puntoA.transform.position;
                }
                else
                {
                    targetPosition = puntoB.transform.position;
                }
            }
        }
    }

    // si colisiona con una bala
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bala"))
        {
            //destruye la bala
            Destroy(other.gameObject);
            //destruye a diana y animacion
            this.gameObject.SetActive(false);
            //aumenta la puntuacion
            GameObject.Find("Score").GetComponent<ScoreSystem>().aumentarPuntuacion(1);
        }
    }
}
