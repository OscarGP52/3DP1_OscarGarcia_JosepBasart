using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour
{
    public float m_DestroyOnTime = 3.0f;
 
    void Start()
    {
        StartCoroutine(DestroyOnTimeFn());  
    }

    IEnumerator DestroyOnTimeFn()
    {
        yield return new WaitForSeconds(m_DestroyOnTime);
        GameObject.Destroy(gameObject);
    }

}
