using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    South=2,
    East =1,
    West=3,
}
public enum DoorExist
{
    WhoKnows, //Uncertain
    Yes,
    No,
}
public static class DoorExistExtension {
    public static bool asBool(this DoorExist doorexist)
    {
        switch(doorexist)
        {
            case DoorExist.WhoKnows:
                throw new System.Exception("DoorExist.Idk cannot be cast as a bool");
            case DoorExist.Yes:
                return true;
            case DoorExist.No:
                return false;
            default:
                throw new System.NotImplementedException();
        }
    }
}

public class DungeonSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject[] _prefabs;

    [SerializeField]
    //[Range(10,50)]
    private int _iterations;
    private Collider2D _randomTrigger;
    public bool DungeonGenerated = false;

    public List<GameObject> ListOfRooms = new List<GameObject>();
    public List<Transform> WalkTheWalk = new List<Transform>();

    private void Start()
    {
        Generate();
    }

    //private void Reset()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        foreach(var room in _open)// need to declare the lists and dictionaries OUTSIDE
    //        ListOfRooms.Clear();
    //        ListOfRooms = new List<Room>();
    //        Invoke("Generate", 1);
    //    }
    //}

    private void Generate()
    {
        List<Vector2Int> openCells = new List<Vector2Int>();
        Dictionary<Vector2Int, Room> cellMap = new Dictionary<Vector2Int, Room>();

        openCells.Add(new Vector2Int(0, 0));

        while(openCells.Count >0 && cellMap.Count < _iterations)
        {
            //Vector2Int cell = openCells[(int)Random.Range(0, openCells.Count )];
            Vector2Int cell = openCells[0];

            if (cellMap.ContainsKey(cell))
            {
                openCells.Remove(cell);
                continue;
            }

            Vector2Int north = cell + new Vector2Int(0, 6);
            Vector2Int east = cell + new Vector2Int(6, 0);
            Vector2Int south = cell + new Vector2Int(0, -6);
            Vector2Int west = cell + new Vector2Int(-6, 0);

            var northDoor = DoorExist.WhoKnows;
            var eastDoor = DoorExist.WhoKnows;
            var southDoor = DoorExist.WhoKnows;
            var westDoor = DoorExist.WhoKnows;

            if (cellMap.ContainsKey(north))
            {
                northDoor = cellMap[north].NeedsRoomFrom(Direction.South);
            }
            if (cellMap.ContainsKey(east))
            {
                eastDoor = cellMap[east].NeedsRoomFrom(Direction.West);
            }
            if (cellMap.ContainsKey(south))
            {
                southDoor = cellMap[south].NeedsRoomFrom(Direction.North);
            }
            if (cellMap.ContainsKey(west))
            {
                westDoor = cellMap[west].NeedsRoomFrom(Direction.East);
            }

            if (northDoor == DoorExist.WhoKnows)
            {
                if (CoinFlip())
                {
                    northDoor = DoorExist.Yes;
                    openCells.Add(north);
                }
                else
                {
                    northDoor = DoorExist.No;
                }
            }

            if (eastDoor == DoorExist.WhoKnows)
            {
                if (CoinFlip())
                {
                    eastDoor = DoorExist.Yes;
                    openCells.Add(east);
                }
                else
                {
                    eastDoor = DoorExist.No;
                }
            }

            if (southDoor == DoorExist.WhoKnows)
            {
                if (CoinFlip())
                {
                    southDoor = DoorExist.Yes;
                    openCells.Add(south);
                }
                else
                {
                    southDoor = DoorExist.No;
                }
            }
            if (westDoor == DoorExist.WhoKnows)
            {
                if (CoinFlip())
                {
                    westDoor = DoorExist.Yes;
                    openCells.Add(west);
                }
                else
                {
                    westDoor = DoorExist.No;
                }
            }
            Room spawnedRoom = new Room(northDoor.asBool(), eastDoor.asBool(), southDoor.asBool(), westDoor.asBool());
            cellMap.Add(cell, spawnedRoom);
            //ListOfRooms.Add(spawnedRoom);
            openCells.Remove(cell);
        }
        foreach (var cell in cellMap)
        {
            GameObject prefab = _prefabs[cell.Value.GetRoomInfo()];
            if (prefab != null)
            {
                Vector3 position = new Vector3(cell.Key.x, cell.Key.y, 0);
                GameObject Room = Instantiate(prefab, position, Quaternion.identity);
                Transform walkable = Room.transform.Find("Walkable");
                if(walkable != null)
                {
                    foreach(Transform child in walkable)
                    {
                        WalkTheWalk.Add(child);
                    }
                }
                ListOfRooms.Add(Room);
            }
        }
        DungeonGenerated = true;
    }
    public bool CoinFlip()
    {
        return Random.value > 0.5;
    }    
}
[System.Serializable]
public class Room
{
    private bool[] _doors;
    public Room(bool north, bool east, bool south, bool west)
    {
        _doors = new bool[] { north, east, south, west };
    }
    public int GetRoomInfo()
    {
        int counter = 0;
        for (int i = 0; i < 4; i++)
        {
            counter += _doors[i] ? 1 <<i : 0;
        }
        return counter;
    }
    public DoorExist NeedsRoomFrom(Direction direction)
    {
        return _doors[(int)direction] ? DoorExist.Yes : DoorExist.No;
    }
}
