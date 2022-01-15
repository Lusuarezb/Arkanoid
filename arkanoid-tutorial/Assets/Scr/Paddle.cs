using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _movementLimit = 7;
    private float _maxSize = 1.2f;
    private float _minSize = 0.8f;
    
    private Vector3 _targetPosition;
    private Camera _cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _targetPosition.x = Camera.ScreenToWorldPoint(Input.mousePosition).x;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -_movementLimit, _movementLimit);
        _targetPosition.y = this.transform.position.y;
        
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _speed);
    }
    
    public float GetMinSize(){
        return _minSize;
    }

    public float GetMaxSize(){
        return _maxSize;
    }

    private Camera Camera{
        get{
            if (_cam == null){
                _cam = Camera.main;
            }
            return _cam;
        }
    }
}
