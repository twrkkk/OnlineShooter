using UnityEngine;

public class FollowTo : MonoBehaviour
{
    [SerializeField] private Transform _target;
    void Update()
    {
        transform.position = _target.position;  
    }
}
