using ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenTargetsManager : MonoBehaviour
{
    [System.Serializable]
    protected class TargetState
    {
        public TokenCellPositioner TokenImageTarget;
        public bool IsUsed;

        public TokenCellPositioner SetUp(bool enabled)
        {
            IsUsed = enabled;
            TokenImageTarget.gameObject.SetActive(enabled);
            TokenImageTarget.AssignedTokenObject.gameObject.SetActive(enabled);

            //Debug.LogWarning($"TOKEN {TokenImageTarget.AssignedTokenObject} ENABLED: {IsUsed}");
            return TokenImageTarget;
        }
    }


    [SerializeField] protected TokenCellPositioner CentralHallPlayerA;
    [SerializeField] protected TokenCellPositioner CentralHallPlayerB;

    //[SerializeField] protected TokenCellPositioner InitialTokenA;
    //[SerializeField] protected TokenCellPositioner InitialTokenB;
    [SerializeField] protected List<TargetState> TokensMilitiaA;
    [SerializeField] protected List<TargetState> TokensMilitiaB;
    [SerializeField] protected List<TargetState> TokensKnightA;
    [SerializeField] protected List<TargetState> TokensKnightB;
    [SerializeField] protected List<TargetState> TokensSiegeA;
    [SerializeField] protected List<TargetState> TokensSiegeB;
    [SerializeField] protected List<TargetState> TokensBarracksA;
    [SerializeField] protected List<TargetState> TokensBarracksB;
    [SerializeField] protected List<TargetState> TokensResourceA;
    [SerializeField] protected List<TargetState> TokensResourceB;

    #region SETUP_METHODS

    public void SetUpCentralHall(int player)
    {
        CentralHallPlayerA.gameObject.SetActive(player == 0);
        CentralHallPlayerB.gameObject.SetActive(player == 1);

        //TokensMilitiaA[0].TokenImageTarget.gameObject.SetActive(false);
        //TokensMilitiaA[1].TokenImageTarget.gameObject.SetActive(false);
        TokensMilitiaA[0].SetUp(false);
        TokensMilitiaB[0].SetUp(false);
    }

    public void SetUpInitialToken(int player)
    {
        CentralHallPlayerA.gameObject.SetActive(false);
        CentralHallPlayerB.gameObject.SetActive(false);

        TokensMilitiaA[0].SetUp(player == 0);
        TokensMilitiaB[0].SetUp(player == 1);
    }

    public void StartGameSetup()
    {
        CentralHallPlayerA.gameObject.SetActive(false);
        CentralHallPlayerB.gameObject.SetActive(false);
        TokensMilitiaA[0].SetUp(true);
        TokensMilitiaB[0].SetUp(true);

        SwitchPlayerTokenTrackers(0);
    }

    public BoardToken GetTokenCentralHallA()
    {
        return CentralHallPlayerA.AssignedTokenObject;
    }

    public BoardToken GetTokenCentralHallB()
    {
        return CentralHallPlayerB.AssignedTokenObject;
    }

    public BoardToken GetInitialTokenA()
    {
        return TokensMilitiaA[0].TokenImageTarget.AssignedTokenObject;
    }

    public BoardToken GetInitialTokenB()
    {
        return TokensMilitiaB[0].TokenImageTarget.AssignedTokenObject;
    }

    public bool TokenIsCentralHallA(BoardToken token)
    {
        return token == CentralHallPlayerA.AssignedTokenObject;
    }

    public bool TokenIsCentralHallB(BoardToken token)
    {
        return token == CentralHallPlayerB.AssignedTokenObject;
    }

    public bool TokenIsCentralHall(BoardToken token)
    {
        return TokenIsCentralHallA(token) || TokenIsCentralHallB(token);
    }

    #endregion

    #region TOKEN_ENABLERS

    protected TokenCellPositioner EnableMilitiaToken(int player)
    {
        if (player == 0)
        {
            foreach (TargetState targetState in TokensMilitiaA)
            {
                if (targetState.IsUsed) continue;
                //targetState.IsAvailable = true;
                //targetState.TokenImageTarget.gameObject.SetActive(true);
                //targetState.TokenImageTarget.AssignedTokenObject.gameObject.SetActive(true);
                //return targetState.TokenImageTarget;
                return targetState.SetUp(true);
            }
        }
        else
        {
            foreach (TargetState targetState in TokensMilitiaB)
            {
                if (targetState.IsUsed) continue;
                //targetState.IsAvailable = true;
                //targetState.TokenImageTarget.gameObject.SetActive(true);
                //targetState.TokenImageTarget.AssignedTokenObject.gameObject.SetActive(true);
                //return targetState.TokenImageTarget;
                return targetState.SetUp(true);
            }
        }
        return null;
    }

    protected TokenCellPositioner EnableKnightToken(int player)
    {
        if (player == 0)
        {
            foreach (TargetState targetState in TokensKnightA)
            {
                if (targetState.IsUsed) continue;
                //targetState.IsAvailable = true;
                //targetState.TokenImageTarget.gameObject.SetActive(true);
                //targetState.TokenImageTarget.AssignedTokenObject.gameObject.SetActive(true);
                //return targetState.TokenImageTarget;
                return targetState.SetUp(true);
            }
        }
        else
        {
            foreach (TargetState targetState in TokensKnightB)
            {
                if (targetState.IsUsed) continue;
                //targetState.IsAvailable = true;
                //targetState.TokenImageTarget.gameObject.SetActive(true);
                //targetState.TokenImageTarget.AssignedTokenObject.gameObject.SetActive(true);
                //return targetState.TokenImageTarget;
                return targetState.SetUp(true);
            }
        }
        return null;
    }

    protected TokenCellPositioner EnableSiegeToken(int player)
    {
        if (player == 0)
        {
            foreach (TargetState targetState in TokensSiegeA)
            {
                if (targetState.IsUsed) continue;
                return targetState.SetUp(true);
            }
        }
        else
        {
            foreach (TargetState targetState in TokensSiegeB)
            {
                if (targetState.IsUsed) continue;
                return targetState.SetUp(true);
            }
        }
        return null;
    }

    protected TokenCellPositioner EnableBarracksToken(int player)
    {
        if (player == 0)
        {
            foreach (TargetState targetState in TokensBarracksA)
            {
                if (targetState.IsUsed) continue;
                return targetState.SetUp(true);
            }
        }
        else
        {
            foreach (TargetState targetState in TokensBarracksB)
            {
                if (targetState.IsUsed) continue;
                return targetState.SetUp(true);
            }
        }
        return null;
    }

    protected TokenCellPositioner EnableResourceToken(int player)
    {
        if (player == 0)
        {
            foreach (TargetState targetState in TokensResourceA)
            {
                if (targetState.IsUsed) continue;
                return targetState.SetUp(true);
            }
        }
        else
        {
            foreach (TargetState targetState in TokensResourceB)
            {
                if (targetState.IsUsed) continue;
                return targetState.SetUp(true);
            }
        }
        return null;
    }

    #endregion

    #region TOKEN_DISABLERS

    protected void DisableMilitiaToken(BoardToken token)
    {
        if (token.GetPlayerOwner() == 0)
        {
            foreach (TargetState targetState in TokensMilitiaA)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                //targetState.IsAvailable = false;
                //targetState.TokenImageTarget.gameObject.SetActive(false);
                //targetState.TokenImageTarget.AssignedTokenObject.gameObject.SetActive(false);
                targetState.SetUp(false);
                return;
            }
        }
        else
        {
            foreach (TargetState targetState in TokensMilitiaB)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                //targetState.IsAvailable = false;
                //targetState.TokenImageTarget.gameObject.SetActive(false);
                //targetState.TokenImageTarget.AssignedTokenObject.gameObject.SetActive(false);
                targetState.SetUp(false);
                return;
            }
        }
    }

    protected void DisableKnightToken(BoardToken token)
    {
        if (token.GetPlayerOwner() == 0)
        {
            foreach (TargetState targetState in TokensKnightA)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                //targetState.IsAvailable = false;
                //targetState.TokenImageTarget.gameObject.SetActive(false);
                //targetState.TokenImageTarget.AssignedTokenObject.gameObject.SetActive(false);
                targetState.SetUp(false);
                return;
            }
        }
        else
        {
            foreach (TargetState targetState in TokensKnightB)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                //targetState.IsAvailable = false;
                //targetState.TokenImageTarget.gameObject.SetActive(false);
                //targetState.TokenImageTarget.AssignedTokenObject.gameObject.SetActive(false);
                targetState.SetUp(false);
                return;
            }
        }
    }

    protected void DisableSiegeToken(BoardToken token)
    {
        if (token.GetPlayerOwner() == 0)
        {
            foreach (TargetState targetState in TokensSiegeA)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                targetState.SetUp(false);
                return;
            }
        }
        else
        {
            foreach (TargetState targetState in TokensSiegeB)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                targetState.SetUp(false);
                return;
            }
        }
    }

    protected void DisableBarracksToken(BoardToken token)
    {
        if (token.GetPlayerOwner() == 0)
        {
            foreach (TargetState targetState in TokensBarracksA)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                targetState.SetUp(false);
                return;
            }
        }
        else
        {
            foreach (TargetState targetState in TokensBarracksB)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                targetState.SetUp(false);
                return;
            }
        }
    }

    protected void DisableResourceToken(BoardToken token)
    {
        if (token.GetPlayerOwner() == 0)
        {
            foreach (TargetState targetState in TokensResourceA)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                targetState.SetUp(false);
                return;
            }
        }
        else
        {
            foreach (TargetState targetState in TokensResourceB)
            {
                if (token != targetState.TokenImageTarget.AssignedTokenObject) continue;
                targetState.SetUp(false);
                return;
            }
        }
    }

    #endregion

    protected IEnumerable<TargetState> AllTokensPlayerA()
    {
        foreach (TargetState targetState in TokensMilitiaA)
        {
            yield return targetState;
        }
        foreach (TargetState targetState in TokensKnightA)
        {
            yield return targetState;
        }
        foreach (TargetState targetState in TokensSiegeA)
        {
            yield return targetState;
        }
        foreach (TargetState targetState in TokensBarracksA)
        {
            yield return targetState;
        }
        foreach (TargetState targetState in TokensResourceA)
        {
            yield return targetState;
        }
    }
    protected IEnumerable<TargetState> AllTokensPlayerB()
    {
        foreach (TargetState targetState in TokensMilitiaB)
        {
            yield return targetState;
        }
        foreach (TargetState targetState in TokensKnightB)
        {
            yield return targetState;
        }
        foreach (TargetState targetState in TokensSiegeB)
        {
            yield return targetState;
        }
        foreach (TargetState targetState in TokensBarracksB)
        {
            yield return targetState;
        }
        foreach (TargetState targetState in TokensResourceB)
        {
            yield return targetState;
        }
    }

    public TokenCellPositioner GetNextAvailToken(Token tokenType, int player)
    {
        switch (tokenType.type)
        {
            case Token.TokenType.CentralHall:
                break;
            case Token.TokenType.Militia:
                return EnableMilitiaToken(player);
            case Token.TokenType.Knight:
                return EnableKnightToken(player);
            case Token.TokenType.Siege:
                return EnableSiegeToken(player);
            case Token.TokenType.Barracks:
                return EnableBarracksToken(player);
            case Token.TokenType.Resource:
                return EnableResourceToken(player);
        }

        return null;
    }

    public bool HasHextAvailToken(Token tokenType, int player) {

        if (player == 0)
        {
            switch (tokenType.type)
            {
                case Token.TokenType.CentralHall:
                    break;
                case Token.TokenType.Militia:
                    foreach (TargetState token in TokensMilitiaA)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
                case Token.TokenType.Knight:
                    foreach (TargetState token in TokensKnightA)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
                case Token.TokenType.Siege:
                    foreach (TargetState token in TokensSiegeA)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
                case Token.TokenType.Barracks:
                    foreach (TargetState token in TokensBarracksA)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
                case Token.TokenType.Resource:
                    foreach (TargetState token in TokensResourceA)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
            }
        }
        else
        {
            switch (tokenType.type)
            {
                case Token.TokenType.CentralHall:
                    break;
                case Token.TokenType.Militia:
                    foreach (TargetState token in TokensMilitiaB)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
                case Token.TokenType.Knight:
                    foreach (TargetState token in TokensKnightB)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
                case Token.TokenType.Siege:
                    foreach (TargetState token in TokensSiegeB)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
                case Token.TokenType.Barracks:
                    foreach (TargetState token in TokensBarracksB)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
                case Token.TokenType.Resource:
                    foreach (TargetState token in TokensResourceB)
                    {
                        if (!token.IsUsed) return true;
                    }
                    break;
            }
        }

        return false;
    }

    public void DisableToken(BoardToken boardToken)
    {
        switch (boardToken.GetProperties().type)
        {
            case Token.TokenType.CentralHall:
                if (boardToken.GetPlayerOwner() == 0)
                {
                    CentralHallPlayerA.AssignedTokenObject.gameObject.SetActive(false);
                    CentralHallPlayerA.gameObject.SetActive(false);
                }
                else
                {
                    CentralHallPlayerB.AssignedTokenObject.gameObject.SetActive(false);
                    CentralHallPlayerB.gameObject.SetActive(false);
                }
                break;
            case Token.TokenType.Militia:
                DisableMilitiaToken(boardToken);
                break;
            case Token.TokenType.Knight:
                DisableKnightToken(boardToken);
                break;
            case Token.TokenType.Siege:
                DisableSiegeToken(boardToken);
                break;
            case Token.TokenType.Barracks:
                DisableBarracksToken(boardToken);
                break;
            case Token.TokenType.Resource:
                DisableResourceToken(boardToken);
                break;
        }
    }

    public void SingleTargetToken(TokenCellPositioner tokenPositioner, int player)
    {
        if (player == 0)
        {
            foreach (TargetState targetState in AllTokensPlayerA())
            {
                if (!targetState.IsUsed || targetState.TokenImageTarget == tokenPositioner)
                {
                    continue;
                }

                targetState.TokenImageTarget.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (TargetState targetState in AllTokensPlayerB())
            {
                if (!targetState.IsUsed || targetState.TokenImageTarget == tokenPositioner)
                {
                    continue;
                }

                targetState.TokenImageTarget.gameObject.SetActive(false);
            }
        }
    }

    public void DisableAll()
    {
        foreach (TargetState targetState in AllTokensPlayerA())
        {
            targetState.TokenImageTarget.gameObject.SetActive(false);
        }
        foreach (TargetState targetState in AllTokensPlayerB())
        {
            targetState.TokenImageTarget.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerTurnManager.TurnChangeEvent += SwitchPlayerTokenTrackers;
    }

    private void OnDisable()
    {
        PlayerTurnManager.TurnChangeEvent -= SwitchPlayerTokenTrackers;
    }


    public void SwitchPlayerTokenTrackers(int player)
    {
        bool enableA = player == 0;
        bool enableB = player == 1;

        foreach (TargetState targetState in AllTokensPlayerA())
        {
            targetState.TokenImageTarget.gameObject.SetActive(targetState.IsUsed && enableA);
        }
        foreach (TargetState targetState in AllTokensPlayerB())
        {
            targetState.TokenImageTarget.gameObject.SetActive(targetState.IsUsed && enableB);
        }

        CentralHallPlayerA.AssignedTokenObject.GetComponentInChildren<GraphicRaycaster>().enabled = enableA;
        CentralHallPlayerB.AssignedTokenObject.GetComponentInChildren<GraphicRaycaster>().enabled = enableB;
    }
}
