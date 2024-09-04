using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public enum TokenActionType
{
    Spawn,
    SwapResource
}

[System.Serializable]
public struct TokenAction
{
    public TokenActionType actionType;
    public Token token;

    public Sprite sprite;
    public string name;
}
