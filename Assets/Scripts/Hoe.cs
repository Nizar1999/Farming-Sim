using UnityEngine;
using UnityEngine.Tilemaps;

public class Hoe : toolUse
{    
    void toolUse.use(Tilemap tm, Vector3Int tilePos, Tile hoeResult)
    {
        tm.SetTile(tilePos, hoeResult);
    }
}
