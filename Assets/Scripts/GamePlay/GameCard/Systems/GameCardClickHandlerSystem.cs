using MemoryGame.Utils;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using Zenject;
using System.Diagnostics;

namespace MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameCardClickHandlerSystem))]
    public sealed class GameCardClickHandlerSystem : UpdateSystem
    {
        private const float CardsHideDelaySeconds = 0.8f;

        [Inject] private ICoroutineHandler _coroutineHandler;
        private Coroutine _cardsCloseCoroutine;

        private Filter _filter;
        private Event<RestartGameEvent> _restartGameEvent;

        private Entity _openedCard1;
        private Entity _openedCard2;
        
        public override void OnAwake()
        {
            _filter = World.Filter.With<GameCardView>().With<Clicked>().Build();
            _restartGameEvent = World.GetEvent<RestartGameEvent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var card in _filter)
            {
                ProcessCardClick(card);
            }

            foreach (var evt in _restartGameEvent.publishedChanges)
            {
                ClearActiveCards();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            ClearActiveCards();
        }

        private void ProcessCardClick(Entity card)
        {
            card.RemoveComponent<Clicked>();

            ref var model = ref card.GetComponent<GameCardModel>();
            if (model.State == GameCardState.Collected || model.State == GameCardState.Opened)
            {
                return;
            }

            if (_openedCard1 == null)
            {
                _openedCard1 = card;
                model.State = GameCardState.Opened;
                card.AddComponent<Opened>();
                CheckGameStart();
                return;
            }

            if (_openedCard2 != null)
            {
                return;
            }

            _openedCard2 = card;
            model.State = GameCardState.Opened;
            card.AddComponent<Opened>();
            ref var firstCardModel = ref _openedCard1.GetComponent<GameCardModel>();
            if (model.Id == firstCardModel.Id)
            {
                firstCardModel.State = GameCardState.Collected;
                model.State = GameCardState.Collected;
                _openedCard1.AddComponent<Collected>();
                _openedCard2.AddComponent<Collected>();
                _openedCard1 = null;
                _openedCard2 = null;
                PairCardsCollected();
                return;
            }

            _cardsCloseCoroutine = _coroutineHandler.DelayAction(() =>
            {
                ref var firstCardModel = ref _openedCard1.GetComponent<GameCardModel>();
                ref var secondCardModel = ref _openedCard2.GetComponent<GameCardModel>();
                firstCardModel.State = GameCardState.Closed;
                secondCardModel.State = GameCardState.Closed;
                _openedCard1.AddComponent<Closed>();
                _openedCard2.AddComponent<Closed>();
                _openedCard1 = null;
                _openedCard2 = null;
                _cardsCloseCoroutine = null;
            }, CardsHideDelaySeconds);
        }

        private void CheckGameStart()
        {
            // if (!_gameStarted)
            // {
            //     _gameStarted = true;
            //     // _signals.TryFire<GamePlaySignals.GameStarted>();
            // }
        }

        private void PairCardsCollected()
        {
            // _collectedCardsAmount += 2;
            // if (_collectedCardsAmount == _cardsAmount)
            // {
            //     // _signals.TryFire<GamePlaySignals.GameCompleted>();
            // }
        }

        private void ClearActiveCards()
        {
            if (_cardsCloseCoroutine != null)
            {
                _coroutineHandler.StopCoroutine(_cardsCloseCoroutine);
            }

            _openedCard1 = null;
            _openedCard2 = null;
            // _gameStarted = false;
            // _collectedCardsAmount = 0;
        }
    }
}