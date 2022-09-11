using System.Collections;
using UnityEngine;

namespace RPG
{
    public class BomberPlant : Mob
    {
        public BomberPlantBomb bombPreab;
        public Entity target;
        public Transform bombSpawnTransform;
        public float updateDelta = 0.5f;
        public float attackCoolDown = 5f;

        protected float nextAttackTime;

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

        protected virtual void ShootBomb()
        {
            BomberPlantBomb bomb = Instantiate(bombPreab, bombSpawnTransform.position, Quaternion.identity);
            bomb.BomberPlant = this;
            bomb.LaunchTo(target.transform.position);
        }

        protected virtual void FindTarget()
        {
            target = InputSystem.Instance.Controllable;
        }

        protected virtual void TryAttack()
        {
            if (nextAttackTime < Time.time && target != null)
            { 
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + attackCoolDown;
            }
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
    }
}
