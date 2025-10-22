using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetGameManager().GetPlayer().SetCheckPoint(transform.position);
        }
    }
}
