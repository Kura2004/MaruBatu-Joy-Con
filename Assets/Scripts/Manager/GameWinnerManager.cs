using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 勝者を管理するクラス
/// </summary>
public class GameWinnerManager : SingletonMonoBehaviour<GameWinnerManager>
{
    public enum Winner
    {
        None,     // 勝者なし
        Player1,  // 1P
        Player2,  // 2P
        Draw      // 引き分け
    }

    private Winner _currentWinner = Winner.None;

    public Winner CurrentWinner
    {
        get { return _currentWinner; }
    }

    /// <summary>
    /// 勝者を設定するメソッド
    /// </summary>
    public void SetWinner(Winner winner)
    {
        _currentWinner = winner;
        Debug.Log("勝者が設定されました: " + winner.ToString());
    }

    /// <summary>
    /// 指定したWinnerが現在の勝者と一致するか確認するメソッド
    /// </summary>
    public bool IsCurrentWinner(Winner winner)
    {
        return _currentWinner == winner;
    }

    /// <summary>
    /// 勝者情報をリセットするメソッド
    /// </summary>
    public void ResetWinner()
    {
        _currentWinner = Winner.None;
        Debug.Log("勝者情報がリセットされました");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded; // シーンが読み込まれた時のイベントを登録
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // イベントの解除
    }

    /// <summary>
    /// シーンが読み込まれた時に呼ばれるメソッド
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン名が "4×4" だった場合、勝者情報をリセットする
        if (scene.name == "4×4")
        {
            ResetWinner();
        }
    }
}
