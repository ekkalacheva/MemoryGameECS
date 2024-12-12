using System;
using UnityEngine;

namespace MemoryGame.GamePlay
{
    internal class ClickDetector: MonoBehaviour
    {
        private Action _listener;

        public void Init(Action listener)
        {
            _listener = listener;
        }

        private void OnEnable()
        {
            var colliders = GetComponentsInChildren<Collider2D>();

            if (colliders.Length <= 0)
            {
                throw new Exception($"There are no any Colliders to handle click event on {name}");
            }
        }

        private void OnMouseDown()
        {
            _listener?.Invoke();
        }
    }
}
