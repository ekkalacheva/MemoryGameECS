using Zenject;
using Installer = Scellecs.Morpeh.Installer;

namespace MemoryGame.GamePlay
{
    public class InjectableECSInstaller : Installer
    {
        [Inject] private DiContainer _container;

        private void Start()
        {
            foreach (var initializer in initializers)
            {
                _container.Inject(initializer);
            }

            foreach (var updateSystem in updateSystems)
            {
                _container.Inject(updateSystem.System);
            }

            foreach (var fixedUpdateSystem in fixedUpdateSystems)
            {
                _container.Inject(fixedUpdateSystem.System);
            }

            foreach (var lateUpdateSystem in lateUpdateSystems)
            {
                _container.Inject(lateUpdateSystem.System);
            }

            foreach (var cleanupSystem in cleanupSystems)
            {
                _container.Inject(cleanupSystem.System);
            }
        }
    }
}