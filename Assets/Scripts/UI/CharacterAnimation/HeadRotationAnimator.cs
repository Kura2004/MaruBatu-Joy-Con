using UnityEngine;
using DG.Tweening;
using System.Collections;

public class HeadRotationAnimator : MonoBehaviour
{
    private RectTransform rectTransform;

    // インスペクターで設定可能なパラメータ
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;  // 回転軸
    [SerializeField] private float[] rotationAngles;
    [SerializeField] private float duration = 1f;                // 補完時間
    [SerializeField] private Ease easing = Ease.InOutSine;       // イージング
    [SerializeField] private float delayBetweenLoops = 0.5f;     // 待機時間

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            for (int i = 0; i < rotationAngles.Length; i++)
            {
                // ターゲット回転を計算
                Vector3 targetRotation = rectTransform.localEulerAngles + rotationAxis * rotationAngles[i];

                // 回転アニメーションを実行
                rectTransform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360)
                             .SetEase(easing);

                // 回転が終わるまで待機
                yield return new WaitForSeconds(duration + delayBetweenLoops);
            }
        }
    }
}