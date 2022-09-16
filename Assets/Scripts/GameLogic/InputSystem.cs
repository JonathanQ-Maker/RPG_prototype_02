using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG
{
    public sealed class InputSystem : MonoBehaviour
    {
        public static InputSystem Instance { get; private set; }

        // Unity Singelton Modeled from https://gamedevbeginner.com/singletons-in-unity-the-right-way/#:~:text=Generally%20speaking%2C%20a%20singleton%20in,or%20to%20other%20game%20systems.
        private void CheckInstance()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }




        //########################################################
        //# Game Logic
        //########################################################

        public KeyCode interactKey, 
            invToggleKey, 
            attackKey;
        public float interactCoolDown = 0.5f, attackCoolDown = 0.1f;
        public Vector2 InputAxis
        {
            get
            {
                return inputAxis.normalized;
            }
        }
        public ControllableEntity Controllable
        {
            get
            {
                return controllable;
            }

            set
            {
                controllable = value;
                OnControllableChange();
            }
        }

        // allows easy swapping of control target
        [SerializeField]
        private ControllableEntity controllable;
        private Vector2 inputAxis = Vector2.zero;
        private float interactTime, attackTime;

        private void Awake()
        {
            CheckInstance(); // always first
            OnControllableChange();
        }

        private void FixedUpdate()
        {
            UpdateAxis();

            if (controllable != null)
                controllable.ControlUpdate(this);
        }

        private void Update()
        {
            CheckInteract(); 
            CheckInventoryToggle();
            CheckAttack();
        }

        private void CheckInteract()
        {
            if (Input.GetKeyDown(interactKey) && interactTime > interactCoolDown)
            {
                controllable.Interact(this);
                interactTime = 0;
            }
            else
            {
                interactTime += Time.deltaTime;
            }
        }

        private void CheckAttack()
        {
            if (attackTime > attackCoolDown && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                controllable.Attack(this);
                attackTime = 0;
            }
            else
            {
                attackTime += Time.deltaTime;
            }
        }

        private void CheckInventoryToggle()
        {
            if (Input.GetKeyDown(invToggleKey))
            {
                DisplaySystem.Instance.ToggleInventory();
            }
        }

        private void UpdateAxis()
        {
            inputAxis.x = Input.GetAxisRaw("Horizontal");
            inputAxis.y = Input.GetAxisRaw("Vertical");
        }

        private void OnControllableChange()
        {
            if (Controllable != null && Controllable is IInventoryOwner)
            {
                IInventoryOwner inventoryOwner = (IInventoryOwner)Controllable;
                DisplaySystem.Instance.inventoryWindow.Inventory = inventoryOwner.Inventory;
            }
        }
    }
}
