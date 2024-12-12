using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using System;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameFieldRebuildSystem))]
    public sealed class GameFieldRebuildSystem: UpdateSystem
    {
        private Event<RestartGameEvent> _restartGameEvent;

        public override void OnAwake()
        {
            _restartGameEvent = World.GetEvent<RestartGameEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var evt in _restartGameEvent.publishedChanges)
            {
                var gameCardsFilter = World.Filter.With<GameCardView>().Build();
                foreach (var gameCard in gameCardsFilter)
                {
                    gameCard.RemoveComponent<GameCardModel>();
                    gameCard.RemoveComponent<Opened>();
                    gameCard.RemoveComponent<Closed>();
                    gameCard.RemoveComponent<Collected>();
                }
            }
        }
    }
}
