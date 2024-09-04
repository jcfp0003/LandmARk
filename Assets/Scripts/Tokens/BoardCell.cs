using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviour, IEquatable<BoardCell>
{
    [SerializeField] protected int X;
    [SerializeField] protected int Y;

    public BoardToken HeldToken;

    public Vector2Int GetCellIndexVector()
    {
        return new Vector2Int(X, Y);
    }

    /// <summary>
    /// Calculates vector of cell steps from self to other cell
    /// </summary>
    /// <param name="o">Target other cell</param>
    /// <returns>Vector with number of steps to reach o from self, both in X and Y</returns>
    public Vector2Int GetVectorToOther(BoardCell o)
    {
        return new Vector2Int(o.X - X, o.Y - Y);
    }


    public bool Equals(BoardCell other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BoardCell)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), X, Y);
    }
}
