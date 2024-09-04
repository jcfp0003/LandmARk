using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelControl : MonoBehaviour
{
    [SerializeField] private TokenTargetsManager _tokenTargetsManager;
    [SerializeField] private Button PassTurnButton;
    [SerializeField] private Image PassTurnOverPanel;
    [SerializeField] private Button ClearButton;
    [SerializeField] private Button MovementsButton;
    [SerializeField] private GameObject ActionsPanelView;
    [SerializeField] private Button ActionAttackButton;
    [SerializeField] private ActionGroupControl ActionGroup0;
    [SerializeField] private ActionGroupControl ActionGroup1;
    [SerializeField] private ActionGroupControl ActionGroup2;
    [SerializeField] private ActionGroupControl ActionGroup3;

    protected void SetActiveAllGroups(bool active, bool overPanel = false)
    {
        ActionGroup0.gameObject.SetActive(active);
        ActionGroup0.SetPanelOverVisible(overPanel);
        ActionGroup1.gameObject.SetActive(active);
        ActionGroup1.SetPanelOverVisible(overPanel);
        ActionGroup2.gameObject.SetActive(active);
        ActionGroup2.SetPanelOverVisible(overPanel);
        ActionGroup3.gameObject.SetActive(active);
        ActionGroup3.SetPanelOverVisible(overPanel);
    }

    public void SetUpActions(TokenAction[] actions, Action<TokenAction> actionCallback)
    {
        SetActiveAllGroups(false);
        PassTurnButton.gameObject.SetActive(false);
        ActionsPanelView.SetActive(true);

        if (actions.Length >= 1)
        {
            ActionGroup0.gameObject.SetActive(true);
            ActionGroup0.PopulateControl(actions[0], actionCallback);
            if (actions[0].token != null)
                ActionGroup0.SetPanelOverVisible(
                    !PlayerResourcesState.IsTokenSpawnable(PlayerTurnManager.PlayerTurn, actions[0].token) ||
                    !_tokenTargetsManager.HasHextAvailToken(actions[0].token, PlayerTurnManager.PlayerTurn));
        }
        if (actions.Length >= 2)
        {
            ActionGroup1.gameObject.SetActive(true);
            ActionGroup1.PopulateControl(actions[1], actionCallback);
            if (actions[1].token != null)
                ActionGroup1.SetPanelOverVisible(
                    !PlayerResourcesState.IsTokenSpawnable(PlayerTurnManager.PlayerTurn, actions[1].token) ||
                    !_tokenTargetsManager.HasHextAvailToken(actions[1].token, PlayerTurnManager.PlayerTurn));
        }
        if (actions.Length >= 3)
        {
            ActionGroup2.gameObject.SetActive(true);
            ActionGroup2.PopulateControl(actions[2], actionCallback);
            if (actions[2].token != null)
                ActionGroup2.SetPanelOverVisible(
                    !PlayerResourcesState.IsTokenSpawnable(PlayerTurnManager.PlayerTurn, actions[2].token) ||
                    !_tokenTargetsManager.HasHextAvailToken(actions[2].token, PlayerTurnManager.PlayerTurn));
        }
        if (actions.Length >= 4)
        {
            ActionGroup3.gameObject.SetActive(true);
            ActionGroup3.PopulateControl(actions[3], actionCallback);
            if (actions[3].token != null)
                ActionGroup3.SetPanelOverVisible(
                    !PlayerResourcesState.IsTokenSpawnable(PlayerTurnManager.PlayerTurn, actions[3].token) ||
                    !_tokenTargetsManager.HasHextAvailToken(actions[3].token, PlayerTurnManager.PlayerTurn));
        }
    }

    public void SetUpAttack(Action attackCallback)
    {
        PassTurnButton.gameObject.SetActive(false);
        ActionsPanelView.SetActive(true);
        ActionAttackButton.gameObject.SetActive(true);
        ActionAttackButton.onClick.RemoveAllListeners();
        ActionAttackButton.onClick.AddListener(delegate { attackCallback(); });
    }

    public void SetUpClear(Action clearCallback)
    {
        PassTurnButton.gameObject.SetActive(false);
        ActionsPanelView.SetActive(true);
        ClearButton.onClick.RemoveAllListeners();
        ClearButton.onClick.AddListener(delegate { clearCallback(); });
    }

    public void SetUpMovementsView(Action showMovementsCallback)
    {
        PassTurnButton.gameObject.SetActive(false);
        ActionsPanelView.SetActive(true);
        MovementsButton.gameObject.SetActive(true);
        MovementsButton.onClick.RemoveAllListeners();
        MovementsButton.onClick.AddListener(delegate { showMovementsCallback(); });
    }

    public void ShowPassTurn()
    {
        SetActiveAllGroups(false);

        ClearButton.onClick.RemoveAllListeners();
        MovementsButton.onClick.RemoveAllListeners();
        ActionAttackButton.onClick.RemoveAllListeners();

        MovementsButton.gameObject.SetActive(false);
        ActionAttackButton.gameObject.SetActive(false);
        ActionsPanelView.gameObject.SetActive(false);

        PassTurnButton.gameObject.SetActive(true);
    }

    public void SetPassTurnInteractable(bool interactable)
    {
        PassTurnButton.interactable = interactable;
        PassTurnOverPanel.gameObject.SetActive(!interactable);
    }
}
