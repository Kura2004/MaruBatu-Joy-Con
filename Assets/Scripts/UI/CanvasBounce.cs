using UnityEngine;
using DG.Tweening;

public class CanvasBounce : MonoBehaviour
{
    [SerializeField] protected RectTransform canvasRectTransform;
    [SerializeField] protected GameObject canvasObject;
    [SerializeField] protected float initialDropHeight = 1000f;
    [SerializeField] protected float groundY = -500f;
    [SerializeField] protected float bounceHeight = 200f;
    [SerializeField] protected int bounceCount = 3;
    [SerializeField] protected float initialBounceDuration = 0.5f;
    [SerializeField] protected float heightDampingFactor = 0.5f;
    [SerializeField] protected float durationDampingFactor = 0.7f;
    [SerializeField] protected float riseDuration = 0.3f;
    [SerializeField] protected bool dropOnStart = false;

    protected bool isFalling = false;
    protected bool isBouncingComplete = true;

    private Joycon leftJoycon;
    private Joycon rightJoycon;

    [SerializeField] CountdownText countdown;
    void Start()
    {
        // Joy-Conの初期化
        var joycons = JoyconManager.Instance.j;
        if (joycons == null) return;

        if (joycons.Count >= 2)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }

        if (dropOnStart)
        {
            InitializeCanvasPosition();
            isFalling = false; // 落下フラグをリセット
            isBouncingComplete = true; // バウンドアニメーションが完了したフラグを設定
        }
        else
        {
            canvasObject.SetActive(false);
        }
    }

    void InitializeCanvasPosition()
    {
        InitializeDrop();
        Vector3 setPos = canvasRectTransform.localPosition;
        setPos.y = groundY;
        canvasRectTransform.localPosition = setPos;
    }

    void Update()
    {
        if (ShouldDropCanvas())
        {
            Debug.Log("キャンバスが落下します");
        }

        // Lボタンでキャンバスを上昇させる
        if (leftJoycon != null && 
            (leftJoycon.GetButtonDown(Joycon.Button.DPAD_DOWN)) && !isFalling && isBouncingComplete)
        {
            RiseCanvas();

            if (dropOnStart)
            {
                GameStateManager.Instance.StartBoardSetup(countdown.GetTotalDuration());
                StartCoroutine(countdown.StartCountdown());
                TimeLimitController.Instance.ResetTimer();
                TimeLimitController.Instance.StopTimer();
                dropOnStart = false;
            }

            Debug.Log("キャンバスが上昇します");
        }

        if (leftJoycon != null && leftJoycon.GetButtonDown(Joycon.Button.DPAD_LEFT) && dropOnStart)
        {
            ScenesLoader.Instance.LoadStartMenu();
            Debug.Log("スタート画面に戻ります");
        }
    }

    bool ShouldDropCanvas()
    {
        return false;
    }

    void InitializeDrop()
    {
        isFalling = true;
        isBouncingComplete = false; // バウンドアニメーションのフラグをリセット
    }

    void DropCanvas()
    {
        InitializeDrop();

        // キャンバスをアクティブにする
        canvasObject.SetActive(true);

        TimeLimitController.Instance.StopTimer();

        // キャンバスを初期の高さに設定
        canvasRectTransform.anchoredPosition = new Vector2(canvasRectTransform.anchoredPosition.x, initialDropHeight);

        // 落下アニメーション
        canvasRectTransform.DOAnchorPosY(groundY, initialBounceDuration).SetEase(Ease.InQuad).OnComplete(Bounce);
    }

    void Bounce()
    {
        float currentBounceHeight = bounceHeight;
        float currentBounceDuration = initialBounceDuration;
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < bounceCount; i++)
        {
            // バウンドアニメーションが終わった後に特定のメソッドを呼ぶ
            sequence.AppendCallback(() => ScenesAudio.FallSe());

            sequence.Append(canvasRectTransform.DOAnchorPosY(groundY + currentBounceHeight, currentBounceDuration).SetEase(Ease.OutQuad));
            sequence.Append(canvasRectTransform.DOAnchorPosY(groundY, currentBounceDuration).SetEase(Ease.InQuad));

            // 弾む高さと時間を減衰させる
            currentBounceHeight *= heightDampingFactor;
            currentBounceDuration *= durationDampingFactor;
        }

        sequence.OnComplete(() =>
        {
            isFalling = false; // 落下フラグをリセット
            isBouncingComplete = true; // バウンドアニメーションが完了したフラグを設定
            ScenesAudio.FallSe();
        });

        sequence.Play();
    }

    void RiseCanvas()
    {
        if (!isFalling)
        {
            // キャンバスを地面の位置に設定
            canvasRectTransform.anchoredPosition = new Vector2(canvasRectTransform.anchoredPosition.x, groundY);

            // 上昇アニメーション
            canvasRectTransform.DOAnchorPosY(initialDropHeight, riseDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                if (dropOnStart)
                    Destroy(this);
                // アニメーション完了後、キャンバスを非アクティブに設定
                canvasObject.SetActive(false);

            });
        }
    }
}
