using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    private Vector3 _targetPosition;
    public void SetMovement(in Vector3 position, in Vector3 velocity, in float avgDelay)
    {
        _targetPosition = position + velocity * avgDelay;
    }
}
