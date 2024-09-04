using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResourcesUI : MonoBehaviour
{
    [SerializeField] protected TMP_Text PlayerAWoodText;
    [SerializeField] protected TMP_Text PlayerAStoneText;
    [SerializeField] protected TMP_Text PlayerBWoodText;
    [SerializeField] protected TMP_Text PlayerBStoneText;
    [SerializeField] protected GameObject PlayerAInactivePanel;
    [SerializeField] protected GameObject PlayerBInactivePanel;

    private void Update()
    {
        PlayerAWoodText.text = PlayerResourcesState.PlayerAWoodCount.ToString();
        PlayerAStoneText.text = PlayerResourcesState.PlayerAStoneCount.ToString();
        PlayerBWoodText.text = PlayerResourcesState.PlayerBWoodCount.ToString();
        PlayerBStoneText.text = PlayerResourcesState.PlayerBStoneCount.ToString();

        PlayerAInactivePanel.SetActive(PlayerTurnManager.PlayerTurn != 0);
        PlayerBInactivePanel.SetActive(PlayerTurnManager.PlayerTurn != 1);
    }
}
