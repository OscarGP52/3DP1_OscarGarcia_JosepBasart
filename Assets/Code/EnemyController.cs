using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;



public class EnemyController : MonoBehaviour
{
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
    public float m_MinDistanceToAttack = 2.0f;

    [Header("Patrol")]
    public List<Transform> m_PatrolPosition;
    int m_CurrentPatrolPossitionId = 0;

    [Header("Sight")]
    public float m_SightAngle = 60;
    public LayerMask m_SightLayerMask;
    public float m_EyesHeight = 1.8f;

    [Header("Ears")]
    public float m_MaxEarDistance = 3.0f;

    [Header("Cooldowns")]
    int m_CooldownToAttack = 0;
    public int m_MaxCooldownToAttack = 1000;

    [Header("Life")]
    public int m_Life = 50;
    public int m_MaxLife = 50;

    [Header("LifeBar")]
    public Transform m_LifeBarTransform;
    public LifeBarElementUI m_LifeBarElementUI;


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

        //UpdateLifeBarUI();
        m_CooldownToAttack++;
    }

    /*void UpdateLifeBarUI()
    {
        m_LifeBarElementUI.Show(m_LifeBarTransform.position, m_Life/(float)m_MaxLife);
    }*/
    private void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        SetIdleState();
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
            Debug.Log("PATROL STATE");
            if (!m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                MoveToNextPatrolPosition();
            if (HearsPlayer())
                SetAlertState();
        }
        void SetAlertState()
        {
            m_State = TState.ALERT;
        }
        void UpdateAlertState()
        {
            // se queda quieto haciendo un barrido visual de 360 grados, si lo ve y no tiene distancia para pegar pasa a chase, si hay rango pasa a atack, si gira y no lo ve pasa a patrol UTILIZAR FUNCION SEESPLAYER
            //transform.rotation *= Quaternion.Euler(0, 120 * Time.deltaTime, 0); ns como hacer que solo gire 360 grados
            Debug.Log("ALERT STATE");   
            m_NavMeshAgent.destination = transform.position;
            if (SeesPlayer())// y ha dado justo una vuelta)
            {
                SetChaseState();
            }
            else
                SetPatrolState();
        }
        void SetAttackState()
        {
            m_State = TState.ATTACK;
        }
        void UpdateAttackState()
        {
            m_CooldownToAttack = 0;
            GameManager.GetGameManager().GetPlayer().RecibirDaño(10);
            SetChaseState();
        }
        void SetChaseState()
        {
            m_State = TState.CHASE;
        }
        void UpdateChaseState()
        {
            Debug.Log("CHASE STATE");
            SetNextChasePossition();
            Vector3 l_PlayerPossition = GameManager.GetGameManager().GetPlayer().transform.position;
            float l_Distance = Vector3.Distance(l_PlayerPossition, transform.position);
            if (l_Distance <= m_MinDistanceToAttack && m_CooldownToAttack >= m_MaxCooldownToAttack)
            {
                SetAttackState();
            }
        }
        public void SetHitState()
        {
            m_State = TState.HIT;
        }
        void UpdateHitState()
        {
            SetAlertState();
        }
        void SetDieState()
        {
            m_State = TState.DIE;
        }
        void UpdateDieState()
        {
            gameObject.SetActive(false);
        }


        void SetNextChasePossition()
        {
            Vector3 l_PlayerPossition = m_Target.transform.position;
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
            if (m_CurrentPatrolPossitionId >= m_PatrolPosition.Count)
                m_CurrentPatrolPossitionId = 0;
        }
        bool SeesPlayer()
        {
            Debug.Log("Sees Player");
            Vector3 l_PlayerPossition = GameManager.GetGameManager().GetPlayer().transform.position;
            Vector3 l_Direction = l_PlayerPossition - transform.position;
            float l_Distance = l_Direction.magnitude;
            //l_Direction.Normalize();
            l_Direction /= l_Distance;
            float l_DotValue = Vector3.Dot(l_Direction, transform.forward);
            if (l_DotValue >= Mathf.Cos(m_SightAngle * 0.5f * Mathf.Deg2Rad))
            {
                Ray l_Ray = new Ray(transform.position + Vector3.up * m_EyesHeight, l_Direction);
                if (!Physics.Raycast(l_Ray, l_Distance, m_SightLayerMask.value))
                    return true;
            }
            return false;
        }
        bool HearsPlayer()
        {
            Debug.Log("Hears Player");
            Vector3 l_PlayerPossition = GameManager.GetGameManager().GetPlayer().transform.position;
            float l_Distance = Vector3.Distance(l_PlayerPossition, transform.position);
            return l_Distance < m_MaxEarDistance;
        }
        public void Hit(int Damage)
        {
            m_Life -= Damage;
            if (m_Life <= 0)
            {
                m_Life = 0;
                SetDieState();
            }
            else
                SetHitState();
        }


    /*
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
    }*/

    // Esto va dentro de Shoot() em playercontroller
    //
    //  if(l_RaycastHit.collider.CompareTag("HitCollider"))
    //      l_RayCastHit.collider.GetCComponent<HitCollider>().Hit();
    //  else
    //  CreateShootParticles(.......)


    //esto va en PlayerController




}

