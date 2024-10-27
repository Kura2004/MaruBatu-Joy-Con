using UnityEngine;
using DG.Tweening;

public class ArmRotationAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform targetRectTransform;
    [SerializeField] private float rotationAngle = 90f;  // 回転する角度
    [SerializeField] private float duration = 1f;        // 補完時間 (秒)
    [SerializeField] private Ease easeType = Ease.Linear; // イージングタイプ


    // Z軸の回転を設定したベクトルの長さをn秒で補完的に変化させるメソッド
    public void RotateZAxis(bool isPositive)
    {
        float targetAngle = isPositive ? rotationAngle : -rotationAngle;

        // 現在の回転角度を取得し、Z軸のみを設定
        Vector3 currentRotation = targetRectTransform.localEulerAngles;
        float newZRotation = currentRotation.z + targetAngle;

        // Z軸の回転を補完的に変化
        targetRectTransform.DOLocalRotate(new Vector3(0, 0, newZRotation), duration)
            .SetEase(easeType);
    }

    // 回転を反転させるメソッド
    public void ToggleRotationDirection()
    {
        // 現在の回転方向を判定し、反転させて回転
        bool isCurrentlyPositive = targetRectTransform.localEulerAngles.z % 360 < 180;
        RotateZAxis(!isCurrentlyPositive);
    }
}
