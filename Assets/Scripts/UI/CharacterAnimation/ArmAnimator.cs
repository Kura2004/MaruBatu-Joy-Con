using UnityEngine;
using DG.Tweening;
using System.Collections;

public class ArmAnimator : MonoBehaviour
{
    private RectTransform rectTransform;

    // インスペクターで設定可能なパラメータ
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationAxis = Vector3.up;  // 回転軸
    [SerializeField] private float rotationAngle = 90f;          // 回転角度
    [SerializeField] private float duration = 1f;                // 補完時間
    [SerializeField] private Ease easing = Ease.InOutSine;       // イージング
    [SerializeField] private float delayBetweenLoops = 0.5f;     // 正負が切り替わる時の待機時間

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(RotateArm());
    }

    private IEnumerator RotateArm()
    {
        while (true)
        {
            Vector3 targetRotation = rectTransform.localEulerAngles + rotationAxis * rotationAngle;
            rectTransform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360)
                         .SetEase(easing);

            // 回転が終わるまで待機
            yield return new WaitForSeconds(duration + delayBetweenLoops);
            
            // 反対の方向に回転
            targetRotation = rectTransform.localEulerAngles - rotationAxis * rotationAngle;
            rectTransform.DOLocalRotate(targetRotation, duration, RotateMode.FastBeyond360)
                         .SetEase(easing);
            
            // 再度待機
            yield return new WaitForSeconds(duration + delayBetweenLoops);
        }
    }
}
