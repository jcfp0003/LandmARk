using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TokenActionsControl : MonoBehaviour
{

    [Header("Token actions")]
    [SerializeField] protected bool CanAttack;
    [SerializeField] protected TokenAction[] TokenAvailActions;

    [Header("GUI Gameobjects")]
    [SerializeField] protected ActionPanelControl GUIActionsPanelControl;
    [SerializeField] protected TokenSelectedControl GUITokenSelectedControl;

    public bool isOwnerTurn = true;

    protected BoardToken SelfToken;
    protected BoardCell SelfCell;

    #region SPAWN_PROPERTIES

    [Header("Spawn - Prefabs")]
    [SerializeField] protected GameObject SpawnLocationMarker;
    [SerializeField] protected GameObject SpawnerMoveWarnMarker;

    [Header("Spawn - GUI")]
    [SerializeField] protected TokenTargetsManager TargetsManager;

    protected bool SpawningToken = false;
    protected int SpawningRemainTurns = 0;

    protected Token TokenToSpawn;

    protected List<GameObject> SpawnPositionMarkers;
    protected List<GameObject> SpawnerWarningMarkers;

    #endregion

    #region ATTACK_PROPERTIES

    [Header("Attack - Prefabs")]
    [SerializeField] protected GameObject AttackMarkerPrefab;

    protected struct AttackTargetData
    {
        public GameObject AttackMarker;
        public BoardCell TargetCell;
        public BoardToken TargetToken;
    }
    protected List<AttackTargetData> ActiveAttackMarkers;
    protected bool AttackTargetSelected;

    #endregion

    #region MOVEMENTS_PROPERTIES

    [Header("Movement - Prefabs")]
    [SerializeField] protected GameObject MovementMarkerPrefab;

    protected List<GameObject> MovementMarkers;

    #endregion

    #region RESOURCE_GENERATOR_PROPERTIES

    [Header("Resource Gen - GUI")]
    [SerializeField] protected Sprite ResourceWoodSprite;
    [SerializeField] protected Sprite ResourceStoneSprite;

    [Header("Resource Gen - GameObjects")]
    [SerializeField] protected Image ResourceTypeIndicator;

    #endregion

    private void Start()
    {
        SelfToken = GetComponent<BoardToken>();
        SelfCell = null;
        TokenToSpawn = null;

        SpawnPositionMarkers = new List<GameObject>();
        SpawnerWarningMarkers = new List<GameObject>();
        ActiveAttackMarkers = new List<AttackTargetData>();
        MovementMarkers = new List<GameObject>();
    }

    public void OnTokenInteracted()
    {
        if (!isOwnerTurn) return;
        SelfCell = TokenMoveRegister.SavedBoardState[SelfToken];
        GUIActionsPanelControl.ShowPassTurn();

        if (SpawningToken)
        {
            return;
        }

        if (CanAttack) GUIActionsPanelControl.SetUpAttack(AttackActionCallback);
        GUIActionsPanelControl.SetUpClear(ClearButtonCallback);
        GUIActionsPanelControl.SetUpMovementsView(ShowMovementsCallback);
        GUIActionsPanelControl.SetUpActions(TokenAvailActions, TokenActionCallback);

        if (ResourceTypeIndicator != null)
        {
            ResourceTypeIndicator.gameObject.SetActive(false);
            foreach (TokenAction action in TokenAvailActions)
            {
                if (action.actionType != TokenActionType.SwapResource) continue;

                ResourceTypeIndicator.gameObject.SetActive(true);
                ResourceTypeIndicator.sprite =
                    SelfToken.GetProperties().generateWood ? ResourceWoodSprite : ResourceStoneSprite;
                break;
            }
        }
    }

    protected void AttackActionCallback()
    {
        ClearButtonCallback();

        Vector2Int currentCellIdx = SelfCell.GetCellIndexVector();
        foreach (Vector2Int allowedMovement in SelfToken.GetTokenRange())
        {
            Vector2Int targetPos = allowedMovement + currentCellIdx;

            // Check if cell inside play area
            if (targetPos.x < 0 || targetPos.y < 0) continue;

            GameObject targetCellGo = GameObject.Find($"CellMarker_{targetPos.x}{targetPos.y}");
            if (!targetCellGo) { continue; }

            // Check if cell not empty
            BoardCell targetCell = targetCellGo.GetComponent<BoardCell>();
            if (!TokenMoveRegister.CheckCellInState(targetCell, out var otherTargetPair)) { continue; }

            // Check other token owner
            if (otherTargetPair.Key.GetPlayerOwner() == SelfToken.GetPlayerOwner()) { continue; }

            // All checks done, add target to available attacks
            GameObject markerInstance = Instantiate(AttackMarkerPrefab, targetCell.transform);
            markerInstance.transform.localPosition = Vector3.zero;
            markerInstance.transform.rotation = Quaternion.identity;
            AttackTargetData newTargetData = new AttackTargetData
            {
                AttackMarker = markerInstance,
                TargetCell = targetCell,
                TargetToken = otherTargetPair.Key
            };
            ActiveAttackMarkers.Add(newTargetData);

            // Add callback to marker button listener
            markerInstance.GetComponentInChildren<Button>().onClick.AddListener(delegate { AttackMarkerPressCallback(newTargetData); });
        }
    }

    protected void ClearButtonCallback()
    {
        if (TokenToSpawn)
        {
            PlayerResourcesState.RefundResources(SelfToken.GetPlayerOwner(), TokenToSpawn);
        }
        TokenToSpawn = null;
        if (GUITokenSelectedControl)
        {
            GUITokenSelectedControl.HidePreview();
        }

        AttackClearCallback();

        if (ResourceTypeIndicator != null)
        {
            ResourceTypeIndicator.gameObject.SetActive(false);
        }

        GUIActionsPanelControl.ShowPassTurn();
        SelfToken.CanMove = true;
    }

    protected void ShowMovementsCallback()
    {
        ShowMovementsPreview();
    }

    protected void TokenActionCallback(TokenAction triggeredAction)
    {
        ClearButtonCallback();

        switch (triggeredAction.actionType)
        {
            case TokenActionType.Spawn:
                SpawnActionCallback(triggeredAction.token);
                return;
            case TokenActionType.SwapResource:
                bool currentGenerateWood = SelfToken.GetProperties().generateWood;
                SelfToken.GetProperties().generateWood = !currentGenerateWood;
                SelfToken.GetProperties().generateStone = currentGenerateWood;
                ResourceTypeIndicator.gameObject.SetActive(false);
                ResourceTypeIndicator.sprite =
                    SelfToken.GetProperties().generateWood ? ResourceWoodSprite : ResourceStoneSprite;
                return;
            default:
                return;
        }
    }


    private void OnEnable()
    {
        SelfCell = null;
        TokenToSpawn = null;
        isOwnerTurn = true;
        PlayerTurnManager.TurnChangeEvent += PlayerTurnChangedCallback;
    }

    private void OnDisable()
    {
        foreach (AttackTargetData targetData in ActiveAttackMarkers)
        {
            Destroy(targetData.AttackMarker);
        }
        ActiveAttackMarkers.Clear();

        PlayerTurnManager.TurnChangeEvent -= PlayerTurnChangedCallback;
    }

    protected void PlayerTurnChangedCallback(int player)
    {
        isOwnerTurn = player == SelfToken.GetPlayerOwner();

        if(GUITokenSelectedControl != null)
            GUITokenSelectedControl.gameObject.SetActive(isOwnerTurn);

        if (player == SelfToken.GetPlayerOwner())
        {

            // Decrease spawn counter every owner player turn
            if (SpawningToken)
            {
                SpawningRemainTurns -= 1;
                GUITokenSelectedControl.ShowPreview(TokenToSpawn.sprite, TokenToSpawn.name.GetLocalizedString(), SpawningRemainTurns.ToString());

                if (SpawningRemainTurns == 0)
                {
                    SpawningToken = false;
                    SelfToken.CanMove = true;
                    if (CheckTokenRangeUnoccupied()) // Token can be spawned, add to pending tasks
                    {
                        ShowTargetMarkers();
                    }
                    else // Token range occupied, no spawn made
                    {
                        TokenToSpawn = null;
                        GUITokenSelectedControl.HidePreview();
                        print("Could not spawn token!");
                    }

                    return;
                }
            }
        }

        if (!SpawningToken && TokenToSpawn)
        {
            SpawningToken = true;
            SelfToken.CanMove = false;
        }

        AttackClearCallback();
    }

    #region SPAWN_METHODS

    protected void SpawnActionCallback(Token token)
    {
        if (TokenToSpawn == token)
        {
            return;
        }

        TokenToSpawn = token;
        SpawningRemainTurns = TokenToSpawn.turnCost;

        GUITokenSelectedControl.ShowPreview(TokenToSpawn.sprite, TokenToSpawn.name.GetLocalizedString(), SpawningRemainTurns.ToString());

        PlayerResourcesState.RemoveResources(SelfToken.GetPlayerOwner(), token);
        SelfToken.CanMove = false;
    }

    protected bool CheckTokenRangeUnoccupied()
    {
        int emptyCount = 0;
        foreach (Vector2Int r in SelfToken.GetTokenRange())
        {
            Vector2Int targetPos = SelfCell.GetCellIndexVector() + r;
            if (targetPos.x < 0 || targetPos.y < 0)
            {
                continue;
            }

            GameObject targetCellGO = GameObject.Find($"CellMarker_{targetPos.x}{targetPos.y}");
            if (!targetCellGO)
            {
                throw new MissingReferenceException($"Missing CellMarker_{targetPos.x}{targetPos.y}");
            }

            BoardCell targetCell = targetCellGO.GetComponent<BoardCell>();
            if (TokenMoveRegister.CheckCellInState(targetCell, out var _))
            {
                continue;
            }

            emptyCount += 1;
        }

        return emptyCount > 0;
    }

    protected void ClearTargetMarkers()
    {
        foreach (GameObject spawnPositionMarker in SpawnPositionMarkers)
        {
            Destroy(spawnPositionMarker);
        }
        SpawnPositionMarkers.Clear();
        foreach (GameObject spawnPositionMarker in SpawnerWarningMarkers)
        {
            Destroy(spawnPositionMarker);
        }
        SpawnerWarningMarkers.Clear();
    }

    protected void ShowTargetMarkers()
    {
        ClearTargetMarkers();

        if (TokenToSpawn is Building)
        {
            List<BoardCell> spawnCells = new() { SelfCell };

            GameObject spawnMarker = Instantiate(SpawnerMoveWarnMarker, SelfCell.transform);
            spawnMarker.transform.rotation = Quaternion.identity;
            SpawnerWarningMarkers.Add(spawnMarker);

            TokenCellPositioner nextToken = TargetsManager.GetNextAvailToken(TokenToSpawn, SelfToken.GetPlayerOwner());
            TokenMoveRegister.PendingSpawnerUnitMoves.Add(new(
                SelfToken, nextToken.AssignedTokenObject,
                SelfCell, spawnCells,
                delegate
                {
                    ClearTargetMarkers();

                    //TargetsManager.SingleTargetToken(nextToken, SelfToken.GetPlayerOwner());
                    GameObject spawnMarker = Instantiate(SpawnLocationMarker, SelfCell.transform);
                    spawnMarker.transform.rotation = Quaternion.identity;
                    SpawnPositionMarkers.Add(spawnMarker);
                    TokenMoveRegister.PendingSpawnTokens.Add(new(
                        SelfToken, nextToken.AssignedTokenObject,
                        SelfCell, spawnCells,
                        ClearTargetMarkers
                        ));
                }));
        }
        else
        {

            List<BoardCell> spawnCells = new();
            foreach (Vector2Int move in SelfToken.GetTokenRange())
            {
                Vector2Int targetPos = SelfCell.GetCellIndexVector() + move;
                if (targetPos.x < 0 || targetPos.y < 0)
                {
                    continue;
                }

                GameObject targetCellGO = GameObject.Find($"CellMarker_{targetPos.x}{targetPos.y}");
                if (!targetCellGO) { continue; }

                BoardCell targetCell = targetCellGO.GetComponent<BoardCell>();
                if (TokenMoveRegister.CheckCellInState(targetCell, out var _)) { continue; }

                GameObject spawnMarker = Instantiate(SpawnLocationMarker, targetCell.transform);
                spawnMarker.transform.rotation = Quaternion.identity;
                SpawnPositionMarkers.Add(spawnMarker);

                spawnCells.Add(targetCell);
            }

            if (spawnCells.Count > 0)
            {
                TokenCellPositioner nextToken = TargetsManager.GetNextAvailToken(TokenToSpawn, SelfToken.GetPlayerOwner());
                TargetsManager.SingleTargetToken(nextToken, SelfToken.GetPlayerOwner());
                TokenMoveRegister.PendingSpawnTokens.Add(new(SelfToken, nextToken.AssignedTokenObject, SelfCell, spawnCells, ClearTargetMarkers));
            }
        }

        TokenToSpawn = null;
        ClearButtonCallback();
    }

    #endregion

    #region ATTACK_METHODS

    /// <summary>
    /// World target marker press callback
    /// </summary>
    /// <param name="targetData">Pressed target data</param>
    protected void AttackMarkerPressCallback(AttackTargetData targetData)
    {
        //AttackClearCallback();
        foreach (AttackTargetData attackMarker in ActiveAttackMarkers)
        {
            if (attackMarker.AttackMarker == targetData.AttackMarker) continue;
            Destroy(attackMarker.AttackMarker);
        }
        ActiveAttackMarkers.Clear();
        ActiveAttackMarkers.Add(targetData);
        targetData.AttackMarker.transform.localScale = new(0.8f, 0.8f, 0.8f);

        targetData.AttackMarker.GetComponent<AttackIndicatorControl>().SwapMaterial(true);
        AttackTargetSelected = true;

        BoardCell currentCell = TokenMoveRegister.SavedBoardState[SelfToken];
        TokenMoveRegister.AttackState newAttackState = new TokenMoveRegister.AttackState(SelfToken, targetData.TargetToken, currentCell, targetData.TargetCell);
        TokenMoveRegister.PendingAttacks.RemoveWhere(pendingAttack => pendingAttack.fromToken == SelfToken);
        TokenMoveRegister.PendingAttacks.Add(newAttackState);
    }

    protected void AttackClearCallback()
    {
        foreach (AttackTargetData targetData in ActiveAttackMarkers)
        {
            Destroy(targetData.AttackMarker);
        }
        ActiveAttackMarkers.Clear();

        ClearMovementsPreview();

        AttackTargetSelected = false;
        TokenMoveRegister.PendingAttacks.RemoveWhere(pendingAttack => pendingAttack.fromToken == SelfToken);
    }

    #endregion

    #region MOVEMENTS_METHODS


    protected void ShowMovementsPreview()
    {
        ClearMovementsPreview();

        var moveList = SelfToken.GetTokenRange();
        BoardCell selfCell = TokenMoveRegister.SavedBoardState[SelfToken];
        Vector2Int selfCellPos = selfCell.GetCellIndexVector();
        foreach (Vector2Int move in moveList)
        {
            Vector2Int moveTargetPos = selfCellPos + move;

            GameObject targetCellGo = GameObject.Find($"CellMarker_{moveTargetPos.x}{moveTargetPos.y}");
            if (!targetCellGo) { continue; }

            GameObject newMarker = Instantiate(MovementMarkerPrefab, targetCellGo.transform);
            newMarker.transform.rotation = Quaternion.identity;
            // newMarker.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            MovementMarkers.Add(newMarker);
        }
    }

    protected void ClearMovementsPreview()
    {
        if (MovementMarkers.Count <= 0) return;
        foreach (GameObject marker in MovementMarkers)
        {
            Destroy(marker);
        }
        MovementMarkers.Clear();
    }

    #endregion
}
