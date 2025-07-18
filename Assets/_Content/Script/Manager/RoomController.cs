using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private BoxCollider m_dragScaleCollider;

    public Vector3 RoomScale => transform.localScale;
    public void ToggleDragScale(bool toggle)
    {
        m_dragScaleCollider.enabled = !toggle;
        if(PlacementManager.Instance == null) return;
        PlacementManager.Instance.TogglePlacement(!toggle);
    }
    
    
}
