using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [SerializeField]
    private float _maxDistance = 1.3f;

    [SerializeField] bool visualize;     
    Dictionary<NewRoom, List<NewRoom>> _neighboursMap = new Dictionary<NewRoom, List<NewRoom>>();
    //Dictionary<Wakable, List<Wakable>> _neighboursMap = new Dictionary<Wakable, List<Wakable>>();

    private void Update()
    {
        NewRoom[] rooms = FindObjectsOfType<NewRoom>();
        for (int a = 0; a < rooms.Length; a++)
        {
            NewRoom roomA = rooms[a];
            List<NewRoom> neighbours = new List<NewRoom>();

            for (int b = 0; b < rooms.Length; b++)
            {
                NewRoom roomB = rooms[b];
                float dist = GetDistanceBetween(roomA, roomB);
                if (dist < _maxDistance)
                    neighbours.Add(roomB);
            }
            _neighboursMap[roomA] = neighbours;
        }
    }
    public float GetDistanceBetween(NewRoom a, NewRoom b)
    {
        return (b.transform.position - a.transform.position).magnitude;
    }
    public List<NewRoom> GetNeighboursOf(NewRoom room)
    {

        if (_neighboursMap.ContainsKey(room))
        {
            return _neighboursMap[room];
        }
        throw new System.ArgumentException("Requested information on a room that has not been registered.");
    }
    private void OnDrawGizmos()
    {
        if (!visualize)
            return;

        foreach (var element in _neighboursMap)
        {
            NewRoom key = element.Key;
            foreach (var neighbour in element.Value)
            {
                float s = 1.3f - GetDistanceBetween(key, neighbour) / _maxDistance;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(key.transform.position, neighbour.transform.position);
            }
        }
    }
}
