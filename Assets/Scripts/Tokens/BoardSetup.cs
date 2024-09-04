using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSetup : MonoBehaviour
{
    [SerializeField] protected TokenTargetsManager TokenTargetsManager;
    [SerializeField] protected BoardState MainBoardState;

    [SerializeField] protected BoardCell[] SetupPositionCells;
    [SerializeField] protected GameObject[] SetupPositionMarkers;

    [SerializeField] protected Transform MarkerTransform;

    private enum GameState
    {
        None,
        SetupA_CH,
        SetupA_MIL,
        SetupB_CH,
        SetupB_MIL,
        Playing
    }
    private GameState _gameState;

    public void InitSetup()
    {
        _gameState = GameState.SetupA_CH;
        MarkerTransform.gameObject.SetActive(true);
        MarkerTransform.transform.parent = SetupPositionCells[0].transform;
        MarkerTransform.transform.localPosition = Vector3.zero;
        MarkerTransform.transform.localScale = Vector3.one;

        GameObject positionMarker = SetupPositionMarkers[0];
        Instantiate(positionMarker, MarkerTransform.transform);

        TokenTargetsManager.SetUpCentralHall(0);
        TokenTargetsManager.GetTokenCentralHallA().gameObject.SetActive(true);
    }

    private void HandleSetupCH_A()
    {
        BoardToken chAtoken = TokenTargetsManager.GetTokenCentralHallA();

        // CH token not moved, ignore loop
        if (!TokenMoveRegister.CurrentBoardMovements.TryGetValue(chAtoken, out BoardCell chACell)) return;

        // CH token moved, update position to cell
        chAtoken.transform.parent = chACell.transform;
        chAtoken.transform.localPosition = Vector3.zero;

        // Check token cell, if not targeted setup position, continue loop
        if (chACell.transform != MarkerTransform.parent) return;

        // Token cell equals to target setup position, continue to next setup
        MarkerTransform.gameObject.SetActive(true);
        MarkerTransform.transform.parent = SetupPositionCells[1].transform;
        MarkerTransform.transform.localPosition = Vector3.zero;
        MarkerTransform.transform.localScale = Vector3.one;

        Destroy(MarkerTransform.GetChild(0).gameObject);
        GameObject positionMarker = SetupPositionMarkers[1];
        Instantiate(positionMarker, MarkerTransform.transform);

        TokenTargetsManager.SetUpInitialToken(0);
        TokenTargetsManager.GetInitialTokenA().gameObject.SetActive(true);
        _gameState = GameState.SetupA_MIL;
        TokenMoveRegister.CurrentBoardMovements.Clear();
        TokenMoveRegister.SetBoardStateEntry(chAtoken, chACell);
    }

    private void HandleSetupMIL_A()
    {
        BoardToken milAtoken = TokenTargetsManager.GetInitialTokenA();

        // CH token not moved, ignore loop
        if (!TokenMoveRegister.CurrentBoardMovements.TryGetValue(milAtoken, out BoardCell milACell)) return;

        // CH token moved, update position to cell
        milAtoken.transform.parent = milACell.transform;
        milAtoken.transform.localPosition = Vector3.zero;

        // Check token cell, if not targeted setup position, continue loop
        if (milACell.transform != MarkerTransform.transform.parent) return;

        // Token cell equals to target setup position, continue to next setup
        MarkerTransform.gameObject.SetActive(true);
        MarkerTransform.transform.parent = SetupPositionCells[2].transform;
        MarkerTransform.transform.localPosition = Vector3.zero;
        MarkerTransform.transform.localScale = Vector3.one;

        Destroy(MarkerTransform.GetChild(0).gameObject);
        GameObject positionMarker = SetupPositionMarkers[2];
        Instantiate(positionMarker, MarkerTransform.transform);

        TokenTargetsManager.SetUpCentralHall(1);
        TokenTargetsManager.GetTokenCentralHallB().gameObject.SetActive(true);
        _gameState = GameState.SetupB_CH;
        TokenMoveRegister.CurrentBoardMovements.Clear();
        TokenMoveRegister.SetBoardStateEntry(milAtoken, milACell);
    }

    private void HandleSetupCH_B()
    {
        BoardToken chBtoken = TokenTargetsManager.GetTokenCentralHallB();

        // CH token not moved, ignore loop
        if (!TokenMoveRegister.CurrentBoardMovements.TryGetValue(chBtoken, out BoardCell chBCell)) return;

        // CH token moved, update position to cell
        chBtoken.transform.parent = chBCell.transform;
        chBtoken.transform.localPosition = Vector3.zero;


        // Check token cell, if not targeted setup position, continue loop
        if (chBCell.transform != MarkerTransform.transform.parent) return;

        // Token cell equals to target setup position, continue to next setup
        MarkerTransform.gameObject.SetActive(true);
        MarkerTransform.transform.parent = SetupPositionCells[3].transform;
        MarkerTransform.transform.localPosition = Vector3.zero;
        MarkerTransform.transform.localScale = Vector3.one;

        Destroy(MarkerTransform.GetChild(0).gameObject);
        GameObject positionMarker = SetupPositionMarkers[3];
        Instantiate(positionMarker, MarkerTransform.transform);

        TokenTargetsManager.SetUpInitialToken(1);
        TokenTargetsManager.GetInitialTokenB().gameObject.SetActive(true);
        _gameState = GameState.SetupB_MIL;
        TokenMoveRegister.CurrentBoardMovements.Clear();
        TokenMoveRegister.SetBoardStateEntry(chBtoken, chBCell);
    }

    private void HandleSetupMIL_B()
    {
        BoardToken milBtoken = TokenTargetsManager.GetInitialTokenB();

        // CH token not moved, ignore loop
        if (!TokenMoveRegister.CurrentBoardMovements.TryGetValue(milBtoken, out BoardCell milBCell)) return;

        // CH token moved, update position to cell
        milBtoken.transform.parent = milBCell.transform;
        milBtoken.transform.localPosition = Vector3.zero;


        // Check token cell, if not targeted setup position, continue loop
        if (milBCell.transform != MarkerTransform.transform.parent) return;

        foreach (Transform child in MarkerTransform.transform)
        {
            Destroy(child.gameObject);
        }
        MarkerTransform.parent = null;
        MarkerTransform.position = Vector3.zero;
        MarkerTransform.localScale = Vector3.one;

        TokenTargetsManager.StartGameSetup();
        _gameState = GameState.Playing;
        TokenMoveRegister.CurrentBoardMovements.Clear();
        TokenMoveRegister.SetBoardStateEntry(milBtoken, milBCell);

        // Setup done, disable setup script
        MainBoardState.enabled = true;
        enabled = false;
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.None:
                return;
            case GameState.SetupA_CH:
                HandleSetupCH_A();
                break;
            case GameState.SetupA_MIL:
                HandleSetupMIL_A();
                break;
            case GameState.SetupB_CH:
                HandleSetupCH_B();
                break;
            case GameState.SetupB_MIL:
                HandleSetupMIL_B();
                break;
            case GameState.Playing:
                return;
            default:
                return;
        }
    }
}
