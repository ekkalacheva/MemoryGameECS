using System;
using UnityEngine;

namespace MemoryGame.GamePlay
{
    internal class ClickDetector: MonoBehaviour
    {
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
        }
    }
}
