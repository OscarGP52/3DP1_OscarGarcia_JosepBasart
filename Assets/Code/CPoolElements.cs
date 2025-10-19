using UnityEngine;
using System.Collections.Generic;

public class CPoolElements
{
    List<GameObject> m_Elements;
    int m_CurrentElementsId;


    public void Init(int Count, GameObject PrefabElement)
    {
        m_Elements = new List<GameObject>();
        for (int i = 0; i < Count; i++)
        {
            GameObject l_GameObject = GameObject.Instantiate(PrefabElement);
            l_GameObject.SetActive(false);
            m_Elements.Add(l_GameObject);
        }
    }

    public GameObject GetNextElement()
    {

        GameObject l_GameObject = m_Elements[m_CurrentElementsId];
        l_GameObject.SetActive(true);
        ++m_CurrentElementsId;
        if(m_CurrentElementsId>=m_Elements.Count)
            m_CurrentElementsId = 0;
        return l_GameObject;
        

    }
}
