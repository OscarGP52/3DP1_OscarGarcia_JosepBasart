using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    static GameManager m_GameManager;
    public PlayerController m_Player;
    UIsystem m_UI;
    public Transform m_DestroyObjects;
    public Fade m_Fade;

    private void Awake()
    {
        if (m_GameManager != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        m_GameManager = this;
        DontDestroyOnLoad(gameObject);

        if (m_Player == null)
        {
            m_Player = GameObject.FindFirstObjectByType<PlayerController>();
        }
    }
    static public GameManager GetGameManager()
    {
        return m_GameManager;
    }
    public void RestartLevel()
    {
        for (int i = 0; i < m_DestroyObjects.childCount; i++)
            GameObject.Destroy(m_DestroyObjects.GetChild(i).gameObject);
        m_Player.Restart();

        Debug.Log("RestartLevel ejecutado. Player actual: " + m_Player?.name);
        for (int i = 0; i < m_DestroyObjects.childCount; i++)
            GameObject.Destroy(m_DestroyObjects.GetChild(i).gameObject);

        if (m_Player == null)
            Debug.LogError("¡El Player en GameManager es NULL!");
        else
            m_Player.Restart();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            SceneManager.LoadSceneAsync("PlayGround");
        if (Input.GetKeyDown(KeyCode.N))
            SceneManager.LoadSceneAsync("Level1");
        if (Input.GetKeyDown(KeyCode.M))
            SceneManager.LoadSceneAsync("Level2");
    }
    public PlayerController GetPlayer()
    {
        return m_Player;
    }
    public UIsystem GetUI()
    {
        return m_UI;
    }
    public void SetPlayer(PlayerController Player)
    {
        m_Player = Player;
    }
    public void SetUI(UIsystem UI)
    {
        m_UI = UI;
    }
}
