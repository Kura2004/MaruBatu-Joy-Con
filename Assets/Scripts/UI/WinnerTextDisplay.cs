using UnityEngine;
using TMPro;  // TextMeshPro用の名前空間をインポート

public class WinnerTextDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText; // TextMeshProのテキストフィールド

    private void Start()
    {
        // GameWinnerManagerの現在の勝者に応じてテキストを変更
        switch (GameWinnerManager.Instance.CurrentWinner)
        {
            case GameWinnerManager.Winner.Player1:
                winnerText.text = "1P Win!";
                break;

            case GameWinnerManager.Winner.Player2:
                winnerText.text = "2P Win!";
                break;

            case GameWinnerManager.Winner.Draw:
                winnerText.text = "Draw";
                break;

            case GameWinnerManager.Winner.None:
            default:
                winnerText.text = "No Winner Yet";
                break;
        }
    }
}
