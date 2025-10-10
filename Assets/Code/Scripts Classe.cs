using NUnit.Framework;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

public class AmmoIteem : Item
{

    //Esto es AmmoItem : Item
    public int m_AmmoCount;

    public virtual void Pick()
    {
        base.Pick();
        GameManager.GetGameManager().GetPLayer().AddAmmo(m_AmmoCount);
    }

    public override bool CanPick()
    {
        return true;
    }

    //Esto va en PlayerController

    public void AddAmmo()
    {
        m_AmmoCount += Ammo;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item l_Item = other.GetComponent<Item>();
            if (l_Item.CanPick())
                l_Item.Pick();
        }
    }
    // Esto es EnemyController : Monbehaviour

    enum TState
    {
        IDLE = 0,
        PATROL,
        ALERT,
        ATTACK,
        CHASE,
        HIT,
        DIE
    }

    TState m_State;

    NavMeshAgent m_NavMeshAgent;

    public Transform m_Target;

    private void Awake()
    {
        m_NavMeshAgent = GetComponent.Get<NavMeshAgent>();
    }

    void SetIdleState()
    {

        m_State = TState.IDLE;
    }
    void UpdateIdleState() 
    {
    }
    void SetPatrolState()
    {

        m_State = TState.PATROL;
    }
    void UpdatePatrolState()
    {
    }
    void SetAlertState()
    {

        m_State = TState.ALERT;
    }
    void UpdateAlertState()
    {

    }
    void SetAttackState()
    {

        m_State = TState.ATTACK;
    }
    void UpdateAttackState()
    {
    }
    void SetChaseState()
    {

        m_State = TState.CHASE;
    }
    void UpdateChaseState()
    {
    }
    void SetHitState()
    {

        m_State = TState.HIT;
    }
    void UpdateHitState()
    {
    }
    void SetDieIdleState()
    {

        m_State = TState.DIE;
    }
    void UpdateDieState()
    {
    }

    public float m_MinDistanceToAttack = 5.0f;
    void SetNextChasePossition()
    {

        Vector3 l_PlayerPossition=GameManager.GetGameManager().GetPLayer().transform.position;
        Vector3 l_Direction = l_PlayerPossition - transform.position;
        l_Direction.Normalize();
        Vector3 l_Position = l_PlayerPossition - l_Direction * m_MinDistanceToAttack;
        m_NavMeshAgent.destination = l_Position;


    }
    public List<Transform> m_PatrolPosition;
    int m_CurrentPatrolPossitionId = 0;

    void MoveToNextPatrolPosition()
    {
        Vector3 l_Destination = m_PatrolPosition[m_CurrentPatrolPossitionId].position;
        m_NavMeshAgent.destination = l_Destination;
        ++m_CurrentPatrolPossitionId;
        if(m_CurrentPatrolPossitionId>=m_PatrolPosition.count)
            m_CurrentPatrolPossitionId=0;
    }
}

