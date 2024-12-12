using MemoryGame.Game;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Zenject;
using System.Collections.Generic;
using ModestTree;
using Scellecs.Morpeh;

namespace MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameCardInitSystem))]
    public sealed class GameCardInitSystem : UpdateSystem
    {
        private GamePlayModel _gamePlayModel;
        private GameComplexityConfig _complexityConfig;
        private int _availableCardsAmount;
        private GameCardSprites _gameCardSprites;
        private Filter _filter;
        private List<int> _cardIds;

        [Inject]
        private void Construct(GamePlayModel gamePlayModel,
            GameComplexityConfig complexityConfig,
            GameCardSprites gameCardSprites)
        {
            _gamePlayModel = gamePlayModel;
            _complexityConfig = complexityConfig;
            _gameCardSprites = gameCardSprites;
            _availableCardsAmount = gameCardSprites.Faces.Length;
        }

        public override void OnAwake()
        {
            _filter = World.Filter.With<GameCardView>().Without<GameCardModel>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var cardEntity in _filter)
            {
                if (_cardIds == null || _cardIds.IsEmpty())
                {
                    _cardIds = GenerateCardIds();
                }
                
                var id = _cardIds[0];
                _cardIds.RemoveAt(0);
                
                cardEntity.AddComponent<GameCardModel>().Id = id;
                var view = cardEntity.GetComponent<GameCardView>();
                view.BackRenderer.sprite = _gameCardSprites.Back;
                view.FaceRenderer.sprite = _gameCardSprites.Faces[id];
            }
        }

        private List<int> GenerateCardIds()
        {
            var availableCards = new List<int>(_availableCardsAmount);
            for (var i = 0; i < _availableCardsAmount; i++)
            {
                availableCards.Add(i);
            }
            var cardsAmount = _complexityConfig.GetCardsAmount(_gamePlayModel.Complexity);
            var cards = new List<int>(cardsAmount);
            for (var i = 0; i < cardsAmount / 2; i++)
            {
                var randomIndex = Random.Range(0, availableCards.Count);
                var id = availableCards[randomIndex];
                availableCards.RemoveAt(randomIndex);

                cards.Add(id);
                cards.Add(id);
            }

            Shuffle(cards);

            return cards;
        }

        private void Shuffle(List<int> numbers)
        {
            for (int i = 0; i < numbers.Count; i++)
            {
                var tmp = numbers[i];
                var r = Random.Range(i, numbers.Count);
                numbers[i] = numbers[r];
                numbers[r] = tmp;
            }
        }
    }
}