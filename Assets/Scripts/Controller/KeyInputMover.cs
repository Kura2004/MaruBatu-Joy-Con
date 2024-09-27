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

        // ��Joy-Con�̃X�e�B�b�N���͂��擾
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

        // �EJoy-Con�̃X�e�B�b�N���͂��擾
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
            Debug.Log("�ړ��悪�͈͊O�ł�: " + targetPosition);
            return;
        }

        isMoving = true;
        transform.DOMove(targetPosition, moveDuration).OnComplete(() => isMoving = false);
    }

    private bool IsOutOfBounds(Vector3 targetPosition)
    {
        if (boundsOrigin == null)
        {
            Debug.LogError("Bounds�̌��_���ݒ肳��Ă��܂���");
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
    private float horizontalMoveDistance = 5f; // �������̈ړ�����

    [SerializeField]
    private float verticalMoveDistance = 5f; // �c�����̈ړ�����

    [SerializeField]
    private float moveDuration = 1f; // �ړ��ɂ����鎞��

    [SerializeField]
    private Transform boundsOrigin; // �͈͂̌��_�i�C���X�y�N�^�[�Őݒ�j
    [SerializeField]
    private Vector3 boundsSize = new Vector3(10f, 0f, 10f); // �͈͂̍L���i�C���X�y�N�^�[�Őݒ�\�j

    private bool isMoving = false; // �ړ������ǂ����̃t���O

    void Update()
    {
        if (isMoving) return; // �ړ����͓��͂𖳎�

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

    // ABXY�{�^���ł̈ړ�����
    private void HandleABXYInput()
    {
        if (Input.GetKeyDown((KeyCode)SwitchController.A))
        {
            // A�{�^���������ꂽ�牡�����Ɉړ�
            TryMoveInDirection(Vector3.right * horizontalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.B))
        {
            // B�{�^���������ꂽ��c�����Ɉړ�
            TryMoveInDirection(Vector3.back * verticalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.X))
        {
            // X�{�^���������ꂽ��c�����Ɉړ�
            TryMoveInDirection(Vector3.forward * verticalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.Y))
        {
            // Y�{�^���������ꂽ�牡�����Ɉړ�
            TryMoveInDirection(Vector3.left * horizontalMoveDistance);
        }
    }

    // Arrow�n�{�^���ł̈ړ�����
    private void HandleArrowInput()
    {
        if (Input.GetKeyDown((KeyCode)SwitchController.UpArrow))
        {
            // ����L�[�������ꂽ��c�����Ɉړ�
            TryMoveInDirection(Vector3.forward * verticalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.DownArrow))
        {
            // �����L�[�������ꂽ��c�����Ɉړ�
            TryMoveInDirection(Vector3.back * verticalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.LeftArrow))
        {
            // �����L�[�������ꂽ�牡�����Ɉړ�
            TryMoveInDirection(Vector3.left * horizontalMoveDistance);
        }
        else if (Input.GetKeyDown((KeyCode)SwitchController.RightArrow))
        {
            // �E���L�[�������ꂽ�牡�����Ɉړ�
            TryMoveInDirection(Vector3.right * horizontalMoveDistance);
        }
    }

    // �ړ��͈͂��m�F���āA�͈͓��Ȃ�ړ�����
    private void TryMoveInDirection(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;

        // �ړ���̍��W���͈͊O�Ȃ�ړ����Ȃ�
        if (IsOutOfBounds(targetPosition))
        {
            Debug.Log("�ړ��悪�͈͊O�ł�: " + targetPosition);
            return;
        }

        // �ړ��������J�n
        isMoving = true; // �ړ����t���O�𗧂Ă�
        transform.DOMove(targetPosition, moveDuration).OnComplete(() => isMoving = false); // �ړ��������Ƀt���O������
    }

    // �ړ��悪�͈͊O���ǂ������`�F�b�N
    private bool IsOutOfBounds(Vector3 targetPosition)
    {
        if (boundsOrigin == null)
        {
            Debug.LogError("Bounds�̌��_���ݒ肳��Ă��܂���");
            return true;
        }

        // Bounds�̒��S�����_�Ƃ��A�͈͂��`�F�b�N����
        Vector3 boundsCenter = boundsOrigin.position;
        Vector3 halfSize = boundsSize / 2f;

        // �e���Ŕ͈͊O���ǂ����𔻒�
        bool isOutOfX = targetPosition.x < boundsCenter.x - halfSize.x || targetPosition.x > boundsCenter.x + halfSize.x;
        bool isOutOfY = targetPosition.y < boundsCenter.y - halfSize.y || targetPosition.y > boundsCenter.y + halfSize.y;
        bool isOutOfZ = targetPosition.z < boundsCenter.z - halfSize.z || targetPosition.z > boundsCenter.z + halfSize.z;

        return isOutOfX || isOutOfY || isOutOfZ;
    }
    */
