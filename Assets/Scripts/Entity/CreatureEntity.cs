using System.Collections;
using UnityEngine;

namespace RPG
{
    public abstract class CreatureEntity : Entity
    {
        public Rigidbody2D rb;
        public Animator animator;
        public float MoveSpeed
        {
            set
            {
                moveSpeed = Mathf.Max(value, 0);
            }

            get
            {
                return moveSpeed;
            }
        }

        public int AttackDamage
        {
            set
            {
                attackDamage = Mathf.Max(value, 0);
            }

            get
            {
                return attackDamage;
            }
        }

        [SerializeField] // makes editable in unity
        protected float moveSpeed = 2f;
        [SerializeField]
        protected int attackDamage = 1;
        protected override void Start()
        {
            base.Start();
            StartCoroutine(UpdateSortingOrderLoop());
        }

        protected virtual IEnumerator UpdateSortingOrderLoop()
        {
            for (; ; )
            {
                UpdateSortingOrder();
                yield return new WaitForSeconds(.05f);
            }
        }

        public override void Hurt(int damage, Entity attacker)
        {
            base.Hurt(damage, attacker);
            DisplaySystem.Instance.ShowIndicator("" + damage, 
                transform.position + Random.insideUnitSphere, 2f);
        }
    }
}
