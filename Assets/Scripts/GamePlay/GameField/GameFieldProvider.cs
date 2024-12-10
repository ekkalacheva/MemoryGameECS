using Scellecs.Morpeh.Providers;
using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace  MemoryGame.GamePlay
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class GameFieldProvider : MonoProvider<GameField>
    {
    }

    [Serializable]
    public struct GameField : IComponent
    {
        public Transform Container;
    }
}