using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG
{
    public sealed class GameLogic : MonoBehaviour
    {
        // allows easy swapping of control target
        public ControllableEntity controllable;
        public static GameLogic Instance { get; private set; }
        public KeyCode interactKey;
        public float interactCoolDown = 0.5f;
        public Slider healthSider;
        public TMP_Text healthText;
        public Vector2 InputAxis
        {
            get
            {
                return inputAxis.normalized;
            }
        }
        public int DisplayHealth
        {
            set
            {
                healthSider.value = value;
                UpdateHealthText();
            }

            get
            {
                return (int)healthSider.value;
            }
        }
        public int MaxDisplayHealth
        {
            set
            {
                healthSider.maxValue = value;
                UpdateHealthText();
            }

            get
            {
                return (int)healthSider.maxValue;
            }
        }


        private Vector2 inputAxis = Vector2.zero;
        private float nextInteractTime;


        // Unity Singelton Modeled from https://gamedevbeginner.com/singletons-in-unity-the-right-way/#:~:text=Generally%20speaking%2C%20a%20singleton%20in,or%20to%20other%20game%20systems.
        
        private void Awake()
        {
            CheckHealthSlider();


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

        private void FixedUpdate()
        {
            UpdateAxis();

            if (controllable != null)
                controllable.ControlUpdate(this);
        }

        private void Update()
        {
            CheckInteract();
        }

        private void CheckInteract()
        {
            if (Input.GetKeyDown(interactKey) && nextInteractTime < Time.time)
            {
                Debug.Log("Interact");
                controllable.Interact(this);
                nextInteractTime = Time.time + interactCoolDown;
            }
        }

        private void CheckHealthSlider()
        {
            if (!healthSider.wholeNumbers)
            {
                Debug.LogWarning("DisplaySystem: healthSider value is not set to whole numbers.");
            }
        }

        private void UpdateAxis()
        {
            inputAxis.x = Input.GetAxisRaw("Horizontal");
            inputAxis.y = Input.GetAxisRaw("Vertical");
        }

        private void UpdateHealthText()
        {
            healthText.text = string.Format("{0}/{1}", DisplayHealth, MaxDisplayHealth);
        }
    }
}
