using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    private DungeonGenerator DungeonGenerator;
    public Transform MovePoint;
    public float MoveSpeed;

    public void Awake()
    {
        MovePoint = transform.GetChild(0);   
        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
        MovePoint.parent = DungeonGenerator.transform;
    }

    protected void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, MovePoint.position, MoveSpeed * Time.deltaTime);
    }

    protected TileType GetTileTypeWithKey(Vector2Int _Vector2)
    {
        DungeonGenerator.Kerker.TryGetValue(_Vector2, out TileType tiletip);
        return tiletip;
    }

    protected bool GetItemTypeWithKey(Vector2Int _Vector2)
	{
        bool TurnOn = false;
        for (int i = 0; i < DungeonGenerator.ItemList.Count; i++)
		{
            if (DungeonGenerator.ItemList[i].transform.position == new Vector3(_Vector2.x, _Vector2.y, 0f))
			{
                TurnOn = true;
			} else
			{
                TurnOn = false;
			}
		}  
        return TurnOn;
	}

    protected Vector2Int GetTileTypeAround(int _xpos, int _ypos)
    {
        Vector2Int vector2Int = new Vector2Int((int)transform.position.x + _xpos, (int)transform.position.y + _ypos);
        if ((GetTileTypeWithKey(vector2Int) == TileType.Floor || GetTileTypeWithKey(vector2Int) == TileType.StartFloor) 
            && GetItemTypeWithKey(vector2Int) == false)
        {
            return new Vector2Int((int)MovePoint.position.x + _xpos, (int)MovePoint.position.y + _ypos);
        } 
        else
        {
            return new Vector2Int((int)MovePoint.position.x, (int)MovePoint.position.y);
        }
    }

    [ContextMenu("Walk Enemies")]
    public void PatrolBehaviour()
    {
        Vector2Int[] WhichSideToMove = new Vector2Int[4];
        WhichSideToMove[0] = GetTileTypeAround(0, 1); //up
        WhichSideToMove[1] = GetTileTypeAround(0, -1); //down
        WhichSideToMove[2] = GetTileTypeAround(1, 0); //right
        WhichSideToMove[3] = GetTileTypeAround(-1, 0); //left

        List<Vector2Int> moveToVec = new List<Vector2Int>();
        for (int i = 0; i < WhichSideToMove.Length; i++)
        {
            if (WhichSideToMove[i] == new Vector2Int((int)MovePoint.position.x, (int)MovePoint.position.y))
            {
                moveToVec.Remove(WhichSideToMove[i]);
            } 
            else
			{
                moveToVec.Add(WhichSideToMove[i]);
			}
        }

        int newDirection = Random.Range(0, moveToVec.Count);
        Vector3 posTile = new Vector3(WhichSideToMove[newDirection].x, WhichSideToMove[newDirection].y, 0f);
        MovePoint.position = posTile;
    }
}