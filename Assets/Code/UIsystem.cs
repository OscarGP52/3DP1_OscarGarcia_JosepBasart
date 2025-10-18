using UnityEngine;
using TMPro;

public class UIsystem : MonoBehaviour
{
    public BulletBehavior bullet;
    int scorePoints = 0; //prescindible
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        shieldText.text = "Shield: " + GameManager.GetGameManager().GetPlayer().GetCurrentShield().ToString();
        lifeText.text = "Life: " + GameManager.GetGameManager().GetPlayer().maxLife.ToString(); //hardcode
        scoreText.text = "Score: " + scorePoints.ToString();
        UpdateAmmo(bullet.GetBulletsLeft(), bullet.GetMagSize());
    }

    public void UpdateShield(int shield)
    {
        shieldText.text = "Shield: " + shield;
    }
    public void UpdateLife(int life)
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
}


 
