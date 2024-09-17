using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshProを使用するための名前空間

public class TimeLimitAdjuster : MonoBehaviour
{
    [SerializeField] private Button button10SecPlus;  // 10秒＋ボタン
    [SerializeField] private Button button10SecMinus; // 10秒マイナスボタン
    [SerializeField] private Button button1SecPlus;   // 1秒＋ボタン
    [SerializeField] private Button button1SecMinus;  // 1秒マイナスボタン
    [SerializeField] private TMP_Text valueDisplay; // TextMeshProを使用するテキスト

    public static float timeLimit = 30f; // 調整するゲームの制限時間

    [System.Obsolete]
    private void Awake()
    {
        // 他のインスタンスが既に存在する場合は、現在のインスタンスを破棄する
        if (FindObjectsOfType<TimeLimitAdjuster>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); // シーン切り替え時にオブジェクトを維持
        }
    }

    private void Start()
    {
        // ボタンのクリックイベントにリスナーを追加
        button10SecPlus.onClick.AddListener(() => AdjustTimeLimit(10));   // 10秒＋ボタンのリスナーを追加
        button10SecMinus.onClick.AddListener(() => AdjustTimeLimit(-10)); // 10秒マイナスボタンのリスナーを追加
        button1SecPlus.onClick.AddListener(() => AdjustTimeLimit(1));     // 1秒＋ボタンのリスナーを追加
        button1SecMinus.onClick.AddListener(() => AdjustTimeLimit(-1));   // 1秒マイナスボタンのリスナーを追加

        // 初期値を表示
        UpdateValueDisplay();
    }

    private void AdjustTimeLimit(int amount)
    {
        timeLimit += amount;
        timeLimit = Mathf.Clamp(timeLimit, 10f, 60f); // 最小値10、最大値60に制限
        UpdateValueDisplay();
    }

    private void UpdateValueDisplay()
    {
        valueDisplay.text = timeLimit.ToString("F0") + "s"; // 整数で表示
    }
}



