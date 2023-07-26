using Colyseus.Schema;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _lerpForce = 2f;
    [SerializeField] private int _delayCount = 5;
    private Vector3 _targetPosition; 
    private Vector3 _prevPosition;
    private List<float> _delays = new List<float>();
    private float _timer;
    private float _avgDelay;

    private void Awake()
    {
        _targetPosition = transform.position;   
        _prevPosition = transform.position;
    }

    internal void OnChange(List<DataChange> changes)
    {
        AddNewDelay();
        _timer = 0f;

        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "x":
                    _targetPosition.x = (float)change.Value;
                    _prevPosition.x = (float)change.PreviousValue;
                    break;
                case "y":
                    _targetPosition.z = (float)change.Value;
                    _prevPosition.z = (float)change.PreviousValue;
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        _avgDelay = AvgDelay();

        Vector3 direction = _targetPosition - _prevPosition;
        transform.position = Vector3.Lerp(transform.position, _targetPosition + direction * _avgDelay, _lerpForce * Time.deltaTime);
    }

    private void AddNewDelay()
    {
        _delays.Add(_timer);
        if (_delays.Count > _delayCount)
            _delays.RemoveAt(0);
    }

    private float AvgDelay()
    {
        foreach (float delay in _delays)
            _avgDelay += delay;
        _avgDelay /= _delayCount;

        return _avgDelay;
    }
}
