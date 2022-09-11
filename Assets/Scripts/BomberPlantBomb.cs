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

        public BomberPlant BomberPlant { get; set; }

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
                    entity.Hurt(damage, this);
                }
            }
            // -0.1f to ensure bomb wont show up after it has exploded, otherwise you get a flash of the bomb again
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length - 0.1f); 
        }

        public void LaunchTo(Vector2 targetPos)
        {
            Vector2 delta = targetPos - new Vector2(transform.position.x, transform.position.y);

            rb.velocity = delta / 2f;
        }
    }
}