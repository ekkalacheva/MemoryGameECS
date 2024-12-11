using MemoryGame.Utils;
using UnityEngine;
using Zenject;

namespace MemoryGame.GamePlay
{
    [CreateAssetMenu(fileName = "GamePlaySettingsInstaller", menuName = "Installers/GamePlaySettingsInstaller")]
    public class GamePlaySettingsInstaller : ScriptableObjectInstaller<GamePlaySettingsInstaller>
    {
        [SerializeField]
        private GameFieldSettings _gameFieldSettingsPhone;

        [SerializeField]
        private GameFieldSettings _gameFieldSettingsTablet;

        [SerializeField]
        private GameCardSprites _gameCardSprites;

        public override void InstallBindings()
        {
            Container.BindInstance(DeviceUtils.IsTablet ? _gameFieldSettingsTablet : _gameFieldSettingsPhone).AsSingle();
            Container.BindInstance(_gameCardSprites);
        }
    }
}