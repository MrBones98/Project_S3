using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Populator : MonoBehaviour
{
    public Vector2 PlayerPosition;
    public Vector2 EnemyPosition;
    public GameObject Player;
    public GameObject Enemy;

    [SerializeField]
    private int _amountOfEnemeies;

    private PathFindingButNodes _pathFindingButNodes;
    private EnemyAI _enemyAI;
    private DungeonSpawner _dungeonSpawner;
    void Start()
    {
        _dungeonSpawner = GetComponent<DungeonSpawner>();
        _pathFindingButNodes = GetComponent<PathFindingButNodes>();

        Invoke("InstantiatePlayer", 1.2f);
        Invoke("InstantiateEnemy", 1.2f);
    }
    
    private void InstantiatePlayer()
    {
        int index = (Random.Range(0, _dungeonSpawner.ListOfRooms.Count - 1));
        PlayerPosition = new Vector2(_dungeonSpawner.ListOfRooms[index].transform.position.x,
                                _dungeonSpawner.ListOfRooms[index].transform.position.y); //Dainis told me nono, Position nono, just write _dungeonSpawner with the rest of the sentence
        Instantiate(Player, PlayerPosition, Quaternion.identity);
    }
    private void InstantiateEnemy()
    {
        for (int i = 0; i < _amountOfEnemeies; i++)
        {
            //int index = (Random.Range(0, _dungeonSpawner.ListOfRooms.Count - 1));
            EnemyPosition = _pathFindingButNodes.StartingNode.position;
            Instantiate(Enemy, EnemyPosition, Quaternion.identity);
        }
    }
}
