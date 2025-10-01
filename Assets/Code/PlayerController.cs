using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float m_Yaw;
    float m_Pitch;
    public float m_YawSpeed;
    public float m_PitchSpeed;
    public float m_MinPitch;
    public float m_MaxPitch;
    public Transform m_PitchCotroller;
    public bool m_UseInvertedYaw;
    public bool m_UseInvertedPitch;
    public CharacterController m_CharacterController;
    float m_VerticalSpeed = 0.0f;
    private bool m_AngleLocked = false;

    public float m_Speed;
    public float m_JumpSpeed;
    public float m_SpeedMultiplier;


    [Header("Input")]
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;

    [Header("Debug Imput")]
    public KeyCode m_DebugLockAngeleKeyCode = KeyCode.I;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y"); ;

        if (Input.GetKeyDown(m_DebugLockAngeleKeyCode))
            m_AngleLocked = !m_AngleLocked;

        if (!m_AngleLocked)
        {
            m_Yaw = m_Yaw + l_MouseX * m_YawSpeed * Time.deltaTime * (m_UseInvertedYaw ? -1.0f : 1.0f);
            m_Pitch = m_Pitch + l_MouseY * m_PitchSpeed * Time.deltaTime * (m_UseInvertedPitch ? -1.0f : 1.0f);
            m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);
            transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
            m_PitchCotroller.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
        }

        Vector3 l_Movement = Vector3.zero;
        float l_YawPiRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90piRadians = (m_Yaw + 90.0f) * Mathf.Deg2Rad;
        Vector3 l_RightDirection = new Vector3(Mathf.Sin(l_YawPiRadians), 0.0f, Mathf.Cos(l_YawPiRadians));
        Vector3 l_ForwardDirection = new Vector3(Mathf.Sin(l_Yaw90piRadians), 0.0f, Mathf.Cos(l_Yaw90piRadians));


        if (Input.GetKey(m_RightKeyCode))
            l_Movement = l_ForwardDirection;
        else if (Input.GetKey(m_LeftKeyCode))
            l_Movement = -l_ForwardDirection;

        if (Input.GetKey(m_UpKeyCode))
            l_Movement += l_RightDirection;
        else if (Input.GetKey(m_DownKeyCode))
            l_Movement -= l_RightDirection;

        float l_SpeedMultyplier = 1.0f;

        if (Input.GetKey(m_RunKeyCode))
            l_SpeedMultyplier = m_SpeedMultiplier;

        l_Movement.Normalize();
        l_Movement *= m_Speed * l_SpeedMultyplier * Time.deltaTime;

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollissionFlags = m_CharacterController.Move(l_Movement);
        if (m_VerticalSpeed < 0.0f && (l_CollissionFlags & CollisionFlags.Below) != 0)
        {

            m_VerticalSpeed = 0.0f;
            if (Input.GetKeyDown(m_JumpKeyCode))
                m_VerticalSpeed = m_JumpSpeed;
        }

        else if (m_VerticalSpeed > 0.0f && (l_CollissionFlags & CollisionFlags.Above) != 0)
            m_VerticalSpeed = 0.0f;

    }
}
