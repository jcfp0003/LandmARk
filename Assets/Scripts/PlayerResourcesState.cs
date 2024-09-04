using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class PlayerResourcesState
{
    public static readonly int WoodPerTurn = 1;
    public static readonly int StonePerTurn = 1;

    public static int PlayerAWoodCount;
    public static int PlayerBWoodCount;
    public static int PlayerAStoneCount;
    public static int PlayerBStoneCount;

    public static int PlayerAWoodIncome = 0;
    public static int PlayerBWoodIncome = 0;
    public static int PlayerAStoneIncome = 0;
    public static int PlayerBStoneIncome = 0;

    public static void ResetResourcesState()
    {
        PlayerAWoodCount = 0;
        PlayerBWoodCount = 0;
        PlayerAStoneCount = 0;
        PlayerBStoneCount = 0;
    }

    public static void TurnSwitch(int newPlayer)
    {
        if (newPlayer == 0)
        {
            PlayerAWoodCount += WoodPerTurn + PlayerAWoodIncome;
            PlayerAStoneCount += StonePerTurn + PlayerAStoneIncome;
        }
        else if (newPlayer == 1)
        {
            PlayerBWoodCount += WoodPerTurn + PlayerBWoodIncome;
            PlayerBStoneCount += StonePerTurn + PlayerBStoneIncome;
        }
    }

    public static bool IsTokenSpawnable(int player, Token token)
    {
        if (player == 0)
        {
            return
                PlayerAWoodCount >= token.resourceWoodCost &&
                PlayerAStoneCount >= token.resourceStoneCost;
        }
        else if (player == 1)
        {
            return
                PlayerBWoodCount >= token.resourceWoodCost &&
                PlayerBStoneCount >= token.resourceStoneCost;
        }
        return false;
    }

    public static void RemoveResources(int player, Token token)
    {
        if (player == 0)
        {
            PlayerAWoodCount -= token.resourceWoodCost;
            PlayerAStoneCount -= token.resourceStoneCost;
        }
        else if (player == 1)
        {
            PlayerBWoodCount -= token.resourceWoodCost;
            PlayerBStoneCount -= token.resourceStoneCost;
        }
    }

    public static void RefundResources(int player, Token token)
    {
        if (player == 0)
        {
            PlayerAWoodCount += token.resourceWoodCost;
            PlayerAStoneCount += token.resourceStoneCost;
        }
        else if (player == 1)
        {
            PlayerBWoodCount += token.resourceWoodCost;
            PlayerBStoneCount += token.resourceStoneCost;
        }
    }

    public static void SetPlayerIncome(int player, int woodIn, int stoneIn)
    {
        if (player == 0)
        {
            PlayerAWoodIncome = woodIn;
            PlayerAStoneIncome = stoneIn;
        }else if (player == 1)
        {
            PlayerBWoodIncome = woodIn;
            PlayerBStoneIncome = stoneIn;
        }
    }
}
