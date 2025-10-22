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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            SceneManager.LoadSceneAsync("SampleScene");
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadSceneAsync("Terrain");
    }
    public PlayerController GetPlayer()
    {
        return m_Player;
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
