using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; // 基本の移動速度
    [SerializeField] private float acceleration = 2f; // 加速度の係数
    [SerializeField] private float deceleration = 2f; // 減速度の係数
    [SerializeField] private float maxVelocity = 10f; // 最大速度の上限

    private Vector3 startPosition; // カメラの初期座標を記録する変数
    private bool isLocked = false; // 座標移動のロック状態
    private Vector3 currentVelocity = Vector3.zero; // 現在の速度

    private void Start()
    {
        // 初期座標を記録
        startPosition = transform.position;
    }

    private void Update()
    {
        HandleInput();
        ApplyMovement();
    }

    private void HandleInput()
    {
        HandleLockToggle();
        HandleMovement();
        HandlePositionReset();
    }

    private void HandleLockToggle()
    {
        // マウスのクリックでロックの切り替え
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isLocked = !isLocked;
            ScenesAudio.ShootSe();
        }
    }

    private void HandleMovement()
    {
        if (isLocked)
            return;

        // Input.GetAxisを使用して移動入力を取得
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/Dキーまたは矢印キー
        float moveVertical = Input.GetAxis("Vertical");   // W/Sキーまたは矢印キー

        Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);

        // キーが押された間、加速
        if (move.magnitude > 0)
        {
            currentVelocity += move.normalized * moveSpeed * acceleration * Time.deltaTime;
            // 速度の上限を適用
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);
        }
        else
        {
            // キーが離された瞬間から減速
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }
    }

    private void ApplyMovement()
    {
        // カメラの位置を移動させる
        transform.position += currentVelocity * Time.deltaTime;
    }

    private void HandlePositionReset()
    {
        // 1キーが押された時、カメラの位置を初期座標にリセット
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentVelocity = Vector3.zero;
            transform.position = startPosition;
            isLocked = false;
            ScenesAudio.WinSe(); // 必要に応じて音を再生するメソッドを追加
        }
    }
}
