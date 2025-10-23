using UnityEngine;
using TMPro;

public class UIsystem : MonoBehaviour
{
    int scorePoints = 0; //prescindible
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        shieldText.text = "Shield: " + GameManager.GetGameManager().GetPlayer().GetCurrentShield().ToString();
        lifeText.text = "Life: " + GameManager.GetGameManager().GetPlayer().maxLife.ToString(); //hardcode
        UpdateAmmo(GameManager.GetGameManager().GetPlayer().GetBulletsLeft(), GameManager.GetGameManager().GetPlayer().GetMagSize());
    }

    public void UpdateShield(float shield)
    {
        shieldText.text = "Shield: " + shield;
    }
    public void UpdateLife(float life)
    {
        lifeText.text = "Life: " + life;
    }
    public void UpdateScore(int score)
    {
        scorePoints += score;
        scoreText.text = "Score: " + scorePoints;
    }
    public void UpdateAmmo(int currentAmmo, int magazineSize)
    {
        ammoText.text = currentAmmo + " / " + magazineSize;
    }
    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    public void PhaseCompleted()
    {
        timerText.text = "Congratulations";
    }
    public void StopTimer()
    {
        timerText.text = "Out of Time";
    }
    public void EraseTimer()
    {
        timerText.text = "";
    }
    public void EraseScore()
    {
        scorePoints = 0;
        scoreText.text = "";
    }
}


 
