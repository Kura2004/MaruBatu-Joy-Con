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
        if (isMoving || !GameStateManager.Instance.IsBoardSetupComplete
            || GameStateManager.Instance.IsRotating) return;

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