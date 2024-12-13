using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;

namespace MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GamePlayTimerUpdateSystem))]
    public sealed class GamePlayTimerUpdateSystem : UpdateSystem
    {
        private readonly Color ActiveTimeColor = new Color(1, 1, 1);
        private readonly Color InactiveTimeColor = new Color(0, 0, 0);

        private Filter _timerFilter;
        private Event<GameStartedEvent> _gameStartedEvent;
        private Event<GameCompletedEvent> _gameCompletedEvent;
        private Event<RestartGameEvent> _restartGameEvent;

        private bool _isActive;
        
        public override void OnAwake()
        {
            _timerFilter = World.Filter.With<GamePlayTimer>().Build();
            _gameStartedEvent = World.GetEvent<GameStartedEvent>();
            _gameCompletedEvent = World.GetEvent<GameCompletedEvent>();
            _restartGameEvent = World.GetEvent<RestartGameEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var timerEntity in _timerFilter)
            {
                UpdateTimer(timerEntity);
            }

            foreach (var evt in _gameStartedEvent.publishedChanges)
            {
                StartTimer();
            }

            foreach (var evt in _gameCompletedEvent.publishedChanges)
            {
                StopTimer();
            }

            foreach (var evt in _restartGameEvent.publishedChanges)
            {
                RestartTimer();
            }
        }

        private void UpdateTimer(Entity timerEntity)
        {
            if (!_isActive)
            {
                return;
            }

            ref var timer = ref timerEntity.GetComponent<GamePlayTimer>();
            timer.ElapsedSeconds += Time.deltaTime;

            UpdateTimerText(timer);
        }

        private void UpdateTimerText(GamePlayTimer timer)
        {
            var minutes = Mathf.FloorToInt(timer.ElapsedSeconds / 60);
            var seconds = Mathf.FloorToInt(timer.ElapsedSeconds % 60);
            timer.Label.text = $"{minutes:00}:{seconds:00}";
        }

        public void UpdateTimerColorState(bool isActive)
        {
            foreach (var timerEntity in _timerFilter)
            {
                ref var timer = ref timerEntity.GetComponent<GamePlayTimer>();
                timer.Label.color = isActive ? ActiveTimeColor : InactiveTimeColor;
            }
        }

        private void StartTimer()
        {
            _isActive = true;
            UpdateTimerColorState(true);
        }

        private void StopTimer()
        {
            _isActive = false;
            UpdateTimerColorState(false);
        }

        private void RestartTimer()
        {
            _isActive = false;
            foreach (var timerEntity in _timerFilter)
            {
                ref var timer = ref timerEntity.GetComponent<GamePlayTimer>();
                timer.ElapsedSeconds = 0;
                UpdateTimerText(timer);
            }

            UpdateTimerColorState(true);
        }
    }
}