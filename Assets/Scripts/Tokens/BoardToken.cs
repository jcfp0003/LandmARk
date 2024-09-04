using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class BoardToken : MonoBehaviour
{
    [SerializeField] protected Token tokenProperties;
    [SerializeField] protected int playerOwner;

    public float RemainingHP;
    public bool CanMove;

    private void OnEnable()
    {
        RemainingHP = tokenProperties.maxHealthPoints;
    }

    public int GetPlayerOwner()
    {
        return playerOwner;
    }

    /// <summary>
    /// Reduces token HP by other token attack damage value
    /// </summary>
    /// <param name="other">Attacking token</param>
    /// <returns>True if token HP results in 0 or lower (destroyed), false otherwise</returns>
    public bool ReceiveDamage(BoardToken other, out float damageDealt)
    {
        Token otherProps = other.tokenProperties;

        float multiplier = otherProps.attackAdvanges.Contains(tokenProperties.type) ? 1.5f : 1f;

        damageDealt = otherProps.attackPoints * multiplier;
        RemainingHP -= damageDealt;

        return RemainingHP <= 0.0f;
    }

    public float GetRemainingHp01()
    {
        return RemainingHP / tokenProperties.maxHealthPoints;
    }

    public Token GetProperties()
    {
        return tokenProperties;
    }

    public Vector2Int[] GetTokenRange()
    {
        return tokenProperties.range;
    }

    public bool CheckMovementAllowed(BoardCell from, BoardCell to)
    {
        if (!CanMove) return false;

        var diffVec = from.GetVectorToOther(to);

        return tokenProperties.range.Any(move => move == diffVec);
    }
}
