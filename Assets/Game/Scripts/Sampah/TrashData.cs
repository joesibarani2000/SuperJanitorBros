using UnityEngine;

public class TrashData : MonoBehaviour
{
    public enum TrashType
    {
        BRING, 
        PUSH,
        SWIPE
    }

    [SerializeField] private string trashName;
    [SerializeField] private TrashType type;
    [SerializeField] private Vector2 rectTrash;
    [SerializeField] private int trashScore;
    [SerializeField] private LayerMask layerMask;

    public string TrashName { get => trashName; }
    public TrashType Type { get => type; }
    public Vector2 RectTrash { get => rectTrash; }
    public int TrashScore { get => trashScore; }
    public int LayerMask { get => layerMask; set => layerMask = value; }
}
