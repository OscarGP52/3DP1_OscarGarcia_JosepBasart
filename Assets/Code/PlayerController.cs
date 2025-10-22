using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    float m_Yaw;
    float m_Pitch;
    public float m_YawSpeed;
    public float m_PitchSpeed;
    public float m_MinPitch;
    public float m_MaxPitch;
    public Transform m_PitchController;
    public bool m_UseInvertedYaw;
    public bool m_UseInvertedPitch;
    public CharacterController m_CharacterController;
    float m_VerticalSpeed=0.0f;
    Vector3 m_StartPosition;
    Quaternion m_startRotation;
    public Camera m_Camera;


    bool m_AngleLocked=false;
    public float m_Speed;
    public float m_JumpSpeed;
    public float m_SpeedMultiplier;

    [Header("Input")]
    public KeyCode m_LeftKeycode=KeyCode.A;
    public KeyCode m_RightKeycode=KeyCode.D;
    public KeyCode m_UpKeycode=KeyCode.W;
    public KeyCode m_DownKeycode=KeyCode.S;
    public KeyCode m_JumpKeycode=KeyCode.Space;
    public KeyCode m_RunKeycode=KeyCode.LeftShift;
    public KeyCode m_GetDamage = KeyCode.K;

    [Header("Debug Input")]
    public KeyCode m_DebugLockAngleKeyCode=KeyCode.I;

    [Header("PlayerStats")]
    public float maxLife = 100f;
    float currentLife;
    public float maxShield = 100;
    float currentShield;

    void Start()
    {
        /*esto va dentro de start() en un if de playercontroller
        l_Player.m_StartRotation = Transform.rotation;
        l_Player.m_StartPosition = Transform.position;*/
        Cursor.lockState=CursorLockMode.Locked;
        currentLife = maxLife;
        currentShield = 0;
    }
    void Update()
    {
        float l_MouseX=Input.GetAxis("Mouse X");
        float l_MouseY=Input.GetAxis("Mouse Y");

        if(Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked=!m_AngleLocked;

        if(!m_AngleLocked)
        {
            m_Yaw=m_Yaw+l_MouseX*m_YawSpeed*Time.deltaTime*(m_UseInvertedYaw ? -1.0f : 1.0f);
            m_Pitch=m_Pitch+l_MouseY*m_PitchSpeed*Time.deltaTime*(m_UseInvertedPitch ? -1.0f : 1.0f);
            m_Pitch=Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);
            transform.rotation=Quaternion.Euler(0.0f, m_Yaw, 0.0f);
            m_PitchController.localRotation=Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
        }
        
        Vector3 l_Movement=Vector3.zero;
        float l_YawPiRadians=m_Yaw*Mathf.Deg2Rad;
        float l_Yaw90PiRadians=(m_Yaw+90.0f)*Mathf.Deg2Rad;
        Vector3 l_ForwardDirection=new Vector3(Mathf.Sin(l_YawPiRadians), 0.0f, Mathf.Cos(l_YawPiRadians));
        Vector3 l_RightDirection=new Vector3(Mathf.Sin(l_Yaw90PiRadians), 0.0f, Mathf.Cos(l_Yaw90PiRadians));

        if(Input.GetKey(m_RightKeycode))
            l_Movement=l_RightDirection;
		else if(Input.GetKey(m_LeftKeycode))
            l_Movement=-l_RightDirection;

        if(Input.GetKey(m_UpKeycode))
            l_Movement+=l_ForwardDirection;
		else if(Input.GetKey(m_DownKeycode))
            l_Movement-=l_ForwardDirection;

        float l_SpeedMultiplier=1.0f;

        if(Input.GetKey(m_RunKeycode))
            l_SpeedMultiplier=m_SpeedMultiplier;

        l_Movement.Normalize();
        l_Movement*=m_Speed*l_SpeedMultiplier*Time.deltaTime;
        
        m_VerticalSpeed=m_VerticalSpeed+Physics.gravity.y*Time.deltaTime;
        l_Movement.y=m_VerticalSpeed*Time.deltaTime;
        
		CollisionFlags l_CollisionFlags=m_CharacterController.Move(l_Movement);
        if(m_VerticalSpeed<0.0f && (l_CollisionFlags & CollisionFlags.Below)!=0) //si estoy cayendo y colisiono con el suelo
        {
            m_VerticalSpeed=0.0f;
            if(Input.GetKeyDown(m_JumpKeycode))
                m_VerticalSpeed=m_JumpSpeed;
        }
        else if(m_VerticalSpeed>0.0f && (l_CollisionFlags & CollisionFlags.Above)!=0) //si estoy subiendo y colision con un techo
            m_VerticalSpeed=0.0f;

        //Prueva de daño
        if (Input.GetKeyDown(m_GetDamage))
        {
            RecibirDaño(10);
        }

    }

    public float GetCurrentShield()
    {
        return currentShield;
    }

    public float GetCurrentLife()
    {
        return currentLife;
    }

    public void RecibirEscudo(int cantidad)
    {
        currentShield += cantidad;
        if (currentShield > maxShield)
            currentShield = maxShield;
        GameObject.Find("Prueba UI").GetComponent<UIsystem>().UpdateShield(currentShield);
    }
    public void Curar(int cantidad)
    {
        currentLife += cantidad;
        if (currentLife > maxLife)
            currentLife = maxLife;
        GameObject.Find("Prueba UI").GetComponent<UIsystem>().UpdateLife(currentLife);
    }
    public void RecibirDaño(int cantidad)
    {
        if (currentShield > 0)
        {
            currentShield -= (cantidad * 0.75f);
            currentLife -= (cantidad * 0.25f);
            if (currentShield < 0)
                currentShield = 0;
            GameObject.Find("Prueba UI").GetComponent<UIsystem>().UpdateShield(currentShield);
            GameObject.Find("Prueba UI").GetComponent<UIsystem>().UpdateLife(currentLife);
        }
        else
        {
            currentLife -= cantidad;
            if (currentLife < 0)
            {
                currentLife = 0;
                Die();
                //aqui iria la muerte del jugador que lo haremos desde el GameManager
            }
            GameObject.Find("Prueba UI").GetComponent<UIsystem>().UpdateLife(currentLife);
        }
        
    }

    void Die()
    {
        GameManager.GetGameManager().m_Fade.FadeIn(() => {
            GameManager.GetGameManager().RestartLevel(); });
    }

    public void Restart()
    {
        transform.position = m_StartPosition;
        transform.rotation = m_startRotation;
        currentLife = maxLife;
        currentShield = 0;
    }

    public void SetCheckPoint(Vector3 NewPosition)
    {
        m_StartPosition = NewPosition;
    }
}
