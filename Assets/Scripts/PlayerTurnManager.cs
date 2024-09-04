using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTurnManager : MonoBehaviour
{
    public delegate void TurnChanged(int playerTurn);
    public static event TurnChanged TurnChangeEvent;

    [SerializeField]
    protected int MaxPlayers = 2;

    public static int PlayerTurn = 0;

    public void StartTurns()
    {
        PlayerTurn = 0;

        //TurnChangeEvent?.Invoke(PlayerTurn);
    }

    public void NextPlayer()
    {
        PlayerTurn = (PlayerTurn + 1) % MaxPlayers;

        TurnChangeEvent?.Invoke(PlayerTurn);
    }
}
