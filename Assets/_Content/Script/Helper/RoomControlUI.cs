using UnityEngine;
using UnityEngine.UI;
public class RoomControlUI : MonoBehaviour
{
    [Header("Lock Position and Scale")]
    [SerializeField] private Button m_lockButton;
    [SerializeField] private Image m_lockImage;
    [SerializeField] private Sprite m_lockSprite;
    [SerializeField] private Sprite m_unlockSprite;

    [Header("Open Furniture Panel")]
    [SerializeField] private Button m_openFurnitureButton;
    [SerializeField] private CanvasGroup m_openFurniturePanel;

    private bool isLocked;
    private bool isFurnitureOpen = false;

    private void Start()
    {
        m_lockButton.onClick.AddListener(OnLock);
        m_openFurnitureButton.onClick.AddListener(OnOpenFurniture);
    }

    private void OnOpenFurniture()
    {
        isFurnitureOpen = !isFurnitureOpen;

        m_openFurniturePanel.interactable = isFurnitureOpen;
        m_openFurniturePanel.alpha = isFurnitureOpen ? 1 : 0;
        m_openFurniturePanel.blocksRaycasts = isFurnitureOpen;

        // Force lock if opening panel
        if (isFurnitureOpen && !isLocked)
        {
            SetLock(true);  // No recursion risk
        }
    }

    private void OnLock()
    {
        // Prevent unlocking if furniture is open
        if (isFurnitureOpen && isLocked)
        {
            Debug.Log("Can't unlock while furniture panel is open.");
            return;
        }

        SetLock(!isLocked);  // Toggle
    }
    private void SetLock(bool shouldLock)
    {
        if (isLocked == shouldLock) return;

        isLocked = shouldLock;
        m_lockImage.sprite = isLocked ? m_lockSprite : m_unlockSprite;

        var _room = FindAnyObjectByType<RoomController>();
        if (_room != null)
        {
            _room.ToggleDragScale(isLocked);
        }
    }
}
