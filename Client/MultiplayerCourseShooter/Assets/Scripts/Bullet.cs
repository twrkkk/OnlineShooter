using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _lifeTime = 5f;
    private int _damage;
    public void Init(Vector3 velocity, int damage = 0)
    {
        _damage = damage;
        _rb.velocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Transform p = null;
        if (p = collision.collider.transform.parent)
        {
            if (p.parent)
            {
                if(p.parent.TryGetComponent<EnemyCharacter>(out EnemyCharacter enemy))
                    enemy.ApplyDamage(_damage);
            }
        }
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
