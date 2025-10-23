using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

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
    Vector3 l_Position;
    Quaternion m_StartRotation;
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

    [Header("Shooting")]
    public float m_shootRange = 100f;
    public int magSize = 10;
    public float timeBetweenShots = 0.5f;
    private int bulletsLeft;
    private int bulletsShot;
    private bool reloading;
    private bool shooting;
    public LayerMask m_hitLayer;
    public Animation m_Animation;
    public AnimationClip m_ReloadAnimationClip;
    public AnimationClip m_ShootAnimationClip;
    public AnimationClip m_IdleAnimationClip;

    void Start()
    {
        GameManager.GetGameManager().SetPlayer(this);

        
        bulletsLeft = magSize;
        magSize = 10;
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

        if (!shooting && !reloading)
        {
            SetIdleAnimation();
        }
        ShootingImput();
        UpdateAmmoDisplay();
    }

    public void ShootingImput() 
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < 10 && magSize > 0 && !reloading)
        {
            Reload();
        }
        if (!reloading && bulletsLeft <= 0)
        {
            Reload();
        }

        if (Input.GetMouseButtonDown(0) && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    public void Shoot()
    {
        SetShootingAnimation();
        Ray ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, m_shootRange, m_hitLayer))
        {
            Debug.Log("Hit object: " + hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject.CompareTag("Diana"))
            {
                GameObject.Find("Prueba UI").GetComponent<UIsystem>().UpdateScore(1);
                hitInfo.collider.gameObject.SetActive(false);
            }
            if (hitInfo.collider.gameObject.CompareTag("EnemyCollider"))
            {
                hitInfo.collider.gameObject.GetComponentInParent<EnemyController>().Hit(10);
            }
        }
        bulletsLeft--;
        bulletsShot++;
    }

    public void Reload()
    {
        SetReloadingAnimation();
        reloading = true;
        Invoke("ReloadFinished", m_ReloadAnimationClip.length);
    }
    private void ReloadFinished()
    {
        int bulletsToLoad = 10 - bulletsLeft;
        magSize -= bulletsToLoad;
        if (magSize < 0)
        {
            bulletsLeft += bulletsToLoad + magSize;
            magSize = 0;
            reloading = false;
        }
        else
        {
            bulletsLeft += bulletsToLoad;
            reloading = false;
        }
    }
    public void SetReloadingAnimation()
    {
        m_Animation.Stop();
        m_Animation.CrossFade(m_ReloadAnimationClip.name, 0.1f);
    }
    public void SetShootingAnimation()
    {
        m_Animation.Stop();
        m_Animation.CrossFade(m_ShootAnimationClip.name, 0.19f);
    }
    public void SetIdleAnimation()
    {
        if (!m_Animation.IsPlaying(m_IdleAnimationClip.name))
        {
            m_Animation.CrossFade(m_IdleAnimationClip.name, 0.19f);
        }
    }
    public void addAmmo(int ammo)
    {
        magSize += ammo;
        UpdateAmmoDisplay();
    }
    public void UpdateAmmoDisplay()
    {
        GameObject.Find("Prueba UI").GetComponent<UIsystem>().UpdateAmmo(bulletsLeft, magSize);
    }

    public float GetCurrentShield()
    {
        return currentShield;
    }

    public float GetCurrentLife()
    {
        return currentLife;
    }
    public int GetBulletsLeft()
    {
        return bulletsLeft;
    }

    public int GetMagSize()
    {
        return magSize;
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
        transform.rotation = m_StartRotation;
        currentLife = maxLife;
        currentShield = 0;
        Debug.Log("Reseteo");
    }

    public void SetCheckPoint(Vector3 NewPosition)
    {
        m_StartPosition = NewPosition;
        Debug.Log("SpawnSet");
    }
}
