using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
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

    [Header("Distances")]
    public float m_MinDistanceToAttack = 5.0f;

    [Header("Patrol")]
    public List<Transform> m_PatrolPosition;
    int m_CurrentPatrolPossitionId = 0;

    [Header("Sight")]
    public float m_SightAngle = 60;
    public LayerMask m_SightLayerMask;
    public float m_EyesHeight = 1.8f;

    [Header("Ears")]
    public float m_MaxEarDistance = 3.0f;


    private void Update()
    {
        switch (m_State)
        {
            case TState.IDLE:
                UpdateIdleState();
                break;
            case TState.ALERT:
                UpdateAlertState(); 
                break;
            case TState.PATROL:
                UpdatePatrolState(); 
                break;
            case TState.ATTACK:
                UpdateAttackState();
                break;
            case TState.CHASE:
                UpdateChaseState();
                break;
            case TState.HIT:
                UpdateHitState();
                break;
            case TState.DIE:
                UpdateDieState();
                break;
        }
    }

    void SetIdleState()
    {

        m_State = TState.IDLE;
    }
    void UpdateIdleState() 
    {
        SetPatrolState();
    }
    void SetPatrolState()
    {

        m_State = TState.PATROL;
        m_CurrentPatrolPossitionId = 0;
        MoveToNextPatrolPosition();
    }
    void UpdatePatrolState()
    {
        if (!m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            MoveToNextPatrolPosition();
        if(HearsPlayer)
            SetAlertState();
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
    void SetDieState()
    {

        m_State = TState.DIE;
        gameObject.SetActive(false);
    }
    void UpdateDieState()
    {
    }

    
    void SetNextChasePossition()
    {

        Vector3 l_PlayerPossition=GameManager.GetGameManager().GetPLayer().transform.position;
        Vector3 l_Direction = l_PlayerPossition - transform.position;
        l_Direction.Normalize();
        Vector3 l_Position = l_PlayerPossition - l_Direction * m_MinDistanceToAttack;
        m_NavMeshAgent.destination = l_Position;
    }
    void MoveToNextPatrolPosition()
    {
        Vector3 l_Destination = m_PatrolPosition[m_CurrentPatrolPossitionId].position;
        m_NavMeshAgent.destination = l_Destination;
        ++m_CurrentPatrolPossitionId;
        if(m_CurrentPatrolPossitionId>=m_PatrolPosition.Count)
            m_CurrentPatrolPossitionId=0;
    }
    bool SeesPlayer()
    {
        Vector3 l_PlayerPossition = GameManager.GetGameManager().GetPLayer().transform.position;
        Vector3 l_Direction = l_PlayerPossition - transform.position;
        float l_Distance = l_Direction.magnitude;
        //l_Direction.Normalize();
        l_Direction/=l_Distance;
        float l_DotValue = Vector3.Dot(l_Direction.transform.forward);
        if (l_DotValue >= Mathf.Cos(m_SightAngle * 0.5f * Mathf.Deg2Rad))
        {
           // Ray l_Ray = new Ray(transform.position + Vector3.up * m_EyesHeight, l_Direction);
            if (!Physics.Raycast(l_Ray, l_Distance, m_SightLayerMask.value))
                return true;
        }
        return false;
    }
    bool HearsPlayer()
    {
        Vector3 l_PlayerPossition = GameManager.GetGameManager().GetPlayer().transform.position;
        float l_Distance = Vector3.Distance(l_PlayerPossition, transform.position);
        return l_Distance < m_MaxEarDistance;
    }
    public int m_Life = 50;
    public void Hit(int Damage)
    {
        m_Life -= Damage;
        if (m_Life < 0)
            SetDieState();
    }
    // public class HitCollider : Monobehaviour

    public int m_Damage;
    public EnemyController m_Enemy;

    public void Hit()
    {
        m_Enemy.Hit(m_Damage);
    }
    // Esto va dentro de Shoot() em playercontroller
    //
    //  if(l_RaycastHit.collider.CompareTag("HitCollider"))
    //      l_RayCastHit.collider.GetCComponent<HitCollider>().Hit();
    //  else
    //  CreateShootParticles(.......)


    //esto va en PlayerController
    public void Restart()
    {
        m_CharacterController.enabled = false;
        Transform.position = m_StartPosition;
        Transform.rotation = m_startRotation;
        m_CharacterController.enabled = true;

    }

    //esto va dentro de start() en un if de playercontroller
    l_Player.m_StartRotation=Transform.rotation;
    l_Player.m_StartPosition=Transform.position;
}

