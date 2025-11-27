using System;
using System.Collections.Generic;
using UnityEngine;

namespace _PROJECT_.GP.Scripts.Interactables
{
    /// <summary>
    /// Highlights an object with an outline material when interacted with.
    /// Implements IInteractable to support Simple, Hold, and Spam interactions.
    /// </summary>
    public class InteractableOutline : MonoBehaviour, IInteractable
    {
        [Header("Outline Settings")] 
        [SerializeField] private Material _outlineMaterial;
        private Material _basicMaterial;

        private MeshRenderer _renderer;

        // Implement IInteractable properties with default values (hidden from Inspector)
        public InteractionType InteractionType => InteractionType.Simple;
        public float HoldDuration => 0f;
        public int SpamCount => 0;
        public bool CanInteract { get; set; }

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _basicMaterial = _renderer.material;
        }

        public void InteractIn()
        {
            OnHoverEnter();
        }

        public void InteractOut()
        {
            OnHoverExit();
        }

        public void Interact()
        {
            // Optional: Default interaction behavior if needed, or leave empty
            // Debug.Log("Outline Object Interacted: " + gameObject.name);
        }

        private void OnHoverEnter()
        {
            if (!_renderer || !_outlineMaterial) return;

            _renderer.material = _outlineMaterial;
        }

        private void OnHoverExit()
        {
            if (!_renderer || !_outlineMaterial) return;

            _renderer.material = _basicMaterial;
        }
    }
}