using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandlerButNodes : MonoBehaviour
{
    [SerializeField]
    private float _maxDistance = 1.3f;

    [SerializeField] bool visualize;
    Dictionary<Transform, List<Transform>> _neighboursMap = new Dictionary<Transform, List<Transform>>();

    private DungeonSpawner _dungeonSpawner;
    private void Start()
    {
        _dungeonSpawner = GetComponent<DungeonSpawner>();
    }
    private void Update()
    {
        List<Transform> nodes = _dungeonSpawner.WalkTheWalk;
        for (int a = 0; a < nodes.Count; a++)
        {
            Transform nodeA = nodes[a];
            List<Transform> neighbours = new List<Transform>();

            for (int b = 0; b < nodes.Count; b++)
            {
                Transform nodeB = nodes[b];
                float dist = GetDistanceBetween(nodeA, nodeB);
                if (dist < _maxDistance)
                    neighbours.Add(nodeB);
            }
            _neighboursMap[nodeA] = neighbours;
        }
    }
    public float GetDistanceBetween(Transform a, Transform b)
    {
        return (b.position - a.position).magnitude;
    }
    public List<Transform> GetNeighboursOf(Transform node)
    {

        if (_neighboursMap.ContainsKey(node))
        {
            return _neighboursMap[node];
        }
        throw new System.ArgumentException("Requested information on a room that has not been registered.");
    }
    private void OnDrawGizmos()
    {
        if (!visualize)
            return;

        foreach (var element in _neighboursMap)
        {
            Transform key = element.Key;
            foreach (var neighbour in element.Value)
            {
                float s = 1.3f - GetDistanceBetween(key, neighbour) / _maxDistance;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(key.transform.position, neighbour.transform.position);
            }
        }
    }
}
