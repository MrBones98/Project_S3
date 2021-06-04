using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum States
{
    Idle,
    Found,
}
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _step;
    [SerializeField] private float _sightRange = 2;
    [SerializeField] private States _npcState = States.Idle;

    private int _index =0;
    private Vector2 _facingDirection;
    private Vector2 _playerPos;
    private PathFindingButNodes _thePath;
    private List<Transform> _inversed = new List<Transform>();
    private void Start()
    {
        _thePath = FindObjectOfType<PathFindingButNodes>();
        _playerPos = FindObjectOfType<PlayerController>().GetComponent<Transform>().position;
        //_thePath.ShortestRoute.Reverse(0, _thePath.ShortestRoute.Count);        
        //transform.position = Vector2.MoveTowards(transform.position, _thePath.ShortestRoute[1].position, _step);
    }
    private void Update()
    {
        switch (_npcState)
        {
            case States.Idle:
                //IdleBehaviour();
                Move();
                if (Searching())
                    _npcState = States.Found;
                break;
            case States.Found:
                FollowPlayer();
                if (!Searching())
                    _npcState = States.Idle;

                break;

            default:
                break;
        }
    }
    private void IdleBehaviour()
    {
        StartCoroutine(MovePls());
    }
    private bool Searching()
    {
        if(Vector2.Distance(transform.position, _playerPos) < _sightRange)
        {
            var hit = Physics2D.Raycast(transform.position, _playerPos);
            if(hit.transform.TryGetComponent(out PlayerController player))
            {
                _playerPos = player.transform.position;
                return true;
            }
        }
        return false;
        //var hits = Physics2D.CircleCast(transform.position, 1.5f, transform.right, ;
        //foreach (var hit in hits)
        //{
        //    if (hit.transform.TryGetComponent(out PlayerController player))
        //    {
        //        _playerPos = player.transform.position;
        //        return true;
        //    }
        //}
        //return false;
    }
    private void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _playerPos, _step * Time.deltaTime);
    }

    private IEnumerator MovePls()
    {
        for (int i = 0; i < _thePath.ShortestRoute.Count - 1;  i++)
        {
            yield return StartCoroutine(GoingBro(transform.position, _thePath.ShortestRoute[i].position, _step));
            
        }
    }
    private IEnumerator GoingBro(Vector2 position, Vector2 goal, float speed)
    {
        Debug.Log(goal);
        while (position != goal)
        {
            Vector2 direction = (goal - position).normalized;
            transform.position = goal;
            yield return null;
        }
    }
    private void Move()
    {
        if(_thePath.ShortestRoute == null)
        {
            return;
        }
        Vector2 direction = (_thePath.ShortestRoute[_index].position - transform.position).normalized;
        transform.position += (Vector3)direction * _step * Time.deltaTime;
        if((transform.position - _thePath.ShortestRoute[_index].position).magnitude < .1f)
        {
            if(_index < _thePath.ShortestRoute.Count-1)
            _index++;
        }
    }
}
