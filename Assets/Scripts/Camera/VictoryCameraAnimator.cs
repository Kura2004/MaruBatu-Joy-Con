using UnityEngine;
using DG.Tweening;

/// <summary>
/// 勝利演出時のカメラ移動を制御するクラス
/// </summary>
public class VictoryCameraAnimator : SingletonMonoBehaviour<VictoryCameraAnimator>
{
    [SerializeField]
    private Camera mainCamera;  // インスペクターで設定できるカメラ

    [SerializeField]
    private float moveDuration = 1.0f;  // カメラが移動する時間
    [SerializeField]
    private Ease moveEase = Ease.OutBounce;  // 勝利演出用のイージング
    [SerializeField]
    private float xCameraOffset = 5.0f;  // カメラのX軸に沿った移動量（インスペクターで設定）

    private bool hasAnimated = false;  // アニメーションが実行されたかどうかを管理するフラグ

    [SerializeField]
    private RectTransform playerImage1P;  // プレイヤー画像1のRectTransform
    [SerializeField]
    private RectTransform playerImage2P;  // プレイヤー画像2のRectTransform

    [SerializeField] float xImageOffset = 5.0f;
    [SerializeField] float adjustOffset = 5.0f;

    [SerializeField] LightningAnimator lightningAnimator;

    [SerializeField] UIObjectShaker shaker1P;
    [SerializeField] UIObjectShaker shaker2P;

    [SerializeField] PlayerImageAnimator animator1P;
    [SerializeField] PlayerImageAnimator animator2P;

    [SerializeField] float delayDuration = 0;
    private void Start()
    {
        hasAnimated = false;
    }

    /// <summary>
    /// カメラを右に補完的に移動させて、勝利を演出する.
    /// </summary>
    public void MoveCameraRightForVictory()
    {
        if (!hasAnimated)  // アニメーションが未実行の場合のみ
        {
            if (mainCamera != null)
            {
                // 現在のカメラ位置を取得
                Vector3 currentPosition = mainCamera.transform.position;
                Vector3 targetPosition = currentPosition + new Vector3(xCameraOffset, 0, 0);

                // DOTweenを使って補完的に移動
                mainCamera.transform.DOMove(targetPosition, moveDuration).SetEase(moveEase)
                                        .OnComplete(() =>
                                        {
                                            lightningAnimator.StartLightningAnimationBlue();
                                            shaker2P.ShakeUIElement();

                                            DOVirtual.DelayedCall(delayDuration,
                                                () => ScenesLoader.Instance.LoadGameOver(Color.white));
                                        });

                // playerImage1Pをローカル座標で左に補完的に移動
                if (playerImage1P != null)
                {
                    playerImage1P.DOLocalMoveX(playerImage1P.localPosition.x - xImageOffset - adjustOffset,
                        moveDuration).SetEase(moveEase);
                }

                // playerImage2Pをローカル座標で右に補完的に移動
                if (playerImage2P != null)
                {
                    playerImage2P.DOLocalMoveX(playerImage2P.localPosition.x - xImageOffset ,
                        moveDuration).SetEase(moveEase);
                }

                animator1P.ChangeSpritesColor(Color.gray, 0.3f);
                animator2P.ChangeSpritesColor(Color.clear, 0.3f);
                animator2P.OnWinImages();
                animator2P.ChangeWinSpritesColor(Color.white, 0.3f);

                hasAnimated = true;  // アニメーション実行済みに設定
            }
            else
            {
                Debug.LogWarning("カメラが設定されていません。");
            }
        }
        else
        {
            Debug.LogWarning("アニメーションはすでに実行されました。");
        }
    }

    /// <summary>
    /// カメラを左に補完的に戻して、勝利演出をリセット.
    /// </summary>
    public void MoveCameraLeftToResetVictory()
    {
        if (!hasAnimated)  // アニメーションが未実行の場合のみ
        {
            if (mainCamera != null)
            {
                // 現在のカメラ位置を取得
                Vector3 currentPosition = mainCamera.transform.position;
                Vector3 targetPosition = currentPosition - new Vector3(xCameraOffset, 0, 0);

                // DOTweenを使って補完的に戻す
                mainCamera.transform.DOMove(targetPosition, moveDuration).SetEase(moveEase)
                    .OnComplete(() =>
                    {
                        lightningAnimator.StartLightningAnimationRed();
                        shaker1P.ShakeUIElement();

                        DOVirtual.DelayedCall(delayDuration,
                            () => ScenesLoader.Instance.LoadGameOver(Color.white));
                    });

                // playerImage1Pをローカル座標で左に補完的に移動
                if (playerImage1P != null)
                {
                    playerImage1P.DOLocalMoveX(playerImage1P.localPosition.x + xImageOffset ,
                        moveDuration).SetEase(moveEase);
                }

                // playerImage2Pをローカル座標で右に補完的に移動
                if (playerImage2P != null)
                {
                    playerImage2P.DOLocalMoveX(playerImage2P.localPosition.x + xImageOffset + adjustOffset,
                        moveDuration).SetEase(moveEase);
                }

                animator2P.ChangeSpritesColor(Color.gray, 0.3f);
                animator1P.ChangeSpritesColor(Color.clear, 0.3f);
                animator1P.OnWinImages();
                animator1P.ChangeWinSpritesColor(Color.yellow, 0.3f);

                hasAnimated = true;  // アニメーション実行済みに設定
            }
            else
            {
                Debug.LogWarning("カメラが設定されていません。");
            }
        }
        else
        {
            Debug.LogWarning("アニメーションはすでに実行されました。");
        }
    }

    /// <summary>
    /// 手動でカメラを設定するメソッド.
    /// </summary>
    /// <param name="camera">設定するカメラ</param>
    public void SetVictoryCamera(Camera camera)
    {
        mainCamera = camera;
        Debug.Log("勝利演出用のカメラが設定されました: " + camera.name);
    }

    /// <summary>
    /// アニメーションフラグをリセットするメソッド（必要に応じて呼び出す）.
    /// </summary>
    public void ResetAnimationFlag()
    {
        hasAnimated = false;
    }
}
