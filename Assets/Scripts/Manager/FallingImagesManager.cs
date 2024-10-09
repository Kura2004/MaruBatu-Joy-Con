using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // DOTweenの名前空間

public class FallingImagesManager : MonoBehaviour
{
    // オブジェクトの生成を開始する
    private void StartGeneratingImages()
    {
        // 一定時間ごとにオブジェクトを生成する
        InvokeRepeating(nameof(GenerateMultipleImages), 0f, spawnInterval);
    }

    [SerializeField] private Image imagePrefab;   // ImageのPrefab
    [SerializeField] private float minX = -100f;  // 生成されるX座標の最小値
    [SerializeField] private float maxX = 100f;   // 生成されるX座標の最大値
    [SerializeField] private float startY = 200f; // 生成されるY座標
    [SerializeField] private float fallDuration = 2f;  // 落下アニメーションの再生時間
    [SerializeField] private float rotateSpeed = 360f; // 回転速度（度/秒）
    [SerializeField] private int objectCount = 5;  // 生成するオブジェクトの総数
    [SerializeField] private float spawnInterval = 0.5f;  // 生成間隔

    [SerializeField] private Transform parentTransform; // 親にするオブジェクトのTransform

    private int currentImageIndex = 0;  // 現在の画像インデックス

    private void Start()
    {
        // オブジェクトの生成を開始
        StartGeneratingImages();
    }

    // 一度に複数の画像を生成し、アニメーションを設定するメソッド
    private void GenerateMultipleImages()
    {
        // 1回の生成で複数のオブジェクトを生成
        for (int i = 0; i < objectCount; i++)
        {
            GenerateAndAnimateImage();
        }
    }

    // 画像を生成し、アニメーションを設定するメソッド
    private void GenerateAndAnimateImage()
    {
        // Imageのインスタンスを生成
        Image newImage = Instantiate(imagePrefab, parentTransform);

        RectTransform imageRectTransform = newImage.GetComponent<RectTransform>();

        // X座標を等間隔に設定
        float spacing = (maxX - minX) / objectCount + Random.Range(-50f, 50f);  // 等間隔の計算
        float baseX = minX + (spacing * (currentImageIndex % objectCount));  // 基準となるX座標
        float randomYOffset = Random.Range(-100f, 100f);  // Y座標に足すランダムな値

        imageRectTransform.anchoredPosition = new Vector2(baseX, startY + randomYOffset); // Y座標は設定した値にランダムなオフセットを加えたもの

        // フェードアウトのために、透明な色にアニメーション
        newImage.DOColor(new Color(imagePrefab.color.r, imagePrefab.color.g, imagePrefab.color.b, 0f), fallDuration)
            .SetEase(Ease.Linear);

        // 落下と回転のアニメーションをDOTweenで実行
        AnimateFallingAndRotating(imageRectTransform);

        currentImageIndex++; // インデックスを増やす
    }



    // DOTweenを使って落下と回転のアニメーションを実行するメソッド
    private void AnimateFallingAndRotating(RectTransform imageRectTransform)
    {

        // Y座標を変えつつ、時間を基準にアニメーション
        imageRectTransform.DOAnchorPosY(imageRectTransform.anchoredPosition.y - 600f, fallDuration)
            .SetEase(Ease.Linear)  // 線形のスムーズなアニメーション
            .OnComplete(() =>
            {
                Destroy(imageRectTransform.gameObject);  // アニメーション完了後に削除
                currentImageIndex--;
            });

        // 回転を回転速度に応じてfallDuration秒間でループさせる
        imageRectTransform.DORotate(new Vector3(0, 0, rotateSpeed * fallDuration), fallDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear);  // 線形のスムーズな回転
    }

}
