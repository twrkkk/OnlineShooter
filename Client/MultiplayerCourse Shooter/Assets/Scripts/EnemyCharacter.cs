using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    private Vector3 _targetPosition;

    private float _velocityMagnitude;

    private void Start()
    {
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if(_velocityMagnitude > Constants.Epsilon)
        {
            float maxDistance = _velocityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, maxDistance);
        }
        else
        {
            transform.position = _targetPosition;
        }
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float avgDelay)
    {
        _targetPosition = position + velocity * avgDelay;
        _velocityMagnitude = velocity.magnitude;
    }
}
