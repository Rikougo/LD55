using Summoning.Combination;
using UnityEngine;

namespace Summoning
{
    public class ProjectileComponent : MonoBehaviour
    {
        public Vector3 Direction { get; set; } = Vector3.zero;
        public int Damage { get; set; } = 0;

        private void Update()
        {
            var l_position = transform.position + Direction * Time.deltaTime;
            transform.position = l_position;
        }

        private void OnTriggerEnter2D(Collider2D p_other)
        {
            if (p_other.CompareTag("Enemy"))
                if (p_other.TryGetComponent(out CombinedMonster l_monster))
                {
                    l_monster.TakeDamage(25);
                    Destroy(gameObject);
                }
        }
    }
}