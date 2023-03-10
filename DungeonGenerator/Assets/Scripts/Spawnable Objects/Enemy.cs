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
    private Player PlayerObj;
    private Vector2Int whichSideToMove;
    private Vector2Int noSideToMove = new Vector2Int(0, 0);
    public DungeonData DungeonData;

    public void Awake()
    {
        
        MovePoint = transform.GetChild(0);
        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
        PlayerObj = FindObjectOfType<Player>();
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

    protected bool GetObjectTypeWithKey(Vector2Int _Vector2, List<GameObject> _list)
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

/*    protected Vector2Int GetTileTypeAround(int _xpos, int _ypos)
    {
        Vector2Int vector2Int = new Vector2Int((int)MovePoint.position.x + _xpos, (int)MovePoint.position.y + _ypos);
        return vector2Int;
    }*/
    protected Vector2Int GetTileTypeAround(Vector2Int direction)
    {
        Vector2Int vector2Int = new Vector2Int((int)MovePoint.position.x + direction.x, (int)MovePoint.position.y + direction.y);
        Debug.Log(vector2Int);
        return vector2Int;
    }

    protected void NEWRANNDOM()
    {
        whichSideToMove = new Vector2Int(Random.Range(-1, 0), Random.Range(-1, 0));
    }

    protected bool isFloorTile(Vector2Int _vector2)
    {
        return GetTileTypeWithKey(_vector2) == TileType.Floor;
    }

    protected bool IsPlayerInHood()
	{
        return Vector2.Distance(MovePoint.position, PlayerObj.transform.position) < 20f;
    }

    public void PatrolBehaviour()
    {
        int checkNoSideToMove = 0;
        if (IsPlayerInHood())
		{
            int newDir = EnemyFindPlayerBehaviour(); 
            

            while (!isFloorTile(GetTileTypeAround(whichSideToMove)) || 
                GetObjectTypeWithKey(GetTileTypeAround(whichSideToMove), DungeonData.EnemyList) == true || 
                GetObjectTypeWithKey(GetTileTypeAround(whichSideToMove), DungeonData.ItemList) == true)
            {
                NEWRANNDOM();
                checkNoSideToMove++;

                if (checkNoSideToMove == 4)
                {
                    break;
                }
            }
            
            Vector3 posTile = new Vector3(GetTileTypeAround(whichSideToMove).x, GetTileTypeAround(whichSideToMove).y, 0f);
            MovePoint.position = posTile;
        }
        else
		{
            NEWRANNDOM();


            while (!isFloorTile(GetTileTypeAround(whichSideToMove)) || 
                GetObjectTypeWithKey(GetTileTypeAround(whichSideToMove), DungeonData.EnemyList) == true || 
                GetObjectTypeWithKey(GetTileTypeAround(whichSideToMove]), DungeonData.ItemList) == true)
            {
                NEWRANNDOM();
                checkNoSideToMove++;
                if (checkNoSideToMove == 4)
                {
                    break;
                }
            }

            Vector3 posTile;
            if (checkNoSideToMove == 4)
            {
                posTile = new Vector3(GetTileTypeAround(noSideToMove).x, GetTileTypeAround(noSideToMove).y, 0f);
            } else
            {
                posTile = new Vector3(GetTileTypeAround(whichSideToMove).x, GetTileTypeAround(whichSideToMove).y, 0f);
            }

            MovePoint.position = posTile;
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