using Scellecs.Morpeh.Providers;
using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using TMPro;

namespace MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GamePlayTimerProvider : MonoProvider<GamePlayTimer>
    {
    }

    [Serializable]
    public struct GamePlayTimer : IComponent
    {
        public TextMeshProUGUI Label;
        public float ElapsedSeconds;
    }
}