using UnityEngine;

public class GlobalColorManager : SingletonMonoBehaviour<GlobalColorManager>
{
    [SerializeField]
    public Color playerColor = new Color(0.5f, 0f, 0.5f);
    [SerializeField]
    public Color opponentColor = new Color(0.56f, 0.93f, 0.56f);
    public Color currentColor;

    public Color CurrentColor
    {
        get => currentColor;
        private set => currentColor = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Initialize();
    }

    private void Initialize()
    {
        CurrentColor = playerColor;
    }

    public void UpdateColorBasedOnTurn()
    {
        if (IsPlayerTurn())
        {
            CurrentColor = opponentColor;
        }
        else if (IsOpponentTurn())
        {
            CurrentColor = playerColor;
        }
        else
        {
            CurrentColor = Color.black;
        }
    }

    public bool IsPlayerTurn() =>
        GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup);

    public bool IsOpponentTurn() =>
        GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup);
}
