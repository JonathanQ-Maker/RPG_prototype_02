using System.Collections;
using UnityEngine;

namespace RPG
{
    public class CreatureEntity : Entity
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
                yield return new WaitForSeconds(.5f);
            }
        }
    }
}
