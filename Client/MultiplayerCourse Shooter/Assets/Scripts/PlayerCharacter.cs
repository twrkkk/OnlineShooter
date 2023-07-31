using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraCenter;
    [SerializeField] private float _minHeadAngle = -70f;
    [SerializeField] private float _maxHeadAngle = 70f;
    [SerializeField] private float _jumpForce = 5f;
    private float _currentRotateX;
    private float _inputH;
    private float _inputV;
    private float _mouseY;
    private bool _isFly = true;

    private void Start()
    {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraCenter;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

    public void SetInput(float h, float v, float mouseY)
    {
        _inputH = h;
        _inputV = v;
        _mouseY += mouseY;
    }
    public void GetMoveInfo(out Vector3 position, out Vector3 velocity)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
    }

    void FixedUpdate()
    {
        Move();
        RotateBody();
    }

    public void RotateHead(float value)
    {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0f, 0f);
    }

    public void RotateBody() 
    {
        //почему-то не работает
        //_rigidbody.angularVelocity = new Vector3(0f, _mouseY, 0f);
        transform.eulerAngles += new Vector3(0f, _mouseY, 0f);
        _mouseY = 0f;
    }
    private void OnCollisionStay(Collision collision)
    {
        for (int i = 0; _isFly && i < collision.contacts.Length; i++)
        {
            if (collision.contacts[i].normal.y > 0.45f)
                _isFly = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _isFly = true;
    }

    public void Jump()
    {
        if (_isFly) return;
        _rigidbody.AddForce(0f, _jumpForce, 0f, ForceMode.VelocityChange);
    }

    private void Move()
    {
        //Vector3 direction = new Vector3(_inputH, 0, _inputV).normalized;
        //transform.position += direction * Time.deltaTime * _speed;
        Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * _speed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
    }
}
