using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Populator _populator;
    [SerializeField] [Range(0,1)] private float _interpolator;

    private Vector2 _initialPos;
    private Vector2 _offset;
    private GameObject _player;
    private void Start()
    {
        Invoke("FindPlayer", 1.6f);
        _offset = transform.position - _player.transform.position;
    }
    
    private void FindPlayer()
    {
        _player = FindObjectOfType<PlayerController>().gameObject;
        //_initialPos = _populator.PlayerPosition;
        //_interpolator = 0.01f;
        //while (_interpolator < 1)
        //{
        //    float x = Mathf.Lerp(transform.position.x, _initialPos.x, _interpolator);
        //    float y = Mathf.Lerp(transform.position.y, _initialPos.y, _interpolator);
        //    transform.position = new Vector3(x , y , -10);//new Vector3(_initialPos.x, _initialPos.y,-10);
        //    _interpolator += 0.08f;
        //}
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(_player.transform.position.x + _offset.x, _player.transform.position.y + _offset.y, -10);
    }
}
