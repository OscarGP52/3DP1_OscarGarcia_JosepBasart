using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarElementUI :MonoBehaviour
{
    public RectTransform m_LifeBarUIRectTransform;
    public Image m_ForegraoundLifeBarUI;

    public void Show (Vector3 WoldPosition, float LifePct)
    {
        Vector3 l_LifeBarViewportPosition = GameManager.GetGameManager.GetPlayer().m_Camera.WorldToViewportPoint(WorldPosition);
        if (l_LifeBarViewportPosition.z > 0.0f)
        {
            Vector2 l_PositionUI = new Vector2(l_LifeBarViewportPosition.x * 1920.0f, -(1.0f - l_LifeBarViewportPosition.y) * 1080.0f);
            m_LifeBarUIRectTransform.anchoredPosition = l_PositionUI;
            m_ForegraoundLifeBarUI.fillAmount = LifePct;
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
