using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardDebugLogger : MonoBehaviour
{
    [Header("GUI elements")]
    [SerializeField] private TMP_Text _debugBoardText;

    [Header("Scene objects")] 
    [SerializeField] private Transform _boardCellsParent;

    private void Update()
    {
        string boardCellsToText = "";

        foreach (Transform cell in _boardCellsParent)
        {
            boardCellsToText += cell.childCount > 0 ? "o" : "x";
            if (boardCellsToText.Length % 9 == 0) boardCellsToText += "\n";
        }

        _debugBoardText.text = boardCellsToText;
    }
}
