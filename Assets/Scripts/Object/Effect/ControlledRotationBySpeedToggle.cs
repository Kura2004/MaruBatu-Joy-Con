using UnityEngine;

public class ControlledRotationBySpeedToggle : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField, Tooltip("通常の回転速度 (度/秒)")]
    private float normalRotationSpeed = 10f;  // 通常時の回転速度

    [SerializeField, Tooltip("速く回転するときの速度 (度/秒)")]
    private float fastRotationSpeed = 50f;    // フラグがオンのときの回転速度

    [SerializeField, Tooltip("回転速度を速くするかどうかを決定するフラグ")]
    public bool isSpeedIncreased;    // 回転速度を変更するフラグ

    [SerializeField, Tooltip("どの軸で回転するか")]
    private RotationAxis rotationAxis = RotationAxis.Y;  // 回転する軸、デフォルトはY

    private float currentRotationSpeed;
    private bool isFastRotating = false; // 高速回転状態を管理する変数

    void OnEnable()
    {
        currentRotationSpeed = normalRotationSpeed;  // 初期の回転速度を設定
        isSpeedIncreased = false;
    }

    private void OnMouseEnter()
    {
        isSpeedIncreased = true;
    }

    private void OnMouseExit()
    {
        isSpeedIncreased = false;
    }

    void Update()
    {
        // フラグに応じて回転速度を設定
        currentRotationSpeed = isSpeedIncreased ? fastRotationSpeed : normalRotationSpeed;

        // 指定された軸で回転
        Vector3 rotationVector = GetRotationVector();

        if (!TimeControllerToggle.isTimeStopped)
        {
            if (isFastRotating)
            {
                // Z軸で高速回転
                rotationVector = Vector3.forward;
                currentRotationSpeed = fastRotationSpeed;
            }

            transform.Rotate(rotationVector * currentRotationSpeed * Time.deltaTime);
        }
    }

    private Vector3 GetRotationVector()
    {
        switch (rotationAxis)
        {
            case RotationAxis.X:
                return Vector3.right;
            case RotationAxis.Y:
                return Vector3.up;
            case RotationAxis.Z:
                return Vector3.forward;
            default:
                return Vector3.up;
        }
    }

    // 回転軸を設定するメソッド
    public void SetRotationAxis(RotationAxis newAxis)
    {
        rotationAxis = newAxis;
    }

    // 高速でZ軸回転するメソッド
    public void SetFastRotation(bool isFast)
    {
        isFastRotating = isFast;
    }
}
