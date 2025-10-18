using UnityEngine;

public class HitCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int m_Damage = 10;
    public EnemyController m_Enemy;
    public void Hit()
    {
        m_Enemy.Hit(m_Damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bala"))
        {
            Debug.Log("HitCollider: Hit by bullet");
            m_Enemy.Hit(m_Damage);
        }
    }
}
