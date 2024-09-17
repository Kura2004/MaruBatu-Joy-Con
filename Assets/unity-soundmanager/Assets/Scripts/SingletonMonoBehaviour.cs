using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// MonoBehaviourを継承したシングルトンなクラス（基底クラス）
/// </summary>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// インスタンス
    /// </summary>
    private static T _instance;

    /// <summary>
    /// インスタンスのゲッター
    /// </summary>
    public static T Instance
    {
        get
        {
            // インスタンスのnullチェック(初回起動時)
            if (_instance == null)
            {
                T[] instances = FindObjectsByType<T>(FindObjectsSortMode.None);

                // インスタンスが存在なし
                if (instances.Length == 0)
                {
                    Debug.LogError(typeof(T) + " インスタンスはありません。アタッチし忘れていませんか？");
                    return null;
                }
                // インスタンスが複数個存在している
                else if (instances.Length >= 2)
                {
                    Debug.LogWarning(typeof(T) + " インスタンスが複数個生成されています。余分なインスタンスを削除します。");

                    // 最初のインスタンスを保持し、それ以外を削除
                    _instance = instances[0];
                    for (int i = 1; i < instances.Length; i++)
                    {
                        Destroy(instances[i].gameObject);
                    }
                }
                // インスタンスが1個存在している(正常)
                else
                {
                    _instance = instances[0];
                    Debug.Log(typeof(T) + " インスタンスが1個生成されています。正常です。");
                }
            }

            return _instance;
        }
    }

    /// <summary>
    /// シーン切り替え時に破棄するかどうか
    /// </summary>
    [SerializeField]
    private bool destroyOnSceneChange = true;

    protected virtual void OnEnable()
    {
        SetInstance();
    }

    void SetInstance()
    {
        // インスタンスが設定されていない場合、設定する
        if (_instance == null)
        {
            _instance = this as T;
            if (!destroyOnSceneChange)
            {
                DontDestroyOnLoad(this.gameObject); // シングルトンをシーン遷移で破壊されないようにする
            }
        }
        else
        {
            // インスタンスが既に存在し、別のインスタンスが作られた場合はそれを削除する
            if (_instance != this)
            {
                Destroy(this.gameObject);
                Debug.LogWarning(typeof(T) + " インスタンスが複数個生成されています。余分なインスタンスを削除します。");
            }
        }
    }

    private void OnDisable()
    {
        if (destroyOnSceneChange)
        {
            Destroy(gameObject); // OnDisableが呼ばれた時にオブジェクトを破棄
            Debug.Log("OnDisableが呼ばれ、インスタンスが破棄されました");
        }
    }
}
