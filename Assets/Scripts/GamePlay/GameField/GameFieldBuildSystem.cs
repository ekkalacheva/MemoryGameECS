using System;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using MemoryGame.Game;
using Zenject;
using Scellecs.Morpeh.Providers;

namespace  MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(GameFieldBuildSystem))]
    public sealed class GameFieldBuildSystem : Initializer
    {
        [SerializeField]
        private EntityProvider _gameCardPrefab;

        private GamePlayModel _gamePlayModel;
        private GameComplexityConfig _complexityConfig;
        private GameFieldOffsets _gameFieldSettings;
        private GameField _gameField;
        private Camera _camera;
        
        [Inject]
        private void Construct(GamePlayModel gamePlayModel,
                               GameComplexityConfig complexityConfig,
                               GameCardSprites gameCardSprites,
                               GameFieldSettings gameFieldSettings)
        {
            _gamePlayModel = gamePlayModel;
            _complexityConfig = complexityConfig;
            _gameFieldSettings = gameFieldSettings.GetFieldSettings(_gamePlayModel.Complexity);
        }

        public override void OnAwake()
        {
            var entity = World.Filter.With<GameField>().Build().First();
            _gameField = entity.GetComponent<GameField>();
            _camera = Camera.main;

            CreateGameField();
        }

        public override void Dispose()
        {
        }

        private void CreateGameField()
        {
            var cardSize = CalculateCardSize();
            var fieldSize = _complexityConfig.GetFieldSize(_gamePlayModel.Complexity);
            var cardsOffset = _gameFieldSettings.CardsOffset;

            var startCardPositionX = -0.5f * (cardSize * (fieldSize.Columns - 1) + cardsOffset * (fieldSize.Columns - 1));
            var startCardPositionY = -0.5f * (cardSize * (fieldSize.Rows - 1) + cardsOffset * (fieldSize.Rows - 1));
            
            for (var i = 0; i < fieldSize.Rows; i++)
            {
                for (var j = 0; j < fieldSize.Columns; j++)
                {
                    var positionX = startCardPositionX + j * (cardSize + cardsOffset);
                    var positionY = startCardPositionY + i * (cardSize + cardsOffset);

                    var cardInstance = Instantiate(_gameCardPrefab);
                    var gameCard = cardInstance.Entity.GetComponent<GameCard>();
                    gameCard.Transform.SetParent(_gameField.Container,false);
                    gameCard.Transform.position = new Vector3(positionX, positionY, gameCard.Transform.position.z);
                    gameCard.Transform.localScale = new Vector3(cardSize, cardSize, gameCard.Transform.localScale.z);
                }
            }
        }

        private float CalculateCardSize()
        {
            var cameraHeight = _camera.orthographicSize * 2;
            var cameraWidth = cameraHeight * _camera.aspect;
            var borderOffset = _gameFieldSettings.BorderOffset;
            var cardsOffset = _gameFieldSettings.CardsOffset;
            var fieldSize = _complexityConfig.GetFieldSize(_gamePlayModel.Complexity);

            var cardWidth = (cameraWidth - 2 * borderOffset.x - (fieldSize.Columns - 1) * cardsOffset) / fieldSize.Columns;
            var cardHeight = (cameraHeight - 2 * borderOffset.y - (fieldSize.Rows - 1) * cardsOffset) / fieldSize.Rows;

            var cardSize = Math.Min(cardWidth, cardHeight);

            return cardSize;
        }
    }
}