using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Lists", menuName = "Lists")]
public class DungeonData : ScriptableObject
{
    public List<GameObject> ItemList = new List<GameObject>();
    public List<GameObject> EnemyList = new List<GameObject>();
    
}
