using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;
using UnityEngine.Timeline;

public class Enemy : MonoBehaviour
{
    private DungeonGenerator DungeonGenerator;
    public Vector2 MovePoint;
    public float MoveSpeed;
    private Player PlayerObj;
    private Vector2Int[] WhichSideToMove = new Vector2Int[5];
    public DungeonData DungeonDatas;

    public int Health;
    public int AttackDamage;

    public void Awake()
    {
        WhichSideToMove[0] = new Vector2Int(0, 1); //up
        WhichSideToMove[1] = new Vector2Int(0, -1); //down
        WhichSideToMove[2] = new Vector2Int(1, 0); //right
        WhichSideToMove[3] = new Vector2Int(-1, 0); //left
        WhichSideToMove[4] = new Vector2Int(0, 0);
        
        Health = Random.Range(4, 8);
        AttackDamage = Random.Range(1, 7);

        MovePoint = transform.position;
        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
        PlayerObj = FindObjectOfType<Player>();
    }

    protected void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, MovePoint, MoveSpeed * Time.deltaTime);
    }

    protected TileType GetTileTypeWithKey(Vector2Int _Vector2)
    {
        DungeonGenerator.Dungeon.TryGetValue(_Vector2, out TileType tiletip);
        return tiletip;
    }

    protected bool GetObjectTypeWithKey(Vector2Int _Vector2, List<Item> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].transform.position == new Vector3(_Vector2.x, _Vector2.y, 0f))
            {
                return true;
            }
        }
        return false;
    }

    protected bool GetObjectTypeWithKey(Vector2Int _Vector2, List<Enemy> _list)
    {
        Vector3 checkDir = new Vector3(_Vector2.x, _Vector2.y, 0f);
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i].transform.position == checkDir || _list[i].MovePoint == (Vector2)checkDir)
            {
                return true;
            }
        }
        return false;
    }

    protected Vector2Int GetTileTypeAround(int _xpos, int _ypos)
    {
        Vector2Int vector2Int = new Vector2Int((int)MovePoint.x + _xpos, (int)MovePoint.y + _ypos);
        return vector2Int;
    }

    protected Vector2Int GetTileTypeAround(Vector2Int direction)
    {
        Vector2Int vector2Int = new Vector2Int((int)MovePoint.x + direction.x, (int)MovePoint.y + direction.y);
        return vector2Int;
    }
    protected bool isFloorTile(Vector2Int _vector2)
    {
        return GetTileTypeWithKey(_vector2) == TileType.Floor || GetTileTypeWithKey(_vector2) == TileType.StartFloor;
    }

    protected bool IsPlayerInHood()
	{
        return Vector2.Distance(transform.position, PlayerObj.transform.position) < 20f;
    }

    public void PatrolBehaviour()
    {
        int check = 0;
        if (IsPlayerInHood())
		{
            int newDir = EnemyFindPlayerBehaviour();
           
            while (!isFloorTile(GetTileTypeAround(WhichSideToMove[newDir])) || 
                GetObjectTypeWithKey(GetTileTypeAround(WhichSideToMove[newDir]), DungeonDatas.ItemList) == true || 
                GetObjectTypeWithKey(GetTileTypeAround(WhichSideToMove[newDir]), DungeonDatas.EnemyList) == true)
            {
                newDir = Random.Range(0, WhichSideToMove.Length - 1);
                
                check++;
                if (check >= 4)
                {
                    newDir = 4;
                    break;
                }
            }
            
            Vector3 posTile = new Vector3(GetTileTypeAround(WhichSideToMove[newDir]).x, GetTileTypeAround(WhichSideToMove[newDir]).y, 0f);
            MovePoint = posTile;
        }
        else
		{
            int newDir = Random.Range(0, WhichSideToMove.Length - 1);

            while (!isFloorTile(GetTileTypeAround(WhichSideToMove[newDir])) || 
                GetObjectTypeWithKey(GetTileTypeAround(WhichSideToMove[newDir]), DungeonDatas.ItemList) == true || 
                GetObjectTypeWithKey(GetTileTypeAround(WhichSideToMove[newDir]), DungeonDatas.EnemyList) == true)
            {
                newDir = Random.Range(0, WhichSideToMove.Length - 1);

                check++;
                if (check >= 4)
                {
                    newDir = 4;
                    break;
                }
            }

            Vector3 posTile = new Vector3(GetTileTypeAround(WhichSideToMove[newDir]).x, GetTileTypeAround(WhichSideToMove[newDir]).y, 0f);
            MovePoint = posTile;
        }
	}

    private void DoPlayerDamage()
    {
        for (int i = 0; i < WhichSideToMove.Length; i++)
        {
            if (WhichSideToMove[i] == new Vector2Int((int)PlayerObj.transform.position.x, (int)PlayerObj.transform.position.y))
            {
                PlayerObj.TakeDamage(AttackDamage);
            }
        }
    }

    public void TakeDamage(int _Damage)
    {
        Health -= _Damage;
    }

    public void ApplyHealth(int _Regeneration)
    {
        Health += _Regeneration;
    }

    public void CheckIfEnemyDies()
	{
        if (Health <= 0)
		{
            foreach (Enemy enemy in DungeonDatas.EnemyList)
			{
                if (enemy == this)
				{
                    DungeonDatas.EnemyList.Remove(enemy);
                    gameObject.SetActive(false);
                }
            }

		}
	}

    public int EnemyFindPlayerBehaviour()
	{
        float dX = PlayerObj.transform.position.x - transform.position.x;
        float dY = PlayerObj.transform.position.y - transform.position.y;

        float absX = Mathf.Abs(dX);
        float absY = Mathf.Abs(dY);

        if (absX > absY)
		{
            if (dX > 0) { return 2; } // left
            else { return 3; } //right
		} 
        else
		{
            if (dY > 0) { return 0; } // up
            else { return 1; } //down
        }
    }
}