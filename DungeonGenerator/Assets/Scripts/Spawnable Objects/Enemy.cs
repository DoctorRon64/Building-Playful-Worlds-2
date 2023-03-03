using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    private DungeonGenerator DungeonGenerator;
    public Transform MovePoint;
    public float MoveSpeed = 5f;

    public void Awake()
    {
        MovePoint = transform.GetChild(0);
        MovePoint.parent = null;
        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
    }

    protected void Update()
    {

    }

    protected TileType GetTileTypeWithKey(Vector2Int _Vector2)
    {
        DungeonGenerator.Kerker.TryGetValue(_Vector2, out TileType tiletip);
        return tiletip;
    }

    protected Vector2Int GetTileTypeAround(int _xpos, int _ypos)
    {
        if (GetTileTypeWithKey(new Vector2Int((int)transform.position.x + _xpos, (int)transform.position.y + _ypos)) == TileType.Floor)
        {
            return new Vector2Int((int)transform.position.x + _xpos, (int)transform.position.y + _ypos);
        } 
        else
        {
            return new Vector2Int(0, 0);
        }
    }

    [ContextMenu("Walk Enemies 10x")]
    protected void RunPatrolAmountOfTimes()
    {
        for (int i = 0; i < 10; i++)
        {
            PatrolBehaviour();
        }
    }


    [ContextMenu("Walk Enemies")]
    protected void PatrolBehaviour()
    {
        Vector2Int[] WhichSideToMove = new Vector2Int[4];
        WhichSideToMove[0] = GetTileTypeAround(0, 1); //up
        WhichSideToMove[1] = GetTileTypeAround(0, -1); //down
        WhichSideToMove[2] = GetTileTypeAround(1, 0); //right
        WhichSideToMove[3] = GetTileTypeAround(-1, 0); //left

        List<Vector2Int> moveToVec = new List<Vector2Int>();
        for (int i = 0; i < WhichSideToMove.Length; i++)
        {
            if (WhichSideToMove[i] != new Vector2Int((int)transform.position.x, (int)transform.position.y))
            {
                moveToVec.Add(WhichSideToMove[i]);
            }
        }

        int newDirection = Random.Range(0, moveToVec.Count);
        MovePoint.position = new Vector3(WhichSideToMove[newDirection].x, WhichSideToMove[newDirection].y, 0f);
        transform.position = Vector2.MoveTowards(transform.position, MovePoint.position, MoveSpeed * Time.deltaTime);
    }
}