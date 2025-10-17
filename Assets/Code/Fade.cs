using UnityEngine;
using UnityEngine.UI;
public class Fade : MonoBehaviour
{

    public float m_FadeTime = 0.8f;
    float m_CurrentTime = 0.0f;
    bool m_IsFadeIn;
    public delegate void OnfadeEndedFn();
    public Image m_FadeImage;
    OnfadeEndedFn m_OnFadeEndedFn;


    private void Update()
    {
        UpdateFade();
    }

    void UpdateFade()
    {
        m_CurrentTime += Time.deltaTime;
        float l_Pct = Mathf.Min(1.0f, m_CurrentTime / m_FadeTime);
        m_FadeImage.color = new Color(0.0f, 0.0f, 0.0f, m_IsFadeIn ? l_Pct : 1.0f - l_Pct);
        if (l_Pct == 1.0f)
            m_OnFadeEndedFn();    
    }

    public void FadeIn(OnfadeEndedFn _OnFadeEndedFn)
    {
        _Fade(_OnFadeEndedFn, true);
    }

    public void FadeOut(OnfadeEndedFn _OnFadeEndedFn) 
    {
        _Fade(_OnFadeEndedFn, false);
    }

    void _Fade(OnfadeEndedFn _OnFadeEndedFn, bool IsFadeIn)
    {
        m_OnFadeEndedFn = _OnFadeEndedFn;
        m_CurrentTime = 0.0f;
        gameObject.SetActive(true);
        m_IsFadeIn = IsFadeIn;
        UpdateFade();
    }
}
