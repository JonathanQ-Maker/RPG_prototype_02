﻿using UnityEngine;

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

        // allows easy swapping of control target
        public ControllableEntity controllable;
        public KeyCode interactKey;
        public float interactCoolDown = 0.5f;
        public Vector2 InputAxis
        {
            get
            {
                return inputAxis.normalized;
            }
        }

        private Vector2 inputAxis = Vector2.zero;
        private float nextInteractTime;

        private void Awake()
        {
            CheckInstance();
        }

        private void FixedUpdate()
        {
            UpdateAxis();

            if (controllable != null)
                controllable.ControlUpdate(this);
        }

        private void Update()
        {
            CheckInteract(); // always first
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

        private void UpdateAxis()
        {
            inputAxis.x = Input.GetAxisRaw("Horizontal");
            inputAxis.y = Input.GetAxisRaw("Vertical");
        }
    }
}