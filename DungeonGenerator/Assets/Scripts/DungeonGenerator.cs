using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class DungeonGenerator : MonoBehaviour
{
    public enum TileType { Floor , StartFloor, Wall }

    public GameObject WallObject;
    public GameObject FloorObject;
    public GameObject StartFloorObject;

    public GameObject Player;
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> Items = new List<GameObject>();

    public int GridWidth = 100;
    public int GridHeight = 100;

    public int minRoomSize = 3;
    public int MaxRoomSize = 7;

    public int MaxObjectInRoom = 3;
    public int MinObjectInRoom = 0;

    private Vector3 posRandomInRoom;
    public int numRoom = 10;

    public Dictionary<Vector2Int, TileType> Dungeon = new Dictionary<Vector2Int, TileType>();
    public List<Room> RoomList = new List<Room>();
    public List<Room> EnemiesCanSpawnRoomList = new List<Room>();
    public List<GameObject> EveryInstantiatedPrefab = new List<GameObject>();

    public DungeonData DungeonData;
    public SetCamera SetCameraFollow;
    private void Awake()
    {
        Generate();
    }

    [ContextMenu("DungeonMaker")]
    public void Generate()
    {
        ClearDungeon();
        MakeStartRoom();
        AllLocateRooms();
        ConnectRooms();
        AllLocateWalls();

        for (int i = 0; i < Enemies.Count; i++)
		{
            SpwanRandomObjectInRoom(Enemies[i], DungeonData.EnemyList);
		}
        for (int i = 0; i < Items.Count; i++)
        {
            SpwanRandomObjectInRoom(Items[i], DungeonData.ItemList);
        }

        GenerateDungeon();
        SetCameraFollow.GetPlayerCam();
    }

    [ContextMenu("Clear Dungeon")]
    public void ClearDungeon()
    {
        for (int i = EveryInstantiatedPrefab.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(EveryInstantiatedPrefab[i]);
        }

        Dungeon.Clear();
        RoomList.Clear();
        DungeonData.EnemyList.Clear();
        DungeonData.ItemList.Clear();
        EveryInstantiatedPrefab.Clear();
    }

    private void ConnectRooms()
    {
        for (int i = 0; i < RoomList.Count; i++)
        {
            Room room = RoomList[i];
            Room otherRoom = RoomList[(i + Random.Range(1, RoomList.Count)) % RoomList.Count];
            ConnectKamers(room, otherRoom);
        }
    }

    private void MakeStartRoom()
    {
        int minX = Random.Range(0, GridWidth);
        int maxX = minX + Random.Range(minRoomSize, MaxRoomSize + 1);
        int minY = Random.Range(0, GridHeight);
        int maxY = minY + Random.Range(minRoomSize, MaxRoomSize + 1);

        Room Room = new Room(minX, maxX, minY, maxY);
        PlaceRoomInsideDungeon(Room, TileType.StartFloor, false);

        for (int j = 0; j < RoomList.Count; j++)
        {
            posRandomInRoom = new Vector3(RoomList[j].GetRandomPositionInRoom().x, RoomList[j].GetRandomPositionInRoom().y, 0);
            GameObject instanceObj = Instantiate(Player, posRandomInRoom, Quaternion.identity);
            instanceObj.transform.parent = gameObject.transform;
            EveryInstantiatedPrefab.Add(instanceObj);
        }

        //RoomList.RemoveAt(0);
    }

    private void AllLocateRooms()
    {
        for (int i = 0; i < numRoom; i++)
        {
            int minX = Random.Range(0, GridWidth);
            int maxX = minX + Random.Range(minRoomSize, MaxRoomSize + 1);
            int minY = Random.Range(0, GridHeight);
            int maxY = minY + Random.Range(minRoomSize, MaxRoomSize + 1);

            Room kamer = new Room(minX, maxX, minY, maxY);

            if (CanRoomFitInsideDungeon(kamer))
            {
                PlaceRoomInsideDungeon(kamer, TileType.Floor, true);
            } 
            else
            {
                i--;
            }
        }
    }
    private void AllLocateWalls()
    {
        var keys = Dungeon.Keys.ToList();
        foreach (var kv in keys)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2Int newPos = kv + new Vector2Int(x, y);
                    if (Dungeon.ContainsKey(newPos)) { continue; }
                    Dungeon.Add(newPos, TileType.Wall);
                }
            }
        }
    }

    private void GenerateDungeon()
    {
        foreach (KeyValuePair<Vector2Int, TileType> keyvalue in Dungeon)
        {
            Vector3 posTile = new Vector3(keyvalue.Key.x, keyvalue.Key.y, 0);
            GameObject obj = null;
            switch (keyvalue.Value)
            {
                case TileType.Floor: obj = Instantiate(FloorObject, posTile, Quaternion.identity, transform); break;
                case TileType.StartFloor: obj = Instantiate(StartFloorObject, posTile, Quaternion.identity, transform); break;
                case TileType.Wall: obj = Instantiate(WallObject, posTile, Quaternion.identity, transform); break;
            }
            EveryInstantiatedPrefab.Add(obj);
        }
    }

    private void SpwanRandomObjectInRoom(GameObject _obj, List<Item> _list)
    {
        for (int j = 0; j < EnemiesCanSpawnRoomList.Count; j++)
        {
            int ObjectAmount = Random.Range(MinObjectInRoom, MaxObjectInRoom);

            bool samePos = false;
            for (int i = 0; i < ObjectAmount; i++)
            {
                posRandomInRoom = new Vector3(EnemiesCanSpawnRoomList[j].GetRandomPositionInRoom().x, EnemiesCanSpawnRoomList[j].GetRandomPositionInRoom().y, 0);
                foreach (Item _lookobjectinlist in _list)
                {
                    if (posRandomInRoom == _lookobjectinlist.transform.position)
                    {
                        samePos = true;
                        break;
                    }
                }
                if (!samePos)
                {
                    GameObject instanceObj = Instantiate(_obj, posRandomInRoom, Quaternion.identity, gameObject.transform);
                    _list.Add(instanceObj.GetComponent<Item>());
                    EveryInstantiatedPrefab.Add(instanceObj);
                }
            }
        }
    }

    private void SpwanRandomObjectInRoom(GameObject _obj, List<Enemy> _list)
    {
        for (int j = 0; j < EnemiesCanSpawnRoomList.Count; j++)
        {
            int ObjectAmount = Random.Range(MinObjectInRoom, MaxObjectInRoom);

            bool samePos = false;
            for (int i = 0; i < ObjectAmount; i++)
            {
                posRandomInRoom = new Vector3(EnemiesCanSpawnRoomList[j].GetRandomPositionInRoom().x, EnemiesCanSpawnRoomList[j].GetRandomPositionInRoom().y, 0);
                foreach (Enemy _lookobjectinlist in _list)
                {
                    if (posRandomInRoom == _lookobjectinlist.transform.position)
                    {
                        samePos = true;
                        break;
                    }
                }
                if (!samePos)
                {
                    GameObject instanceObj = Instantiate(_obj, posRandomInRoom, Quaternion.identity, gameObject.transform);
                    _list.Add(instanceObj.GetComponent<Enemy>());
                    EveryInstantiatedPrefab.Add(instanceObj);
                }
            }
        }
    }

    private void ConnectKamers(Room _Room1, Room _Room2)
    {
        Vector2Int posOne = _Room1.GetCenter();
        Vector2Int posTwo = _Room2.GetCenter();
        int dirX = posTwo.x > posOne.x ? 1 : -1;
        int kamer1x = 0;
        for (kamer1x = posOne.x; kamer1x != posTwo.x; kamer1x += dirX)
        {
            Vector2Int position = new Vector2Int(kamer1x, posOne.y);
            if (Dungeon.ContainsKey(position)) { continue; }
            Dungeon.Add(position, TileType.Floor);
        }

        int dirY = posTwo.y > posOne.y ? 1 : -1;
        for (int y = posOne.y; y != posTwo.y; y += dirY)
        {
            Vector2Int position = new Vector2Int(kamer1x, y);
            if (Dungeon.ContainsKey(position)) { continue; }
            Dungeon.Add(position, TileType.Floor);
        }
    }

    public void PlaceRoomInsideDungeon(Room _Room, TileType _tile, bool _CanObjectSpawnInRoom)
    {
        for (int x = _Room.minX; x <= _Room.maxX; x++)
        {
            for (int y = _Room.minY; y <= _Room.maxY; y++)
            {
                Dungeon.Add(new Vector2Int(x, y), _tile);
            }
        }
        if (_CanObjectSpawnInRoom) { EnemiesCanSpawnRoomList.Add(_Room); }
        RoomList.Add(_Room);
    }

    public bool CanRoomFitInsideDungeon(Room _Room)
    {
        for(int x = _Room.minX - 1; x <= _Room.maxX + 1; x++)
        {
            for (int y = _Room.minY - 1; y <= _Room.maxY + 1; y++)
            {
                if (Dungeon.ContainsKey(new Vector2Int(x, y))) { return false; }
            }
        }
        return true;
    }

    public class Room
    {
        public int minX, maxX, minY, maxY;
        public Room(int _minX, int _maxX, int _minY,int _maxY)
        {
            minX = _minX;
            maxX = _maxX;
            minY = _minY;
            maxY = _maxY;
        }

        public Vector2Int GetCenter()
        {
            return new Vector2Int(Mathf.RoundToInt(Mathf.Lerp(minX, maxX, 0.5f)), Mathf.RoundToInt(Mathf.Lerp(minY, maxY, 0.5f)));
        }

        public Vector2Int GetRandomPositionInRoom()
        {
            return new Vector2Int(Random.Range(minX, maxX + 1), Random.Range(minY, maxY + 1));
        }
    }
}