using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ScriptableObjects;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardState : MonoBehaviour
{
    protected enum BoardTurnStates
    {
        Moving_Spawners,
        Spawning_Tokens,
        Moving_Tokens,
        Turn_Ended
    }

    [Header("Manager instances")]
    [SerializeField] private PlayerTurnManager _turnManager;
    [SerializeField] private TokenTargetsManager _tokenTargetsManager;

    [Header("Position marker")]
    [SerializeField] private Transform MarkerParentTransform;
    [SerializeField] private GameObject MarkerSplinePrefab;

    [Header("Particles")]
    [SerializeField] private Transform ParticlesParentTransform;
    [SerializeField] private GameObject DamageParticlePrefab;

    [Header("GUI objects")]
    [SerializeField] private ActionPanelControl ActionsPanel;
    [SerializeField] private GameObject PlayerResourcesPanel;
    [SerializeField] private RectTransform PlayerATurnText;
    [SerializeField] private RectTransform PlayerBTurnText;

    private Dictionary<Vector2Int, MovePositionMarker> _movementMarkers = new();

    protected bool CanPassTurn;
    protected BoardTurnStates CurrentBoardTurnState;

    [Header("Sound effects")] 
    [SerializeField] private UnityEvent AttackSFX;
    [SerializeField] private UnityEvent TokenDiedSFX;
    [SerializeField] private UnityEvent BuildingDiedSFX;
    [SerializeField] private UnityEvent GameEndedSFX;

    [Header("Properties - Game ended")]
    [SerializeField] private RectTransform GameEndPanel;
    [SerializeField] private GameObject[] OtherGUIObjects;
    [SerializeField] protected float GameEndAnimationTime;
    [SerializeField] protected Ease GameEndEasing;

    private void OnEnable()
    {
        ActionsPanel.gameObject.SetActive(true);
        ActionsPanel.ShowPassTurn();
        PlayerResourcesPanel.SetActive(true);
        PlayerResourcesState.ResetResourcesState();

        _turnManager.StartTurns();

        CanPassTurn = true;

        CurrentBoardTurnState = BoardTurnStates.Moving_Spawners;
    }


    private void Update()
    {
        CanPassTurn = true;
        switch (CurrentBoardTurnState)
        {
            case BoardTurnStates.Moving_Spawners:
                CanPassTurn = false;
                HandleMoveSpawners();
                break;
            case BoardTurnStates.Spawning_Tokens:
                CanPassTurn = false;
                HandleSpawnTokens();
                break;
            case BoardTurnStates.Moving_Tokens:
                HandleMoveTokens();
                break;
        }

        ActionsPanel.SetPassTurnInteractable(CanPassTurn);

    }

    protected void HandleMoveSpawners()
    {
        List<TokenMoveRegister.SpawnActionState> movesDone = new();
        HashSet<BoardCell> occupiedCells = new();
        foreach (TokenMoveRegister.SpawnActionState spawnerMove in TokenMoveRegister.PendingSpawnerUnitMoves)
        {
            //BoardCell movedToCell = TokenMoveRegister.CurrentBoardMovements[spawnerMove.fromToken];

            // While token not moved, wait
            if(!TokenMoveRegister.CurrentBoardMovements.TryGetValue(spawnerMove.fromToken, out BoardCell movedToCell))
            {
                continue;
            }

            bool destinationOccupied = occupiedCells.Contains(movedToCell);

            occupiedCells.Add(movedToCell);
            Vector2Int tokenSavedCellPos = spawnerMove.fromCell.GetCellIndexVector();

            // Ignore movements over same cell
            if (spawnerMove.fromCell.Equals(movedToCell))
            {
                //if (_movementMarkers.Remove(spawnerMove.fromCell.GetCellIndexVector(), out var m))
                //{
                //    Destroy(m.gameObject);
                //}
                Debug.LogWarning($"token {spawnerMove.fromToken} ignored at equal");
                continue;
            }

            // When marker doesnt exists, instantiate
            //if (!_movementMarkers.TryGetValue(tokenSavedCellPos, out var marker))
            //{
            //    marker = Instantiate(MarkerSplinePrefab, MarkerParentTransform).GetComponent<MovePositionMarker>();
            //    _movementMarkers.Add(tokenSavedCellPos, marker);
            //}
            //marker.RetargetLine(spawnerMove.fromCell.transform.position, movedToCell.transform.position);

            // Check if token movement is allowed
            if (!spawnerMove.fromToken.CheckMovementAllowed(spawnerMove.fromCell, movedToCell))
            {
                //marker.SwapToMaterial(true);
                Debug.LogWarning($"token {spawnerMove.fromToken} ignored at illegal move");
                continue;
            }
            // Check if token moved to occupied cell
            if (TokenMoveRegister.SavedBoardState.ContainsValue(movedToCell))
            {
                //marker.SwapToMaterial(true);
                Debug.LogWarning($"token {spawnerMove.fromToken} ignored at occupied");
                continue;
            }
            // Check if moved to occupied cell after other move
            if (destinationOccupied)
            {
                //marker.SwapToMaterial(true);
                Debug.LogWarning($"token {spawnerMove.fromToken} ignored at occupied post");
                continue;
            }

            //marker.SwapToMaterial(false);
            Debug.LogWarning($"token {spawnerMove.fromToken} moved");
            movesDone.Add(spawnerMove);
        }

        foreach (var move in movesDone)
        {
            TokenMoveRegister.PendingSpawnerUnitMoves.Remove(move);
            move.placedCallback();

            TokenMoveRegister.SetBoardStateEntry(move.fromToken, TokenMoveRegister.CurrentBoardMovements[move.fromToken]);
        }

        // TODO: check if other pendings get invalidated after movements


        // No movements remain, pass to next board state
        if (TokenMoveRegister.PendingSpawnerUnitMoves.Count == 0)
        {
            CurrentBoardTurnState = BoardTurnStates.Spawning_Tokens;
            CanPassTurn = false;
            // Clear markers
            foreach (var moveMarkers in _movementMarkers)
            {
                Destroy(moveMarkers.Value.gameObject);
            }
            _movementMarkers.Clear();

            TokenMoveRegister.CurrentBoardMovements.Clear();
        }
    }

    protected void HandleSpawnTokens()
    {
        List<TokenMoveRegister.SpawnActionState> spawnsDone = new();

        // Test if token placed for all pending spawns
        foreach (TokenMoveRegister.SpawnActionState pendingSpawn in TokenMoveRegister.PendingSpawnTokens)
        {
            // Search target token in current board state
            if (TokenMoveRegister.CurrentBoardMovements.TryGetValue(pendingSpawn.targetToken, out BoardCell boardCell))
            {
                // Token found, check if placed in allowed cell
                if (pendingSpawn.targetCells.Contains(boardCell))
                {
                    spawnsDone.Add(pendingSpawn);
                }
            }
        }

        // Remove done spawns and invoke callbacks
        TokenMoveRegister.PendingSpawnTokens.RemoveWhere(v => spawnsDone.Contains(v));
        spawnsDone.ForEach(v =>
        {
            v.placedCallback();
            BoardCell targetCell = TokenMoveRegister.CurrentBoardMovements[v.targetToken];
            TokenMoveRegister.SetBoardStateEntry(v.targetToken, targetCell);
            _tokenTargetsManager.SwitchPlayerTokenTrackers(v.targetToken.GetPlayerOwner());

            Debug.LogWarningFormat($"[BoardState] Token {v.targetToken} spawned at {targetCell}");
        });

        // TODO: check if other pendings get invalidated after spawns

        // No spawns remain, pass to next board state
        if (TokenMoveRegister.PendingSpawnTokens.Count == 0)
        {
            CurrentBoardTurnState = BoardTurnStates.Moving_Tokens;
            CanPassTurn = false;
        }
    }

    protected void HandleMoveTokens()
    {
        HashSet<BoardCell> occupiedCells = new HashSet<BoardCell>();

        // Check every movement and update markers
        foreach (var (movedToken, movedToCell) in TokenMoveRegister.CurrentBoardMovements)
        {
            // Get token saved position. Throw exception if token not registered
            if (!TokenMoveRegister.SavedBoardState.TryGetValue(movedToken, out BoardCell tokenSavedCell))
            {
                throw new Exception($"Token {movedToken} missing in state");
            }
            bool targetCellOccupied = occupiedCells.Contains(movedToCell);
            occupiedCells.Add(movedToCell);
            Vector2Int tokenSavedCellPos = tokenSavedCell.GetCellIndexVector();

            // Ignore movements over same cell
            if (tokenSavedCell.Equals(movedToCell))
            {

                if (_movementMarkers.Remove(tokenSavedCellPos, out var m))
                {
                    Destroy(m.gameObject);
                }
                continue;
            }

            // When marker already exists, update target
            if (!_movementMarkers.TryGetValue(tokenSavedCellPos, out var marker))
            {
                // When marker doesnt exists, instantiate
                marker = Instantiate(MarkerSplinePrefab, MarkerParentTransform).GetComponent<MovePositionMarker>();
                _movementMarkers.Add(tokenSavedCellPos, marker);
            }
            marker.RetargetLine(tokenSavedCell.transform.position, movedToCell.transform.position);

            // Check if token movement is allowed
            if (!movedToken.CheckMovementAllowed(tokenSavedCell, movedToCell))
            {
                CanPassTurn = false;
                marker.SwapToMaterial(true);
                continue;
            }
            // Check if token moved to occupied cell
            if (TokenMoveRegister.SavedBoardState.ContainsValue(movedToCell))
            {
                CanPassTurn = false;
                marker.SwapToMaterial(true);
                continue;
            }
            // Check if moved to occupied cell after other move
            if (targetCellOccupied)
            {
                CanPassTurn = false;
                marker.SwapToMaterial(true);
                continue;
            }

            marker.SwapToMaterial(false);

        }
    }

    public void TurnEnded()
    {
        // Clear markers
        foreach (var moveMarkers in _movementMarkers)
        {
            Destroy(moveMarkers.Value.gameObject);
        }
        _movementMarkers.Clear();

        CanPassTurn = false;
        CurrentBoardTurnState = BoardTurnStates.Turn_Ended;

        // Apply movements
        foreach (var (movedToken, movedCell) in TokenMoveRegister.CurrentBoardMovements)
        {
            TokenMoveRegister.SetBoardStateEntry(movedToken, movedCell);
        }

        // Handle attacks
        UnityEvent audioPlayEvent = null;
        int playerWon = -1;
        HashSet<BoardToken> destroyedTokens = new HashSet<BoardToken>();
        foreach (TokenMoveRegister.AttackState attack in TokenMoveRegister.PendingAttacks)
        {
            if(destroyedTokens.Contains(attack.toToken) || destroyedTokens.Contains(attack.fromToken))
            {
                continue;
            }

            float damageDealt = -1;
            bool movedDestroyed = attack.fromToken.ReceiveDamage(attack.toToken, out damageDealt);
            SpawnDamageIndicator(damageDealt.ToString(), attack.fromToken.gameObject.transform);
            bool againstDestroyed = attack.toToken.ReceiveDamage(attack.fromToken, out damageDealt);
            SpawnDamageIndicator(damageDealt.ToString(), attack.toToken.gameObject.transform);


            // Other token defeated, destroy from board
            if (againstDestroyed)
            {
                if (_tokenTargetsManager.TokenIsCentralHall(attack.toToken))
                {
                    // TriggerPlayerWon(attack.fromToken.GetPlayerOwner());
                    playerWon = attack.fromToken.GetPlayerOwner();
                }

                TokenMoveRegister.SavedBoardState.Remove(attack.toToken);
                //_tokenTargetsManager.DisableMilitiaToken(a.toToken);
                _tokenTargetsManager.DisableToken(attack.toToken);
                destroyedTokens.Add(attack.toToken);

                // Moved token survives, move to replace other
                if (!movedDestroyed)
                {
                    TokenMoveRegister.SetBoardStateEntry(attack.fromToken, attack.toCell);
                }

                // Set audio trigger event for state
                if (playerWon == -1)
                {
                    audioPlayEvent = attack.toToken.GetProperties() is Building ? BuildingDiedSFX : TokenDiedSFX;
                }
            }
            // Moved token defeated, destroy from board
            if (movedDestroyed)
            {
                TokenMoveRegister.SavedBoardState.Remove(attack.fromToken);
                //_tokenTargetsManager.DisableMilitiaToken(a.fromToken);
                _tokenTargetsManager.DisableToken(attack.fromToken);
                destroyedTokens.Add(attack.fromToken);
                // If other token survives, it isn't moved as it is the defender

                // Set audio trigger event for state
                if (playerWon == -1)
                {
                    audioPlayEvent = attack.toToken.GetProperties() is Building ? BuildingDiedSFX : TokenDiedSFX;
                }
            }
        }

        // Play attack result SFX
        audioPlayEvent ??= AttackSFX;
        audioPlayEvent.Invoke();

        // Clear board temp state
        TokenMoveRegister.CurrentBoardMovements.Clear();
        TokenMoveRegister.PendingAttacks.Clear();

        // Restart board state
        CurrentBoardTurnState = BoardTurnStates.Moving_Spawners;

        if (playerWon > -1)
        {
            TriggerPlayerWon(playerWon);
            return;
        }

        _turnManager.NextPlayer();
        if (PlayerTurnManager.PlayerTurn == 0)
        {
            PlayerATurnText.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutQuint).OnComplete(() =>
            {
                PlayerATurnText.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InBack);
            });
        }
        else
        {
            PlayerBTurnText.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutQuint).OnComplete(() =>
            {
                PlayerBTurnText.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InBack);
            });
        }

        // Add resource income after turn endend
        int playerTotalWoodIncome = 0;
        int playerTotalStoneIncome = 0;
        foreach (var (boardToken, boardCell) in TokenMoveRegister.SavedBoardState)
        {
            if (boardToken.GetPlayerOwner() != PlayerTurnManager.PlayerTurn)
            {
                continue;
            }

            if (boardToken.GetProperties().generateWood)
            {
                playerTotalWoodIncome += boardToken.GetProperties().woodPerTurn;
            }else if (boardToken.GetProperties().generateStone)
            {
                playerTotalStoneIncome += boardToken.GetProperties().stonePerTurn;
            }
        }
        PlayerResourcesState.SetPlayerIncome(PlayerTurnManager.PlayerTurn, playerTotalWoodIncome, playerTotalStoneIncome);
        PlayerResourcesState.TurnSwitch(PlayerTurnManager.PlayerTurn);

        // Debug.Log($"New turn:\n A -> {PlayerResourcesState.PlayerAWoodCount} wood, {PlayerResourcesState.PlayerAStoneCount}\n    B-> {PlayerResourcesState.PlayerBWoodCount} wood, {PlayerResourcesState.PlayerBStoneCount}");
    }

    private void TriggerPlayerWon(int player)
    {
        GameEndPanel.DOScale(Vector3.one, GameEndAnimationTime).SetEase(GameEndEasing).Play();
        _tokenTargetsManager.DisableAll();
        ActionsPanel.gameObject.SetActive(false);
        PlayerResourcesPanel.SetActive(false);
        foreach (GameObject otherGuiObject in OtherGUIObjects)
        {
            otherGuiObject.transform.localScale = Vector3.zero;
        }
    }

    private void SpawnDamageIndicator(string damage, Transform origin)
    {
        GameObject newIndicator = Instantiate(DamageParticlePrefab, ParticlesParentTransform);
        DamageIndicator indicator = newIndicator.GetComponent<DamageIndicator>();
        indicator.SetupIndicator(damage, origin);
    }
}
