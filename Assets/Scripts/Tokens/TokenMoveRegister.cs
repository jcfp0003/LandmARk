using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class TokenMoveRegister
{
    public struct SpawnActionState
    {
        public BoardToken fromToken;
        public BoardToken targetToken;
        public BoardCell fromCell;

        public List<BoardCell> targetCells;
        public Action placedCallback;

        public SpawnActionState(BoardToken fromToken, BoardToken targetToken, BoardCell fromCell, List<BoardCell> targetCells, Action placedCallback)
        {
            this.fromToken = fromToken;
            this.targetToken = targetToken;
            this.fromCell = fromCell;
            this.targetCells = targetCells;
            this.placedCallback = placedCallback;
        }
    }

    public struct AttackState
    {
        public BoardToken fromToken;
        public BoardToken toToken;

        public BoardCell fromCell;
        public BoardCell toCell;

        public AttackState(BoardToken fromToken, BoardToken toToken, BoardCell fromCell, BoardCell toCell)
        {
            this.fromToken = fromToken;
            this.toToken = toToken;
            this.fromCell = fromCell;
            this.toCell = toCell;
        }
    }


    public static readonly Dictionary<BoardToken, BoardCell> CurrentBoardMovements = new();
    public static readonly Dictionary<BoardToken, BoardCell> SavedBoardState = new();

    public static readonly HashSet<SpawnActionState> PendingSpawnerUnitMoves = new();
    public static readonly HashSet<SpawnActionState> PendingSpawnTokens = new();

    public static readonly HashSet<AttackState> PendingAttacks = new();


    public static void SetBoardStateEntry(BoardToken token, BoardCell cell)
    {
        SavedBoardState[token] = cell;
        token.gameObject.SetActive(true);
        token.transform.parent = cell.transform;
        token.transform.localPosition = Vector3.zero;
    }

    public static bool CheckCellInState(BoardCell findCell, out KeyValuePair<BoardToken, BoardCell> foundPair)
    {
        foreach (var (token, cell) in SavedBoardState)
        {
            if (!cell.Equals(findCell)) continue;
            
            foundPair = new(token, cell);
            return true;
        }

        foundPair = new(null, null);
        return false;
    }

    public static void ClearState()
    {
        CurrentBoardMovements.Clear();
        SavedBoardState.Clear();
        PendingSpawnerUnitMoves.Clear();
        PendingSpawnTokens.Clear();
        PendingAttacks.Clear();
    }

}
