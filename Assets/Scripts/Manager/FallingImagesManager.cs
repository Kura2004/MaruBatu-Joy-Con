using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  // DOTweenの名前空間
using System.Collections;  // コルーチンを使用するために必要

public class FallingImagesManager : MonoBehaviour
{
    [SerializeField] private Image imagePrefab;   // ImageのPrefab
    [SerializeField] private float minX = -100f;  // 生成されるX座標の最小値
    [SerializeField] private float maxX = 100f;   // 生成されるX座標の最大値
    [SerializeField] private float startY = 200f; // 生成されるY座標
    [SerializeField] private float endY = -1000f;
    [SerializeField] private float fallDuration = 2f;  // 落下アニメーションの再生時間
    [SerializeField] private float rotateSpeed = 360f; // 回転速度（度/秒）
    [SerializeField] private int objectCount = 5;  // 生成するオブジェクトの総数
    [SerializeField] private float spawnInterval = 0.5f;  // 生成間隔
    [SerializeField] private float waitTimeAfterSpawn = 1f;  // 生成後の待機時間

    [SerializeField] private Transform parentTransform; // 親にするオブジェクトのTransform

    private void Start()
    {
        // オブジェクトの生成を開始
        StartGeneratingImages();
    }

    // オブジェクトの生成を開始する
    private void StartGeneratingImages()
    {
        // 一定時間ごとにオブジェクトを生成する
        InvokeRepeating(nameof(GenerateMultipleImages), 0f, spawnInterval);
    }

    // 一度に複数の画像を生成し、アニメーションを設定するメソッド
    private void GenerateMultipleImages()
    {
        // コルーチンを使って生成と待機を実行
        StartCoroutine(GenerateAndAnimateImage());
    }

    // 画像を生成し、アニメーションを設定するコルーチン
    private IEnumerator GenerateAndAnimateImage()
    {
        // ランダムなインデックス配列を生成
        int[] randomIndices = GenerateRandomIndices(objectCount);

        // 1回の生成で複数のオブジェクトを生成
        for (int i = 0; i < objectCount; i++)
        {
            // ランダムなインデックスを使用
            int randomIndex = randomIndices[i];

            // Imageのインスタンスを生成
            Image newImage = Instantiate(imagePrefab, parentTransform);

            RectTransform imageRectTransform = newImage.GetComponent<RectTransform>();

            // X座標を等間隔に設定（ランダムなインデックスを使用）
            //float r = Random.Range(-50f, 50f);
            float spacing = (maxX - minX) / objectCount;  // 等間隔の計算
            float baseX = minX + (spacing * randomIndex);  // ランダムなインデックスによるX座標

            imageRectTransform.anchoredPosition = new Vector2(baseX, startY); // Y座標は設定した値にランダムなオフセットを加えたもの

            // 一定時間待機
            yield return new WaitForSeconds(waitTimeAfterSpawn);

            // フェードアウトのために、透明な色にアニメーション
            newImage.DOColor(new Color(imagePrefab.color.r, imagePrefab.color.g, imagePrefab.color.b, 0f), fallDuration)
                .SetEase(Ease.Linear);

            // 落下と回転のアニメーションをDOTweenで実行
            AnimateFallingAndRotating(imageRectTransform);
        }
    }

    // DOTweenを使って落下と回転のアニメーションを実行するメソッド
    private void AnimateFallingAndRotating(RectTransform imageRectTransform)
    {
        // Y座標を変えつつ、時間を基準にアニメーション
        imageRectTransform.DOAnchorPosY(imageRectTransform.anchoredPosition.y + endY, fallDuration)
            .SetEase(Ease.Linear)  // 線形のスムーズなアニメーション
            .OnComplete(() =>
            {
                Destroy(imageRectTransform.gameObject);  // アニメーション完了後に削除
            });

        // 回転を回転速度に応じてfallDuration秒間でループさせる
        imageRectTransform.DORotate(new Vector3(0, 0, rotateSpeed * fallDuration), fallDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear);  // 線形のスムーズな回転
    }

    // ランダムなインデックス配列を生成するメソッド
    private int[] GenerateRandomIndices(int count)
    {
        int[] indices = new int[count];

        // 順番にインデックスを代入
        for (int i = 0; i < count; i++)
        {
            indices[i] = i;
        }

        // 配列をシャッフル
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, count);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        return indices;
    }
}
