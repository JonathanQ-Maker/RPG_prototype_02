
using System.Collections;
using UnityEngine;

namespace RPG
{
    public class Mob : Entity
    {
        public Rigidbody2D rb;
        public Animator animator;
        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }

        [SerializeField] // makes editable in editor
        protected float moveSpeed;

        protected override void Start()
        {
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
