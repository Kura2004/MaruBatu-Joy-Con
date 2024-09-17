using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ColorTransitionEffect : MonoBehaviour
{
    // 色をタグ付けするためのenum
    public enum ColorTag
    {
        Red,
        Green,
        Blue,
        Yellow,
        White,
        Black,
        Gray,
        Cyan,
        Orange
    }

    [SerializeField]
    private float fadeDuration = 2f; // 色が変化するまでの時間（秒）

    // イージングを設定可能に
    [SerializeField]
    private Ease easeType = Ease.Linear;

    // 色をインスペクターで編集できるようにする
    [SerializeField]
    private Color redColor = Color.red;
    [SerializeField]
    private Color greenColor = Color.green;
    [SerializeField]
    private Color blueColor = Color.blue;
    [SerializeField]
    private Color yellowColor = Color.yellow;
    [SerializeField]
    private Color whiteColor = Color.white;
    [SerializeField]
    private Color blackColor = Color.black;
    [SerializeField]
    private Color grayColor = Color.gray;      // 灰色
    [SerializeField]
    private Color cyanColor = Color.cyan;      // 水色
    [SerializeField]
    private Color orangeColor = new Color(1f, 0.5f, 0f); // オレンジ色

    private Dictionary<ColorTag, Color> colorDictionary;

    private SpriteRenderer spriteRenderer; // SpriteRendererの参照

    private void Awake()
    {
        // SpriteRendererコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component is missing on the GameObject.");
        }

        // 色の辞書を初期化
        colorDictionary = new Dictionary<ColorTag, Color>()
        {
            { ColorTag.Red, redColor },
            { ColorTag.Green, greenColor },
            { ColorTag.Blue, blueColor },
            { ColorTag.Yellow, yellowColor },
            { ColorTag.White, whiteColor },
            { ColorTag.Black, blackColor },
            { ColorTag.Gray, grayColor },
            { ColorTag.Cyan, cyanColor },
            { ColorTag.Orange, orangeColor }
        };
    }

    private void OnEnable()
    {
        onEffect = false;
        StartColorTransition(ColorTag.Gray);
    }

    /// <summary>
    /// 指定した色に変化させるメソッド
    /// </summary>
    /// <param name="colorTag">変化させる色のタグ</param>
    public void StartColorTransition(ColorTag colorTag)
    {
        if (spriteRenderer != null && colorDictionary.ContainsKey(colorTag))
        {
            // 現在の色から指定した色までDoTweenで補完的に変化させる
            Color targetColor = colorDictionary[colorTag];
            spriteRenderer.DOColor(targetColor, fadeDuration)
                          .SetEase(easeType);
        }
        else
        {
            Debug.LogWarning("Invalid color tag or missing SpriteRenderer.");
        }
    }

    /// <summary>
    /// カラータグと色の辞書に新しい色を追加するメソッド
    /// </summary>
    /// <param name="colorTag">新しい色のタグ</param>
    /// <param name="color">追加する色</param>
    public void AddColor(ColorTag colorTag, Color color)
    {
        if (!colorDictionary.ContainsKey(colorTag))
        {
            colorDictionary.Add(colorTag, color);
        }
        else
        {
            Debug.LogWarning("ColorTag already exists.");
        }
    }

    bool onEffect = false;
    [SerializeField] bool setOrange = false;

    private void Update()
    {
        var turnMana = GameTurnManager.Instance;
        if ((turnMana.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup) ||
            turnMana.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup))
            && !onEffect)
        {
            onEffect = true;
            if (setOrange)
            {
                StartColorTransition(ColorTag.Orange);
            }

            else
            {
                StartColorTransition(ColorTag.Cyan);
            }
        }

        if ((turnMana.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) ||
            turnMana.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece))
            && onEffect)
        {
            onEffect = false;
            StartColorTransition(ColorTag.Gray);
        }
    }
}
