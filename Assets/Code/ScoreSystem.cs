using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    
    public int puntuacion = 0;
    public TextMeshProUGUI puntuacionText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIupdate();
    }

    public void aumentarPuntuacion(int puntos)
    {
        puntuacion += puntos;
        UIupdate();
    }

    void UIupdate()
    {
        puntuacionText.text = "Score: " + puntuacion.ToString();
    }
}
