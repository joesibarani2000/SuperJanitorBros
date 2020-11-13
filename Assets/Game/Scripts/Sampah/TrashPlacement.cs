using UnityEngine;

public class TrashPlacement : MonoBehaviour
{
    [SerializeField] private TrashData.TrashType type;

    public void Placed(TrashBehaviour dataTransfer)
    {
        if (type == dataTransfer.GetTrashType())
        {
            GameData.instance.AddScore(dataTransfer.GetData().TrashScore);
            dataTransfer.transform.position = transform.position;
            // give vfx here
        } 
    }

    public void UnPlaced(TrashBehaviour dataTransfer)
    {
        if (type == dataTransfer.GetTrashType())
        {
            GameData.instance.AddScore(-dataTransfer.GetData().TrashScore);
            // give vfx here
        }
    }
}
