using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class Player : MonoBehaviour
{
    public Dictionary<Vector2Int, TileType> Kerker;

    public void Generator(DungeonGenerator _DungeonGenerator)
    {
        Kerker = _DungeonGenerator.Kerker;
    }
}