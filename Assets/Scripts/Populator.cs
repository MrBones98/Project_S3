using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Populator : MonoBehaviour
{
    public Vector2 PlayerPosition;
    public Vector2 EnemyPosition;
    public GameObject Player;
    public GameObject Enemy;
    public bool Spawned = false;

    [SerializeField]
    private int _amountOfEnemeies;
    public int EnemySpawnIndex;

    private DungeonSpawner _dungeonSpawner;
    void Start()
    {
        _dungeonSpawner = GetComponent<DungeonSpawner>();
        InstantiatePlayer();
        //InstantiateEnemy();
    }
    private void Update()
    {
        if(!Spawned)
        {
            InstantiateEnemy();
        }
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
            EnemySpawnIndex = (int)Random.Range(0, _dungeonSpawner.WalkTheWalk.Count - 1);
            //int index = (Random.Range(0, _dungeonSpawner.ListOfRooms.Count - 1));
            EnemyPosition = _dungeonSpawner.WalkTheWalk[EnemySpawnIndex].position;
            Instantiate(Enemy, EnemyPosition, Quaternion.identity);  
        }
        Spawned = true;
    }
    private IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(5);
    }
}
