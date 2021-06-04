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

    private int _index;
    private Vector2 _facingDirection;
    private Vector2 _playerPos;
    private PathFindingButNodes _thePath;
    private List<Transform> _inversed = new List<Transform>();
    private void Awake()
    {
        _thePath = GetComponent<PathFindingButNodes>();
        _playerPos = FindObjectOfType<PlayerController>().GetComponent<Transform>().position;
        StartCoroutine(WaitingDelay());
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
    }
    private void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, _playerPos, _step * Time.deltaTime);
    }

    private IEnumerator MovePls()
    {
        while(_index < _thePath.ShortestRoute.Count - 2)
        {
            for (_index = _thePath.ShortestRoute.Count - 2; _index > _thePath.ShortestRoute.Count - 1;  _index--)
            {
                transform.position = Vector2.MoveTowards(transform.position, _thePath.ShortestRoute[_index].transform.position, _step * Time.deltaTime);
                if (transform.position == _thePath.ShortestRoute[_index].transform.position)
                    continue;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private void Move()
    {
        if (!_thePath.Generated)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, _thePath.ShortestRoute[_index].transform.position, _step * Time.deltaTime);
        
        if(transform.position == _thePath.ShortestRoute[_index].transform.position && _index < _thePath.ShortestRoute.Count -1)
        {
            _index++;
        }
        else if(_index > _thePath.ShortestRoute.Count - 1)
        {
            _index--;
        }

    }
    private void GetIndex()
    {
        _index = _thePath.ShortestRoute.Count; // it should start like at 20
    }
    private IEnumerator WaitingDelay()
    {
        yield return new WaitForSeconds(0.4f);
        _thePath.CalculatePath();

    }
}
