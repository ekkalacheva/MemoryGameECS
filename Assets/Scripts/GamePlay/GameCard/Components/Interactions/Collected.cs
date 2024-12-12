using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace MemoryGame.GamePlay
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Collected: IComponent
    {
    }
}
