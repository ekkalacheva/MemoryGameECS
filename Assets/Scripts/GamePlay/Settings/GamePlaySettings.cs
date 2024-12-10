using MemoryGame.Game;
using  MemoryGame.Utils;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace  MemoryGame.GamePlay
{
    [CreateAssetMenu(fileName = "GamePlaySettings", menuName = "Installers/GamePlaySettings")]
    public class GamePlaySettings : ScriptableObject
    {
        [SerializeField] 
        private EntityProvider _gameCardPrefab;

        [SerializeField]
        private GameFieldSettings _gameFieldSettingsPhone;

        [SerializeField]
        private GameFieldSettings _gameFieldSettingsTablet;

        [SerializeField]
        private GameCardSprites _gameCardSprites;

        [SerializeField]
        private GameComplexityConfig _complexityConfig;

        public EntityProvider CardPrefab => _gameCardPrefab;

        public GameFieldSettings GameField => DeviceUtils.IsTablet ? _gameFieldSettingsTablet : _gameFieldSettingsPhone;

        public GameCardSprites GameCardSprites => _gameCardSprites;

        public GameComplexityConfig Complexity => _complexityConfig;
    }
}