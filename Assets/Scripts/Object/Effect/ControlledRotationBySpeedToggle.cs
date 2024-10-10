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

    [SerializeField, Tooltip("どの軸で回転するか")]
    private RotationAxis rotationAxis = RotationAxis.Y;  // 回転する軸、デフォルトはY

    private Vector3 rotationVector;

    void OnEnable()
    {
        rotationVector = GetRotationVector();
    }

    void Update()
    {
        //if (!TimeControllerToggle.isTimeStopped)

        transform.localEulerAngles += rotationVector * normalRotationSpeed * Time.deltaTime;
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
}
