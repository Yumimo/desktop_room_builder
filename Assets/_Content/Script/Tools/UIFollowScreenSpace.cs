using UnityEngine;

public class UIFollowScreenSpace : MonoBehaviour
{
    [SerializeField] private Transform m_lookAt;
    [SerializeField] private Vector3 m_offset;
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(camera == null)return;
        var _pos = camera.WorldToScreenPoint(m_lookAt.position + m_offset);

        if (transform.position != _pos)
        {
            transform.position = _pos;
        }
    }
}
