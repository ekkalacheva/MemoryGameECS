using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameCardClickInitSystem))]
    public sealed class GameCardClickInitSystem : UpdateSystem
    {
        private Filter _filter;

        public override void OnAwake()
        {
            _filter = World.Filter.With<GameCardView>().Without<Clickable>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var cardEntity in _filter)
            {
                var view = cardEntity.GetComponent<GameCardView>();
                var clickDetector = view.Transform.gameObject.AddComponent<ClickDetector>();
                clickDetector.Init(() =>
                {
                    cardEntity.AddComponent<Clicked>();
                });
                cardEntity.AddComponent<Clickable>();
            }
        }
    }
}