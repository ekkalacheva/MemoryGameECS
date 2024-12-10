using System;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using MemoryGame.Game;
using Scellecs.Morpeh.Providers;
using static UnityEditor.U2D.ScriptablePacker;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System.Drawing;

namespace  MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameFieldBuildSystem))]
    public sealed class GameFieldBuildSystem : UpdateSystem
    {
        [SerializeField]
        private GamePlaySettings _settings;

        private GameField _gameField;
        private Camera _camera;
        private int _availableCardsAmount;

        private readonly GameComplexity _complexity = GameComplexity.Easy; //TODO: change this


        public override void OnAwake()
        {
            var entity = World.Filter.With<GameField>().Build().First();
            _gameField = entity.GetComponent<GameField>();
            _camera = Camera.main;
            _availableCardsAmount = _settings.GameCardSprites.Faces.Length;

            CreateGameField();
        }

        public override void OnUpdate(float deltaTime)
        {
        }
        
        private void CreateGameField()
        {
            var cardSize = CalculateCardSize();
            var fieldSize = _settings.Complexity.GetFieldSize(_complexity);
            var cardsOffset = _settings.GameField.GetFieldSettings(_complexity).CardsOffset;

            var startCardPositionX = -0.5f * (cardSize * (fieldSize.Columns - 1) + cardsOffset * (fieldSize.Columns - 1));
            var startCardPositionY = -0.5f * (cardSize * (fieldSize.Rows - 1) + cardsOffset * (fieldSize.Rows - 1));
            
            var ids = GenerateCardIds();

            for (var i = 0; i < fieldSize.Rows; i++)
            {
                for (var j = 0; j < fieldSize.Columns; j++)
                {
                    var id = ids[i * fieldSize.Columns + j];

                    var positionX = startCardPositionX + j * (cardSize + cardsOffset);
                    var positionY = startCardPositionY + i * (cardSize + cardsOffset);

                    var cardModel = new GameCardModel(id);

                    var cardInstance = Instantiate(_settings.CardPrefab);
                    var gameCard = cardInstance.Entity.GetComponent<GameCard>();
                    gameCard.Transform.position = new Vector3(positionX, positionY, gameCard.Transform.position.z);
                    gameCard.Transform.localScale = new Vector3(cardSize, cardSize, gameCard.Transform.localScale.z);
                }
            }
        }

        private float CalculateCardSize()
        {
            var cameraHeight = _camera.orthographicSize * 2;
            var cameraWidth = cameraHeight * _camera.aspect;
            var gameFieldSettings = _settings.GameField.GetFieldSettings(_complexity);
            var borderOffset = gameFieldSettings.BorderOffset;
            var cardsOffset = gameFieldSettings.CardsOffset;
            var fieldSize = _settings.Complexity.GetFieldSize(_complexity);

            var cardWidth = (cameraWidth - 2 * borderOffset.x - (fieldSize.Columns - 1) * cardsOffset) / fieldSize.Columns;
            var cardHeight = (cameraHeight - 2 * borderOffset.y - (fieldSize.Rows - 1) * cardsOffset) / fieldSize.Rows;

            var cardSize = Math.Min(cardWidth, cardHeight);

            return cardSize;
        }

        private int[] GenerateCardIds()
        {
            var availableCards = new List<int>(_availableCardsAmount);
            for (var i = 0; i < _availableCardsAmount; i++)
            {
                availableCards.Add(i);
            }
            var cardsAmount = _settings.Complexity.GetCardsAmount(_complexity);
            var cards = new int[cardsAmount];
            for (var i = 0; i < cardsAmount / 2; i++)
            {
                var randomIndex = Random.Range(0, availableCards.Count);
                var id = availableCards[randomIndex];
                availableCards.RemoveAt(randomIndex);

                cards[i * 2] = id;
                cards[i * 2 + 1] = id;
            }

            ShuffleArray(cards);

            return cards;
        }
        
        private void ShuffleArray(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                var tmp = numbers[i];
                var r = Random.Range(i, numbers.Length);
                numbers[i] = numbers[r];
                numbers[r] = tmp;
            }
        }
    }
}