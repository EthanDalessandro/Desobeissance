using System;
using System.Collections.Generic;
using UnityEngine;

namespace _PROJECT_.GP.Scripts.Interactables
{
    /// <summary>
    /// Highlights an object with an outline material when interacted with.
    /// </summary>
    public class InteractableOutline : MonoBehaviour, IInteractable
    {
        [Header("Outline Settings")] private Material _basicMaterial;
        [SerializeField] private Material _outlineMaterial;

        private MeshRenderer _renderer;

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