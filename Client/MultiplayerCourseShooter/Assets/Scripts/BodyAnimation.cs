using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _isSitting;

    private void Awake()
    {
        
    }

    public void Sitdown(bool value)
    {
        if (value == _isSitting) return;

        _isSitting = value;
        _animator.SetBool("Sit", _isSitting);
    }
}
