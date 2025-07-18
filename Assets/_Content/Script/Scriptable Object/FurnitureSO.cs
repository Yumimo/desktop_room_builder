using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureSO", menuName = "Scriptable Objects/FurnitureSO")]
public class FurnitureSO : ScriptableObject
{
    public string category;
    public Vector2Int size = new Vector2Int(1, 1);
    public Sprite thumbnail;
    public GameObject prefab;
}
