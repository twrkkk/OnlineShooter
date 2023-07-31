using Colyseus.Schema;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _lerpForce = 2f;
    [SerializeField] private int _delayCount = 5;
    [SerializeField] private EnemyCharacter _character;
    private Vector3 _targetPosition; 
    private Vector3 _velocity; 
    private List<float> _delays = new List<float>();
    private float _lastReceiveTime;
    private float _avgDelay 
    { 
        get 
        {
            float sum = 0f;
            foreach (var delay in _delays)
                sum += delay;
            sum/=_delays.Count;

            return sum;
        } 
    }

    private void Awake()
    {
        _targetPosition = transform.position;   
    }

    private void SetReceiveTime()
    {
        float delay = Time.time - _lastReceiveTime;
        _lastReceiveTime = Time.time;

        _delays.Add(delay);
        if (_delays.Count > _delayCount)
            _delays.RemoveAt(0);
    }

    internal void OnChange(List<DataChange> changes)
    {
        SetReceiveTime();

        foreach (DataChange change in changes)
        {
            switch (change.Field)
            {
                case "pX":
                    _targetPosition.x = (float)change.Value;
                    break;
                case "pY":
                    _targetPosition.y = (float)change.Value;
                    break;
                case "pZ":
                    _targetPosition.z = (float)change.Value;
                    break;
                case "vX":
                    _velocity.x = (float)change.Value;
                    break;
                case "vY":
                    _velocity.y = (float)change.Value;
                    break;
                case "vZ":
                    _velocity.z = (float)change.Value;
                    break;
                default:
                    break;
            }
        }

        _character.SetMovement(_targetPosition, _velocity, _avgDelay);
    }
}
