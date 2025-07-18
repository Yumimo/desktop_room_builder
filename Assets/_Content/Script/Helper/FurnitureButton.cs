using UnityEngine;
using UnityEngine.UI;
public class FurnitureButton : MonoBehaviour
{
    [SerializeField] Button m_button;
    [SerializeField] Image m_image;
    
    private FurnitureSO furnitureSO;

    public void SetupButton(FurnitureSO _furnitureSO)
    {
        m_image.sprite = _furnitureSO.thumbnail;
        furnitureSO = _furnitureSO;
        m_button.onClick.AddListener(() =>
        {
            PlacementManager.Instance.SetupFurniture(furnitureSO);
        });
        gameObject.SetActive(true);
    }
}
