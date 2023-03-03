using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    private DungeonGenerator DungeonGenerator;
    private Transform PlayerObj;
    public Transform MovePoint;
    private enum StateEnum { Patrol , Attack };
    private StateEnum State;

    public void Awake()
    {
        MovePoint = transform.GetChild(0);
        PlayerObj = FindObjectOfType<Player>().transform;
        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
    }

    protected void Update()
    {
        switch (State)
        {
            case StateEnum.Patrol: PatrolState(); break;
            case StateEnum.Attack: AttackState(); break;
        }
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
        return new Vector2Int(0, 0);
    }

    protected void PatrolState()
    {
        foreach (KeyValuePair<Vector2Int, TileType> keyvalue in DungeonGenerator.Kerker)
        {
            Vector2Int[] WhichSideToMove = new Vector2Int[4];
            WhichSideToMove[0] = GetTileTypeAround(0, 1); //up
            WhichSideToMove[1] = GetTileTypeAround(0, -1); //down
            WhichSideToMove[2] = GetTileTypeAround(1, 0); //right
            WhichSideToMove[3] = GetTileTypeAround(-1, 0); //left

            int newDirection = Random.Range(0, WhichSideToMove.Length);
            MovePoint.position = new Vector3(WhichSideToMove[newDirection].x, WhichSideToMove[newDirection].y, 0f);
        }

        if (Vector2.Distance(transform.position, PlayerObj.position) < 10)
        {
            State = StateEnum.Attack;
        }
    }

    protected void AttackState()
    {
        foreach (KeyValuePair<Vector2Int, TileType> keyvalue in DungeonGenerator.Kerker)
        {
            //loop NAAR SPELER
        }

        if (Vector2.Distance(transform.position, PlayerObj.position) > 10)
        {
            State = StateEnum.Patrol;
        }
    }

}