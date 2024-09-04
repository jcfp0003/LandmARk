using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using Token = ScriptableObjects.Token;

public class ActionGroupControl : MonoBehaviour
{
    [Header("UI objects")]
    [SerializeField] protected Button ActionPressButton;
    [SerializeField] protected Image ActionPreviewImage;
    [SerializeField] protected Image ActionPreviewImageFull;
    [SerializeField] protected GameObject ActionCostsGroup;
    [SerializeField] protected TMP_Text ActionTitleText;
    [SerializeField] protected TMP_Text ActionWoodCostText;
    [SerializeField] protected TMP_Text ActionStoneCostText;
    [SerializeField] protected TMP_Text ActionTurnCostText;
    [SerializeField] protected Image PanelOverImage;

    public void PopulateControl(TokenAction action, Action<TokenAction> callback)
    {
        Token token = action.token;

        if (token != null)
        {
            ActionPreviewImageFull.gameObject.SetActive(false);
            ActionPreviewImage.gameObject.SetActive(true);
            ActionCostsGroup.SetActive(true);
            ActionWoodCostText.text = token.resourceWoodCost.ToString();
            ActionStoneCostText.text = token.resourceStoneCost.ToString();
            ActionTurnCostText.text = token.turnCost.ToString();
            ActionTitleText.text = token.name.GetLocalizedString();
            ActionPreviewImage.sprite = token.sprite;
        }
        else
        {
            ActionPreviewImageFull.gameObject.SetActive(true);
            ActionPreviewImage.gameObject.SetActive(false);
            ActionCostsGroup.SetActive(false);

            ActionWoodCostText.text = "0";
            ActionStoneCostText.text = "0";
            ActionTurnCostText.text = "0";
            ActionTitleText.text = action.name;
            ActionPreviewImageFull.sprite = action.sprite;
        }
        

        ActionPressButton.onClick.RemoveAllListeners();
        ActionPressButton.onClick.AddListener(delegate { callback(action); });
    }

    public void SetPanelOverVisible(bool visible)
    {
        PanelOverImage.gameObject.SetActive(visible);
        ActionPressButton.enabled = !visible;
    }

    private void OnDisable()
    {
        ActionPressButton.onClick.RemoveAllListeners();
    }

}
