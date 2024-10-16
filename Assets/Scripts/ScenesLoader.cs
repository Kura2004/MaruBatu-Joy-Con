using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// MonoBehaviourを継承したシングルトンなクラス（ScenesLoader用）
/// </summary>
public class ScenesLoader : SingletonMonoBehaviour<ScenesLoader>
{
    // シーンのロード時間を設定するためのenum
    public enum SceneLoadTime
    {
        Short = 1,    // 短いロード時間（秒）
        Medium = 2,   // 中程度のロード時間（秒）
        Long = 3      // 長いロード時間（秒）
    }

    [System.Serializable]
    public class SceneSettings
    {
        public string sceneName;
        public float loadDuration;  // ロード時間を少数第一位まで設定
    }

    [Header("シーンのロード時間設定")]
    [SerializeField] private SceneSettings[] sceneSettings;

    private bool isLocked = false; // ロック状態を管理する変数

    protected override void OnEnable()
    {
        // シーンがロードされたときに呼ばれるイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
        base.OnEnable();
    }

    private void OnDisable()
    {
        // イベントの登録解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンがロードされたときに呼ばれるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"シーン「{scene.name}」がロードされました。");

        // ロード後に操作を無効化する処理を開始
        float loadDuration = GetSceneLoadDuration(scene.name);
        StartCoroutine(DisableInputForDuration(loadDuration));

        // シーンごとに異なる初期化処理が必要な場合はここに追加
        if (scene.name == "StartMenu")
        {
            // StartMenuの初期化処理
        }
        else if (scene.name == "4×4")
        {
            // 4×4ステージの初期化処理
        }
        else if (scene.name == "GameOver")
        {
            // GameOverの初期化処理
        }
    }

    // 指定されたシーンのロード時間を取得
    private float GetSceneLoadDuration(string sceneName)
    {
        foreach (var setting in sceneSettings)
        {
            if (setting.sceneName == sceneName)
            {
                return setting.loadDuration;
            }
        }
        Debug.LogWarning($"指定されたシーン名「{sceneName}」の設定が見つかりません。デフォルトのロード時間を使用します。");
        return 1f; // デフォルトのロード時間
    }

    /// <summary>
    /// 指定された期間の間、操作を無効にするためのコルーチン.
    /// </summary>
    /// <param name="duration">無効化する時間（秒）</param>
    /// <returns></returns>
    private IEnumerator DisableInputForDuration(float duration)
    {
        // 操作を無効化
        isLocked = true;

        // 指定された期間だけ待機
        yield return new WaitForSeconds(duration + 0.1f);

        // 操作を有効化
        isLocked = false;
    }

    public void LoadStage44()
    {
        if (isLocked)
        {
            Debug.LogWarning("シーンのロード処理がロックされています。");
            ScenesAudio.BlockedSe();
            return;
        }
        isLocked = true;
        float loadDuration = GetSceneLoadDuration("4×4");
        FadeManager.Instance.LoadScene("4×4", loadDuration, Color.black);
        Debug.Log("Stage44を読み込みます");
    }

    public void LoadStartMenu()
    {
        if (isLocked)
        {
            Debug.LogWarning("シーンのロード処理がロックされています。");
            ScenesAudio.BlockedSe();
            return;
        }
        isLocked = true;
        float loadDuration = GetSceneLoadDuration("StartMenu");
        FadeManager.Instance.LoadScene("StartMenu", loadDuration, Color.black);
        Debug.Log("StartMenuを読み込みます");
    }

    public void LoadGameOver(Color fadeColor)
    {
        if (isLocked)
        {
            Debug.LogWarning("シーンのロード処理がロックされています。");
            ScenesAudio.BlockedSe();
            return;
        }
        isLocked = true;
        float loadDuration = GetSceneLoadDuration("GameOver");
        FadeManager.Instance.LoadScene("GameOver", loadDuration, fadeColor);
        Debug.Log("GameOverを読み込みます");
    }
}
