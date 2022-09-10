using System.Collections;
using UnityEngine;

namespace RPG
{
    public class BomberPlantBomb : Entity
    {
        public Animator animator;
        public float explosionRadius = 1f;
        public int damage = 5;
        public Rigidbody2D rb;

        protected void Update()
        {
            UpdateSortingOrder();
        }

        protected void OnExplode() // called by animator event
        {
            rb.velocity = Vector2.zero;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D collider2D in colliders)
            {
                if (collider2D.TryGetComponent(out Entity entity))
                {
                    entity.Health -= damage;
                }
            }
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
        }

        public void LaunchTo(Vector2 targetPos)
        {
            Vector2 delta = targetPos - new Vector2(transform.position.x, transform.position.y);

            rb.velocity = delta / 2f;
        }
    }
}