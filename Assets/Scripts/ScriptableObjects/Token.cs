using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace ScriptableObjects
{
    public class Token : ScriptableObject
    {
        public int maxHealthPoints;
        public int attackPoints;
        public int turnCost;
        public int resourceWoodCost;
        public int resourceStoneCost;

        public LocalizedString name;
        public Sprite sprite;

        public int woodPerTurn;
        public int stonePerTurn;
        public bool generateWood;
        public bool generateStone;

        public enum TokenType
        {
            CentralHall,
            Militia,
            Knight,
            Siege,
            Barracks,
            Resource
        }
        public TokenType type;
        public List<TokenType> attackAdvanges;

        public Vector2Int[] range;
    }
}
