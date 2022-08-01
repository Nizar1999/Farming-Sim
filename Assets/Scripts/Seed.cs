using UnityEngine;

public class Seed : MonoBehaviour
{
    public GameObject plant;

    public GameObject useSeed(Vector3 tilePos)
    {
        return Instantiate(plant, tilePos, Quaternion.identity);
    }
}
