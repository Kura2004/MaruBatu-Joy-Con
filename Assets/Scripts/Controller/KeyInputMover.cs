using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JoyconInputMover : MonoBehaviour
{
    [SerializeField]
    private float horizontalMoveDistance = 5f;
    [SerializeField]
    private float verticalMoveDistance = 5f;
    [SerializeField]
    private float moveDuration = 1f;
    [SerializeField]
    private Transform boundsOrigin;
    [SerializeField]
    private Vector3 boundsSize = new Vector3(10f, 0f, 10f);

    private bool isMoving = false;

    private List<Joycon> joycons;
    private Joycon leftJoycon;
    private Joycon rightJoycon;

    void Start()
    {
        joycons = JoyconManager.Instance.j;

        if (joycons.Count >= 1)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }
    }

    void Update()
    {
        if (isMoving || !GameStateManager.Instance.IsBoardSetupComplete) return;

        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece) ||
            GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup))
        {
            Handle2PInput();
        }

        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) ||
            GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup))
        {
            Handle1PInput();
        }
    }

    [SerializeField] bool onDebug = false;

    private void Handle1PInput()
    {
        if (leftJoycon == null) return;

        // 左Joy-Conのスティック入力を取得
        Vector2 stickInput = new Vector2(-leftJoycon.GetStick()[1], leftJoycon.GetStick()[0]);

#if UNITY_EDITOR
        if (onDebug)
        {
            stickInput.x = Input.GetAxis("Horizontal");
            stickInput.y = Input.GetAxis("Vertical");
        }
#endif

        if (Mathf.Abs(stickInput.x) < 0.1f) stickInput.x = 0;
        if (Mathf.Abs(stickInput.y) < 0.1f) stickInput.y = 0;

        if (stickInput.x == 0 && stickInput.y == 0) return;

        if (Mathf.Abs(stickInput.x) > Mathf.Abs(stickInput.y))
        {
            Vector3 moveDirection = (stickInput.x > 0) ? Vector3.right * horizontalMoveDistance : Vector3.left * horizontalMoveDistance;
            TryMoveInDirection(moveDirection);
        }
        else
        {
            Vector3 moveDirection = (stickInput.y > 0) ? Vector3.forward * verticalMoveDistance : Vector3.back * verticalMoveDistance;
            TryMoveInDirection(moveDirection);
        }
    }

    private void Handle2PInput()
    {
        if (rightJoycon == null) return;

        // 右Joy-Conのスティック入力を取得
        Vector2 stickInput = new Vector2(rightJoycon.GetStick()[1], -rightJoycon.GetStick()[0]);

#if UNITY_EDITOR
        if (onDebug)
        {
            stickInput.x = Input.GetAxis("Horizontal");
            stickInput.y = Input.GetAxis("Vertical");
        }
#endif

        if (Mathf.Abs(stickInput.x) < 0.1f) stickInput.x = 0;
        if (Mathf.Abs(stickInput.y) < 0.1f) stickInput.y = 0;

        if (stickInput.x == 0 && stickInput.y == 0) return;

        if (Mathf.Abs(stickInput.x) > Mathf.Abs(stickInput.y))
        {
            Vector3 moveDirection = (stickInput.x > 0) ? Vector3.right * horizontalMoveDistance : Vector3.left * horizontalMoveDistance;
            TryMoveInDirection(moveDirection);
        }
        else
        {
            Vector3 moveDirection = (stickInput.y > 0) ? Vector3.forward * verticalMoveDistance : Vector3.back * verticalMoveDistance;
            TryMoveInDirection(moveDirection);
        }
    }

    private void TryMoveInDirection(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;

        if (IsOutOfBounds(targetPosition))
        {
            Debug.Log("移動先が範囲外です: " + targetPosition);
            return;
        }

        isMoving = true;
        transform.DOMove(targetPosition, moveDuration).OnComplete(() => isMoving = false);
    }

    private bool IsOutOfBounds(Vector3 targetPosition)
    {
        if (boundsOrigin == null)
        {
            Debug.LogError("Boundsの原点が設定されていません");
            return true;
        }

        Vector3 boundsCenter = boundsOrigin.position;
        Vector3 halfSize = boundsSize / 2f;

        bool isOutOfX = targetPosition.x < boundsCenter.x - halfSize.x || targetPosition.x > boundsCenter.x + halfSize.x;
        bool isOutOfY = targetPosition.y < boundsCenter.y - halfSize.y || targetPosition.y > boundsCenter.y + halfSize.y;
        bool isOutOfZ = targetPosition.z < boundsCenter.z - halfSize.z || targetPosition.z > boundsCenter.z + halfSize.z;

        return isOutOfX || isOutOfY || isOutOfZ;
    }
}


/*
    [SerializeField]
    private float horizontalMoveDistance = 5f; // 横方向の移動距離

    [SerializeField]
    private float verticalMoveDistance = 5f; // 縦方向の移動距離

    [SerializeField]
    private float moveDuration = 1f; // 移動にかかる時間

    [SerializeField]
    private Transform boundsOrigin; // 範囲の原点（インスペクターで設定）
    [SerializeField]
    private Vector3 boundsSize = new Vector3(10f, 0f, 10f); // 範囲の広さ（インスペクターで設定可能）

    private bool isMoving = false; // 移動中かどうかのフラグ

    void Update()
    {
        if (isMoving) return; // 移動中は入力を無視

        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece) ||
            GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup))
        {
            HandleABXYInput();
        }

        if (GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) ||
            GameTurnManager.Instance.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup))
        {
            HandleArrowInput();
        }
    }

    // ABXYボタンでの移動処理
    private void HandleABXYInput()
    {
        if (Input.GetKeyDown((KeyCode)SwitchController.A))
        {
            // Aボタンが押されたら横方向に移動
            TryMoveInDirection(Vector3.right * horizontalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.B))
        {
            // Bボタンが押されたら縦方向に移動
            TryMoveInDirection(Vector3.back * verticalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.X))
        {
            // Xボタンが押されたら縦方向に移動
            TryMoveInDirection(Vector3.forward * verticalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.Y))
        {
            // Yボタンが押されたら横方向に移動
            TryMoveInDirection(Vector3.left * horizontalMoveDistance);
        }
    }

    // Arrow系ボタンでの移動処理
    private void HandleArrowInput()
    {
        if (Input.GetKeyDown((KeyCode)SwitchController.UpArrow))
        {
            // 上矢印キーが押されたら縦方向に移動
            TryMoveInDirection(Vector3.forward * verticalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.DownArrow))
        {
            // 下矢印キーが押されたら縦方向に移動
            TryMoveInDirection(Vector3.back * verticalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.LeftArrow))
        {
            // 左矢印キーが押されたら横方向に移動
            TryMoveInDirection(Vector3.left * horizontalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.RightArrow))
        {
            // 右矢印キーが押されたら横方向に移動
            TryMoveInDirection(Vector3.right * horizontalMoveDistance);
        }
    }

    // 移動範囲を確認して、範囲内なら移動する
    private void TryMoveInDirection(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;

        // 移動後の座標が範囲外なら移動しない
        if (IsOutOfBounds(targetPosition))
        {
            Debug.Log("移動先が範囲外です: " + targetPosition);
            return;
        }

        // 移動処理を開始
        isMoving = true; // 移動中フラグを立てる
        transform.DOMove(targetPosition, moveDuration).OnComplete(() => isMoving = false); // 移動完了時にフラグを解除
    }

    // 移動先が範囲外かどうかをチェック
    private bool IsOutOfBounds(Vector3 targetPosition)
    {
        if (boundsOrigin == null)
        {
            Debug.LogError("Boundsの原点が設定されていません");
            return true;
        }

        // Boundsの中心を原点とし、範囲をチェックする
        Vector3 boundsCenter = boundsOrigin.position;
        Vector3 halfSize = boundsSize / 2f;

        // 各軸で範囲外かどうかを判定
        bool isOutOfX = targetPosition.x < boundsCenter.x - halfSize.x || targetPosition.x > boundsCenter.x + halfSize.x;
        bool isOutOfY = targetPosition.y < boundsCenter.y - halfSize.y || targetPosition.y > boundsCenter.y + halfSize.y;
        bool isOutOfZ = targetPosition.z < boundsCenter.z - halfSize.z || targetPosition.z > boundsCenter.z + halfSize.z;

        return isOutOfX || isOutOfY || isOutOfZ;
    }
    */
