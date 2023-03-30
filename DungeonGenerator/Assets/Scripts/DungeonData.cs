using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData", menuName = "DungeonData/list")]
public class DungeonData : ScriptableObject
{
    public List<Item> ItemList = new List<Item>();
    public List<Enemy> EnemyList = new List<Enemy>();
    public Player Player;
}
