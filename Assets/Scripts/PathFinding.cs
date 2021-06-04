using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathFinding : MonoBehaviour
{
    [SerializeField]
    private NewRoom _start, _destination;
    private RoomHandler _roomHandler;
    private Populator _populator;
    public List<NewRoom> ShortestRoute;
    private void Start()
    {
        _roomHandler = GetComponent<RoomHandler>();
    }

    private void LateUpdate()
    {
        ShortestRoute = FindShortestRoute();
    }
    private void OnDrawGizmos()
    {
        if(ShortestRoute != null)
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < ShortestRoute.Count -1; i++)
            {
                NewRoom roomA = ShortestRoute[i];
                NewRoom roomB = ShortestRoute[i + 1];
                if (roomA && roomB)
                {
                    Gizmos.DrawLine(roomA.transform.position, roomB.transform.position);
                }
            }
        }
    }
    private List<NewRoom> FindShortestRoute()
    {
        if (_start == null || _destination == null)
            return null;

        return AStar(_start, _destination, _roomHandler.GetDistanceBetween);
    }
    public delegate float HeuristicDelegate(NewRoom a, NewRoom b);

    private List<NewRoom> AStar(NewRoom start, NewRoom goal, HeuristicDelegate heuristic)
    {
        List<NewRoom> openSet = new List<NewRoom>() { start };

        
        Dictionary<NewRoom, NewRoom> camefrom = new Dictionary<NewRoom, NewRoom>();
        Dictionary<NewRoom, float> gScore = new Dictionary<NewRoom, float>();
        gScore[start] = 0;

        Dictionary<NewRoom, float> fScore = new Dictionary<NewRoom, float>();
        fScore[start] = heuristic(start, goal);

        while(openSet.Count > 0)
        {
            NewRoom current = GetLowestFScore(openSet, fScore);

            if (current == goal)
                return ReconstructPath(camefrom, current);

            openSet.Remove(current);

            foreach (var neighbour in _roomHandler.GetNeighboursOf(current))
            {
                float tentativeGScore = gScore[current] + _roomHandler.GetDistanceBetween(current, neighbour);
                //bool isSmaller = tentativeGScore < gScore[neighbour];
                //Debug.Log(isSmaller);
                if(!gScore.ContainsKey(neighbour) || tentativeGScore < gScore[neighbour])
                {
                    camefrom[neighbour] = current;
                    gScore[neighbour] = tentativeGScore;
                    fScore[neighbour] = tentativeGScore + heuristic(neighbour, goal);

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }
    private List<NewRoom> ReconstructPath(Dictionary<NewRoom, NewRoom> cameFrom, NewRoom current)
    {
        List<NewRoom> totalPath = new List<NewRoom>() { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        return totalPath;
    }
    private NewRoom GetLowestFScore(List<NewRoom> openSet, Dictionary<NewRoom, float> fScore)
    {
        float min = float.MaxValue;
        NewRoom minRoom = null;

        foreach (var e in openSet)
        {
            if (fScore.ContainsKey(e))
            {
                float score = fScore[e];
                if (score < min)
                {
                    min = score;
                    minRoom = e;
                }
            }
        }
        return minRoom;
    }
}
