using UnityEngine;
using DG.Tweening;

public class CanvasBounce : MonoBehaviour
{
    [SerializeField] protected RectTransform canvasRectTransform;
    [SerializeField] protected GameObject canvasObject; // アニメーションをかけるキャンバスのGameObjectを追加
    [SerializeField] protected float initialDropHeight = 1000f; // キャンバスが落ち始める高さ
    [SerializeField] protected float groundY = -500f; // 地面のY座標
    [SerializeField] protected float bounceHeight = 200f; // 初期の弾む高さ
    [SerializeField] protected int bounceCount = 3; // 弾む回数
    [SerializeField] protected float initialBounceDuration = 0.5f; // 初期の弾むアニメーションの時間
    [SerializeField] protected float heightDampingFactor = 0.5f; // 高さの減衰率
    [SerializeField] protected float durationDampingFactor = 0.7f; // 時間の減衰率
    [SerializeField] protected float riseDuration = 0.3f; // 上昇アニメーションの時間
    [SerializeField] protected bool dropOnStart = false; // 最初にDropCanvasを呼び出すかどうかのフラグ

    protected bool isFalling = false;
    protected bool isBouncingComplete = true; // バウンドアニメーションの完了フラグ
    public static bool isBlocked = false;

    protected virtual void Start()
    {
        if (dropOnStart)
        {
            InitializeCanvasPosition();
            isFalling = false; // 落下フラグをリセット
            isBouncingComplete = true; // バウンドアニメーションが完了したフラグを設定
        }

        else
        {
            // キャンバスのGameObjectを非アクティブに設定
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

    protected virtual void Update()
    {
        // キャンバスが落下する条件
        if (ShouldDropCanvas())
        {
            //DropCanvas();
            Debug.Log("キャンバスが落下します");
        }

        // バウンドが完了している場合、かつ Q キーが押されたときにキャンバスを上昇させる
        if (Input.GetKeyDown(KeyCode.Q) && !isFalling && isBouncingComplete)
        {
            RiseCanvas();

            if (dropOnStart)
            {
                GameTurnManager.Instance.IsGameStarted = true;
                GameStateManager.Instance.StartBoardSetup(1.6f);
                TimeLimitController.Instance.ResetTimer();
                TimeLimitController.Instance.StopTimer();
                dropOnStart = false;
            }

            Debug.Log("キャンバスが上昇します");
        }
    }

    protected virtual bool ShouldDropCanvas()
    {
        return Input.GetKeyDown(KeyCode.Q) && !isFalling && canvasRectTransform.anchoredPosition.y != groundY;
    }

    protected void InitializeDrop()
    {
        isFalling = true;
        isBouncingComplete = false; // バウンドアニメーションのフラグをリセット
        isBlocked = true;
    }

    protected virtual void DropCanvas()
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

    protected virtual void Bounce()
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

    protected virtual void RiseCanvas()
    {
        if (!isFalling)
        {
            // キャンバスを地面の位置に設定
            canvasRectTransform.anchoredPosition = new Vector2(canvasRectTransform.anchoredPosition.x, groundY);

            // 上昇アニメーション
            canvasRectTransform.DOAnchorPosY(initialDropHeight, riseDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // アニメーション完了後、キャンバスを非アクティブに設定
                canvasObject.SetActive(false);
            });
        }
        isBlocked = false;

        if (!TimeControllerToggle.isTimeStopped && !TimeLimitController.Instance.isTimerRunning)
        {
            TimeLimitController.Instance.StartTimer();
        }
    }
}
