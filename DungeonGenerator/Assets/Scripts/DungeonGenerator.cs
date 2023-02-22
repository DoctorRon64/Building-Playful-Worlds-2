using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using JetBrains.Annotations;
using System.Net.Sockets;

public partial class DungeonGenerator : MonoBehaviour
{
    public enum TileType { Floor , Wall }

    public GameObject MuurObject;
    public GameObject GrondObject;
    public GameObject Enemy;

    public int EnemyAmount;

    public int GridBreedte = 100;
    public int GridHoogte = 100;

    public int minKamerGrote = 3;
    public int maxKamerGrote = 7;

    public int numKamers = 10;
    public Dictionary<Vector2Int, TileType> Kerker = new Dictionary<Vector2Int, TileType>();
    public List<Room> kamerList = new List<Room>();
    public List<GameObject> alleGeinstantieerdePrefabs = new List<GameObject>();

    private void Awake()
    {
        Generate();
    }

    [ContextMenu("DungeonMaker")]
    public void Generate()
    {
        Debug.Log("Begin de kerker te genereren");

        ClearDungeon();
        AllLocateRooms();
        ConnectRooms();
        AllLocateWalls();
        SpwanRandomObjectInRoom(Enemy);
        GenereerKerker();
        //spwan player
        //spawn items
    }

    [ContextMenu("Clear Dungeon")]
    public void ClearDungeon()
    {
        for (int i = alleGeinstantieerdePrefabs.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(alleGeinstantieerdePrefabs[i]);
        }

        Kerker.Clear();
        kamerList.Clear();
        alleGeinstantieerdePrefabs.Clear();
    }

    private void ConnectRooms()
    {
        // [0, 1, 2, {3}, 4, 5] 0, 1, 2
        for (int i = 0; i < kamerList.Count; i++)
        {
            Room room = kamerList[i];
            Room otherRoom = kamerList[(i + Random.Range(1, kamerList.Count)) % kamerList.Count];
            ConnectKamers(room, otherRoom);
        }
    }

    private void AllLocateRooms()
    {
        //for de hoeveelheid kamers erzijn
        for (int i = 0; i < numKamers; i++)
        {
            //randomize de grotes van de kamers op basis van de gridsize
            int minX = Random.Range(0, GridBreedte);
            int maxX = minX + Random.Range(minKamerGrote, maxKamerGrote + 1);
            int minY = Random.Range(0, GridHoogte);
            int maxY = minY + Random.Range(minKamerGrote, maxKamerGrote + 1);

            //genereer kamer met deze exacte grotes
            Room kamer = new Room(minX, maxX, minY, maxY);

            //kan de kamer in de dungeon passen zo niet genereer een nieuwe grote kamer
            if (KanDeKamerInDeKerkerPassen(kamer))
            {
                //returned een boolean waarde of de kamer geplaatst kan worden
                PlaatsKamerInKerker(kamer);
            } else
            {
                i--;
            }
        }
    }
    private void AllLocateWalls()
    {
        var keys = Kerker.Keys.ToList();
        foreach (var kv in keys)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    //if(Mathf.Abs(x) == Mathf.Abs(z)) { continue; }
                    Vector2Int newPos = kv + new Vector2Int(x, y);
                    if (Kerker.ContainsKey(newPos)) { continue; }
                    Kerker.Add(newPos, TileType.Wall);
                }
            }
        }
    }

    private void GenereerKerker()
    {
        foreach (KeyValuePair<Vector2Int, TileType> keyvalue in Kerker)
        {
            Vector3 posTile = new Vector3(keyvalue.Key.x, keyvalue.Key.y, 0);
            GameObject obj = null;
            switch (keyvalue.Value)
            {
                case TileType.Floor: obj = Instantiate(GrondObject, posTile, Quaternion.identity, transform); break;
                case TileType.Wall: obj = Instantiate(MuurObject, posTile, Quaternion.identity, transform); break;
            }
            alleGeinstantieerdePrefabs.Add(obj);
        }
    }

    private void SpwanRandomObjectInRoom(GameObject _obj)
    {
        Vector3 posRandomInRoom;
        for (int j = 0; j < kamerList.Count; j++)
        {
            posRandomInRoom = new Vector3(kamerList[j].GetRandomPositionInRoom().x, kamerList[j].GetRandomPositionInRoom().y, 0);
            GameObject instanceObj = Instantiate(_obj, posRandomInRoom, Quaternion.identity);
            instanceObj.transform.parent = gameObject.transform;
            alleGeinstantieerdePrefabs.Add(instanceObj);
        }
    }

    private void ConnectKamers(Room _Kamer1, Room _Kamer2)
    {
        //beweeg een gang op de x as to hij een kamer raakt en dan op de y as tot hij de kamer berijkt
        Vector2Int posEen = _Kamer1.GetCenter();
        Vector2Int posTwee = _Kamer2.GetCenter();
        int dirX = posTwee.x > posEen.x ? 1 : -1;
        int kamer1x = 0;
        for (kamer1x = posEen.x; kamer1x != posTwee.x; kamer1x += dirX)
        {
            Vector2Int position = new Vector2Int(kamer1x, posEen.y);
            if (Kerker.ContainsKey(position)) { continue; }
            Kerker.Add(position, TileType.Floor);
        }

        int dirY = posTwee.y > posEen.y ? 1 : -1;
        for (int y = posEen.y; y != posTwee.y; y += dirY)
        {
            Vector2Int position = new Vector2Int(kamer1x, y);
            if (Kerker.ContainsKey(position)) { continue; }
            Kerker.Add(position, TileType.Floor);
        }
    }

    public void PlaatsKamerInKerker(Room _kamer)
    {
        for (int x = _kamer.minX; x <= _kamer.maxX; x++)
        {
            for (int y = _kamer.minY; y <= _kamer.maxY; y++)
            {
                Kerker.Add(new Vector2Int(x, y), TileType.Floor);
            }
        }
        kamerList.Add(_kamer);
    }

    public bool KanDeKamerInDeKerkerPassen(Room _kamer)
    {
        for(int x = _kamer.minX - 1; x <= _kamer.maxX + 1; x++)
        {
            for (int y = _kamer.minY - 1; y <= _kamer.maxY + 1; y++)
            {
                if (Kerker.ContainsKey(new Vector2Int(x, y))) { return false; }
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