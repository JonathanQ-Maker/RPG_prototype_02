using UnityEngine;

namespace RPG
{
    public class TreeProp : PropEntity
    {
        public float hoverScale = 1.1f;
        public float dropForce = 30f;
        public GameObject apple;

        public override void OnHover(CharacterEntity interactee)
        {
            transform.localScale *= hoverScale; 
        }

        public override void OnUnhover(CharacterEntity interactee)
        {
            transform.localScale /= hoverScale;
        }

        public override void Interact(CharacterEntity interactee)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += 1f;
            GameObject appleInstance = Instantiate(apple, spawnPosition, Quaternion.identity);
            Item itemInstance = appleInstance.GetComponent<Item>();
            itemInstance.rb.velocity = Random.insideUnitCircle * dropForce;
        }
    }
}
