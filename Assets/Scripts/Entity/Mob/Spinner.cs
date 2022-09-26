using System.Collections;
using UnityEngine;

namespace RPG
{
    public class Spinner : Mob
    {
        public Entity target;
        public float updateDelta = 0.5f;
        public float attackCoolDown = 5f;
        public float bounceForce = 50f;
        public bool Attacking
        {
            get;
            protected set;
        }

        protected float nextAttackTime;
        protected IEnumerator attackEnum;

        protected override void Start()
        {
            base.Start();
            FindTarget();
            StartCoroutine(AILoop());
        }

        protected override void OnHealthChange(int oldHealth)
        {
            if (IsDead)
            {
                animator.SetBool("IsDead", true);
                Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
            }

            if (oldHealth > Health)
            {
                animator.SetTrigger("Hurt");
            }
        }

        protected virtual void FindTarget()
        {
            target = InputSystem.Instance.Controllable;
        }

        protected virtual void TryAttack()
        {
            if (nextAttackTime >= attackCoolDown && target != null && !Attacking)
            {
                Attacking = true;
                attackEnum = AttackLoop();
                StartCoroutine(attackEnum);
                nextAttackTime = 0;
            }
            else nextAttackTime += updateDelta;
        }

        protected IEnumerator AttackLoop()
        {
            animator.SetBool("Charging", true);
            while (Attacking)
            {
                Vector2 direction = (target.transform.position - transform.position).normalized;
                rb.AddForce(direction * MoveSpeed, ForceMode2D.Impulse);
                yield return new WaitForFixedUpdate();
            }
            animator.SetBool("Charging", false);
        }

        protected IEnumerator AILoop()
        {
            while (!IsDead)
            {
                if (target == null) FindTarget();
                TryAttack();
                yield return new WaitForSeconds(updateDelta);
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision2D)
        {
            Entity entity;
            if (collision2D.gameObject.TryGetComponent(out entity))
            {
                entity.Hurt(attackDamage, this);
            }
            Attacking = false;
            Vector2 direction = collision2D.contacts[0].normal;
            rb.AddForce(direction * bounceForce, ForceMode2D.Impulse);
        }
    }
}
