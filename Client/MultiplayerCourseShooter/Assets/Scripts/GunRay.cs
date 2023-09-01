using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRay : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 50f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _aim;
    [SerializeField] private float _aimSize;
    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);    

        if(Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _layerMask, QueryTriggerInteraction.Ignore))
        {
            transform.localScale = new Vector3(1f, 1f, hitInfo.distance);
            _aim.position = hitInfo.point;
            float distance = Vector3.Distance(_camera.position, hitInfo.point);
            _aim.localScale = Vector3.one * _aimSize * distance;
        }
    }
}
