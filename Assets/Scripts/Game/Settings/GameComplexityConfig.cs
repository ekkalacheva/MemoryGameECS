using System;
using UnityEngine;

namespace MemoryGame.Game
{
    [Serializable]
    public class GameComplexityConfig
    {
        [SerializeField]
        private GameFieldSize _easy;

        [SerializeField]
        private GameFieldSize _medium;

        [SerializeField]
        private GameFieldSize _hard;

        public GameFieldSize GetFieldSize(GameComplexity complexity)
        {
            switch (complexity)
            {
                case GameComplexity.Easy: return _easy;
                case GameComplexity.Medium: return _medium;
                case GameComplexity.Hard: return _hard;

                default: throw new NotImplementedException("Unsupported complexity type");
            }
        }

        public int GetCardsAmount(GameComplexity complexity)
        {
            var fieldSize = GetFieldSize(complexity);
            return fieldSize.Columns * fieldSize.Rows;
        }
    }

    [Serializable]
    public class GameFieldSize
    {
        [SerializeField]
        private int _rows;

        [SerializeField]
        private int _columns;

        public int Rows => _rows;
        public int Columns => _columns;
    }
}
