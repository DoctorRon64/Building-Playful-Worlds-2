using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class Player : MonoBehaviour
{
    public DungeonGenerator Generator;
    public Dictionary<Vector2Int, TileType> Kerker = new Dictionary<Vector2Int, TileType>();

    private void Awake()
    {
        Generator = GetComponent<DungeonGenerator>();
        Kerker = Generator.Kerker;
    }

    public void CanPlayerMove()
    {
        foreach (KeyValuePair<Vector2Int, TileType> keyvalue in Kerker)
        {
            
        }
    }
}