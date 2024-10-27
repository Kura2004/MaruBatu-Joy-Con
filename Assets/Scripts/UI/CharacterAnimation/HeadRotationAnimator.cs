using UnityEngine;
using DG.Tweening;

public class HeadRotationAnimator : MonoBehaviour
{
    [SerializeField] private RectTransform headRectTransform;
    [SerializeField] private float headRotationAngle = 90f;  // 回転する角度
    [SerializeField] private float headRotationDuration = 1f;  // 補完時間 (秒)
    [SerializeField] private Ease headEaseType = Ease.Linear;  // イージングタイプ

    // Z軸の回転を設定したベクトルの長さをn秒で補完的に変化させるメソッド
    public void RotateHeadZAxis(bool isPositive)
    {
        float targetAngle = isPositive ? headRotationAngle : -headRotationAngle;

        // 現在の回転角度を取得し、Z軸のみを設定
        Vector3 currentRotation = headRectTransform.localEulerAngles;
        float newZRotation = currentRotation.z + targetAngle;

        // Z軸の回転を補完的に変化
        headRectTransform.DOLocalRotate(new Vector3(0, 0, newZRotation), headRotationDuration)
            .SetEase(headEaseType);
    }

    // 回転を反転させるメソッド
    public void ToggleHeadRotationDirection()
    {
        // 現在の回転方向を判定し、反転させて回転
        bool isCurrentlyPositive = headRectTransform.localEulerAngles.z % 360 < 180;
        RotateHeadZAxis(!isCurrentlyPositive);
    }
}
