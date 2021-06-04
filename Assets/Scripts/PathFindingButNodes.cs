using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingButNodes : MonoBehaviour
{
    private DungeonSpawner _dungeonSpawner;
    private RoomHandlerButNodes _roomHandler;

    public Transform StartingNode, EndNode;
    public List<Transform> ShortestRoute;
    public int RouteLenght;

    private void Start()
    {
        _dungeonSpawner = FindObjectOfType<DungeonSpawner>();
        _roomHandler = FindObjectOfType<RoomHandlerButNodes>();
        Invoke("AssignRandomNodes", 1);
    }

    private void LateUpdate()
    {
        ShortestRoute = FindShortestRoute();
        //ShortestRoute.Reverse();
    }
    private void OnDrawGizmos()
    {
        if (ShortestRoute != null)
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < ShortestRoute.Count - 1; i++)
            {
                Transform nodeA = ShortestRoute[i];
                Transform nodeB = ShortestRoute[i + 1];
                if (nodeA && nodeB)
                {
                    Gizmos.DrawLine(nodeA.transform.position, nodeB.transform.position);
                }
            }
        }
    }
    private List<Transform> FindShortestRoute()
    {
        if (StartingNode == null || EndNode == null)
            return null;

        return AStar(StartingNode, EndNode, _roomHandler.GetDistanceBetween);
    }
    public delegate float HeuristicDelegate(Transform a, Transform b);

    private List<Transform> AStar(Transform start, Transform goal, HeuristicDelegate heuristic)
    {
        List<Transform> openSet = new List<Transform>() { start };


        Dictionary<Transform, Transform> camefrom = new Dictionary<Transform, Transform>();
        Dictionary<Transform, float> gScore = new Dictionary<Transform, float>();
        gScore[start] = 0;

        Dictionary<Transform, float> fScore = new Dictionary<Transform, float>();
        fScore[start] = heuristic(start, goal);

        while (openSet.Count > 0)
        {
            Transform current = GetLowestFScore(openSet, fScore);

            if (current == goal)
                return ReconstructPath(camefrom, current);

            openSet.Remove(current);

            foreach (var neighbour in _roomHandler.GetNeighboursOf(current))
            {
                float tentativeGScore = gScore[current] + _roomHandler.GetDistanceBetween(current, neighbour);
                if (!gScore.ContainsKey(neighbour) || tentativeGScore < gScore[neighbour])
                {
                    camefrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = tentativeGScore + heuristic(neighbour, goal);

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        RewindPath();
        return null;
    }
    private List<Transform> ReconstructPath(Dictionary<Transform, Transform> cameFrom, Transform current)
    {
        List<Transform> totalPath = new List<Transform>() { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        return totalPath;
    }
    private Transform GetLowestFScore(List<Transform> openSet, Dictionary<Transform, float> fScore)
    {
        float min = float.MaxValue;
        Transform minTransform = null;

        foreach (var e in openSet)
        {
            if (fScore.ContainsKey(e))
            {
                float score = fScore[e];
                if (score < min)
                {
                    min = score;
                    minTransform = e;
                }
            }
        }
        return minTransform;
    }
    private void RewindPath()
    {
        List<Transform> rewindedPath = new List<Transform>();
        for (int i = ShortestRoute.Count - 1; i >= 0; i--)
        {
            rewindedPath.Add(ShortestRoute[i]);
        }

        ShortestRoute = rewindedPath;
    }
    private void AssignRandomNodes()
    {
        StartingNode = _dungeonSpawner.WalkTheWalk[Random.Range(0, _dungeonSpawner.WalkTheWalk.Count)];
        EndNode = _dungeonSpawner.WalkTheWalk[Random.Range(0, _dungeonSpawner.WalkTheWalk.Count)];

        while((Vector2.Distance(StartingNode.position, EndNode.position) < RouteLenght))
        {
            EndNode = _dungeonSpawner.WalkTheWalk[Random.Range(0, _dungeonSpawner.WalkTheWalk.Count)];
        }
    }
}
