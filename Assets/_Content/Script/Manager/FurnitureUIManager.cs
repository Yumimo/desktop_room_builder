using System;
using UnityEngine;

public class FurnitureUIManager : MonoBehaviour
{
    [SerializeField] FurnitureSO[]  m_furnitureSO;
    
    [Header("UI")]
    [SerializeField] private CanvasGroup m_canvasGroup;
    
    [Header("Spawn Furniture")]
    [SerializeField] private FurnitureButton m_furnitureButton;
    [SerializeField] private GameObject m_container;

    private void Start()
    {
        for (int i = 0; i < m_furnitureSO.Length; i++)
        {
            var _button = Instantiate(m_furnitureButton, m_container.transform);
            _button.SetupButton(m_furnitureSO[i]);
        }
    }

    public void HideFurniturePanel()
    {
        m_canvasGroup.interactable = false;
        m_canvasGroup.alpha = 0;
        m_canvasGroup.blocksRaycasts = false;
    }
}
