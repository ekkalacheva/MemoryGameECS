using System;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameCardUpdateViewSystem))]
    public sealed class GameCardUpdateViewSystem : UpdateSystem
    {
        private Filter _openedCards;
        private Filter _closedCards;

        public override void OnAwake()
        {
            _openedCards = World.Filter.With<GameCardView>().With<Opened>().Build();
            _closedCards = World.Filter.With<GameCardView>().With<Closed>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var card in _openedCards)
            {
                var view = card.GetComponent<GameCardView>();
                view.BackRenderer.gameObject.SetActive(false);
                view.FaceRenderer.gameObject.SetActive(true);
                card.RemoveComponent<Opened>();
            }

            foreach (var card in _closedCards)
            {
                var view = card.GetComponent<GameCardView>();
                view.BackRenderer.gameObject.SetActive(true);
                view.FaceRenderer.gameObject.SetActive(false);
                card.RemoveComponent<Closed>();
            }
        }
    }
}
