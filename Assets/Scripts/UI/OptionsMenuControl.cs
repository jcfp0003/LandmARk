using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuControl : MonoBehaviour
{
    [Header("UI Gameobjects")]
    [SerializeField] protected RectTransform OptionsParentPanel;
    [SerializeField] protected RectTransform QRFeedbackPanel;
    [SerializeField] protected RectTransform TutorialPanel;

    [Header("Animation parameters - General")]
    [SerializeField] protected float AnimationTime;
    [SerializeField] protected Ease OpenEasing;
    [SerializeField] protected Ease CloseEasing;

    [Space]
    [Header("Animation parameters - Feedback QR")]
    [SerializeField] protected float QRAnimationTime;
    [SerializeField] protected Ease QREasing;

    [Space]
    [Header("Animation parameters - Tutorial")]
    [SerializeField] protected float TutorialAnimationTime;
    [SerializeField] protected Ease TutorialEasing;


    protected bool qrPanelShown;
    protected bool tutorialPanelShown;

    private void Start()
    {
        OptionsParentPanel.localScale = Vector3.zero;
        QRFeedbackPanel.localScale = Vector3.zero;
        qrPanelShown = false;
        tutorialPanelShown = false;
    }

    public void ShowOptionsMenu()
    {
        QRFeedbackPanel.localScale = Vector3.zero;
        qrPanelShown = false;
        OptionsParentPanel.DOScale(Vector3.one, AnimationTime).SetEase(OpenEasing).Play();
    }

    public void CloseOptionsMenu()
    {
        HideQRPanel();
        HideTutorial();
        OptionsParentPanel.DOScale(Vector3.zero, AnimationTime).SetEase(CloseEasing).Play();
    }

    public void ShowQRPanel()
    {
        QRFeedbackPanel.DOScale(Vector3.one, QRAnimationTime).SetEase(QREasing).Play();
        qrPanelShown = true;
    }

    public void HideQRPanel()
    {
        QRFeedbackPanel.DOScale(Vector3.zero, QRAnimationTime).SetEase(QREasing).Play();
        qrPanelShown = false;
    }

    public void ToggleQRPanel()
    {
        if (qrPanelShown)
        {
            HideQRPanel();
        }
        else
        {
            ShowQRPanel();
        }
    }

    public void ShowTutorial()
    {
        TutorialPanel.DOScale(Vector3.one, TutorialAnimationTime).SetEase(TutorialEasing).Play();
        tutorialPanelShown = true;
    }

    public void HideTutorial()
    {
        TutorialPanel.DOScale(Vector3.zero, TutorialAnimationTime).SetEase(TutorialEasing).Play();
        tutorialPanelShown = false;
    }

    public void ReturnToMainMenu()
    {
        TokenMoveRegister.ClearState();
        SceneManager.LoadScene(0);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

}
