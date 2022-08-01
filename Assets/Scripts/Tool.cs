using UnityEngine;
using UnityEngine.Tilemaps;

public interface toolUse
{
    abstract void use(Tilemap tm, Vector3Int tilePos, Tile result);
}
public class Tool : MonoBehaviour
{
    public enum toolType {Hoe, Watering};
    public toolType tool;

    [SerializeField] toolUse toolInstance;
    [SerializeField] Tile toolResult;

    private void Start()
    {
        init();
    }
    
    public void init()
    {
        if (toolInstance == null)
        {
            if (tool == toolType.Hoe)
            {
                toolInstance = new Hoe();
            }

            if (tool == toolType.Watering)
            {
                toolInstance = new Watering();
            }
        }
    }

    public void onUse(Tilemap tm, Vector3Int tilePos)
    {
        toolInstance.use(tm, tilePos, toolResult);
    }
}
