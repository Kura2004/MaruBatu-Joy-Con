using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerLoserImageDisplay : MonoBehaviour
{
    // 1Pの勝者、敗者に対応するImageのリスト
    [SerializeField] private List<Image> player1WinnerImages; // 1P勝者のImageリスト
    [SerializeField] private List<Image> player1LoserImages;  // 1P敗者のImageリスト

    // 2Pの勝者、敗者に対応するImageのリスト
    [SerializeField] private List<Image> player2WinnerImages; // 2P勝者のImageリスト
    [SerializeField] private List<Image> player2LoserImages;  // 2P敗者のImageリスト

    // 引き分け時のImageのリスト
    [SerializeField] private List<Image> player1DrawImages; // 1P引き分け時のImageリスト
    [SerializeField] private List<Image> player2DrawImages; // 2P引き分け時のImageリスト

    private void Start()
    {

        DisableAllImages(); // 最初に全ての画像を非表示にする

        // 現在の勝者に応じて表示する画像を切り替える
        switch (GameWinnerManager.Instance.CurrentWinner)
        {
            case GameWinnerManager.Winner.Player1:
                SetPlayerImages(player1WinnerImages, true);  // 1P勝者の画像を表示
                SetPlayerImages(player2LoserImages, true);   // 2P敗者の画像を表示
                break;

            case GameWinnerManager.Winner.Player2:
                SetPlayerImages(player2WinnerImages, true);  // 2P勝者の画像を表示
                SetPlayerImages(player1LoserImages, true);   // 1P敗者の画像を表示
                break;

            case GameWinnerManager.Winner.Draw:
                SetDrawImages(); // 引き分け時の画像を表示
                break;

            case GameWinnerManager.Winner.None:
            default:
                // すべての画像を非表示にしたまま
                break;
        }
    }

    /// <summary>
    /// 指定したプレイヤーのImageリストを表示する
    /// </summary>
    private void SetPlayerImages(List<Image> images, bool show)
    {
        foreach (var image in images)
        {
            image.gameObject.SetActive(show);
        }
    }

    /// <summary>
    /// 引き分け時のImageを表示する
    /// </summary>
    private void SetDrawImages()
    {
        SetPlayerImages(player1DrawImages, true);
        SetPlayerImages(player2DrawImages, true);
    }

    /// <summary>
    /// すべてのImageを非表示にする
    /// </summary>
    private void DisableAllImages()
    {
        SetPlayerImages(player1WinnerImages, false);
        SetPlayerImages(player1LoserImages, false);
        SetPlayerImages(player1DrawImages, false);

        SetPlayerImages(player2WinnerImages, false);
        SetPlayerImages(player2LoserImages, false);
        SetPlayerImages(player2DrawImages, false);
    }
}