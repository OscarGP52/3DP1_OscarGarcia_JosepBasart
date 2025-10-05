using UnityEngine;
using TMPro;

public class UIsystem : MonoBehaviour
{
    
    BulletBehavior bullet; //prescindible
    TextMeshProUGUI shieldText;
    TextMeshProUGUI lifeText;
    TextMeshProUGUI ammoText;
    TextMeshProUGUI scoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shieldText.text = "Shield: " + GameObject.Find("Player").GetComponent<PlayerController>().GetCurrentShield().ToString();
        lifeText.text = "Life: " + GameObject.Find("Player").GetComponent<PlayerController>().GetCurrentLife().ToString();
        //scoreText.text = "Score: " + score.ToString();
        //ammoText.text = "bullets / magazine"
        bullet.UpdateAmmoDisplay();
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
        scoreText.text = "Score: " + score;
    }
    public void UpdateAmmo(int currentAmmo, int magazineSize)
    {
        ammoText.text = currentAmmo + " / " + magazineSize;
    }
}


 
