using UnityEngine;
using DG.Tweening;

public class CanvasBounce : MonoBehaviour
{
    [SerializeField] protected RectTransform canvasRectTransform;
    [SerializeField] protected GameObject canvasObject;
    [SerializeField] protected float initialDropHeight = 1000f;
    [SerializeField] protected float groundY = -500f;
    [SerializeField] protected float bounceHeight = 200f;
    [SerializeField] protected int bounceCount = 3;
    [SerializeField] protected float initialBounceDuration = 0.5f;
    [SerializeField] protected float heightDampingFactor = 0.5f;
    [SerializeField] protected float durationDampingFactor = 0.7f;
    [SerializeField] protected float riseDuration = 0.3f;
    [SerializeField] protected bool dropOnStart = false;

    protected bool isFalling = false;
    protected bool isBouncingComplete = true;

    private Joycon leftJoycon;
    private Joycon rightJoycon;

    [SerializeField] CountdownText countdown;
    void Start()
    {
        // Joy-Con�̏�����
        var joycons = JoyconManager.Instance.j;
        if (joycons == null) return;

        if (joycons.Count >= 2)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }

        if (dropOnStart)
        {
            InitializeCanvasPosition();
            isFalling = false; // �����t���O�����Z�b�g
            isBouncingComplete = true; // �o�E���h�A�j���[�V���������������t���O��ݒ�
        }
        else
        {
            canvasObject.SetActive(false);
        }
    }

    void InitializeCanvasPosition()
    {
        InitializeDrop();
        Vector3 setPos = canvasRectTransform.localPosition;
        setPos.y = groundY;
        canvasRectTransform.localPosition = setPos;
    }

    void Update()
    {
        if (ShouldDropCanvas())
        {
            Debug.Log("�L�����o�X���������܂�");
        }

        // L�{�^���ŃL�����o�X���㏸������
        if (leftJoycon != null && 
            (leftJoycon.GetButtonDown(Joycon.Button.DPAD_DOWN)) && !isFalling && isBouncingComplete)
        {
            RiseCanvas();

            if (dropOnStart)
            {
                GameStateManager.Instance.StartBoardSetup(countdown.GetTotalDuration());
                StartCoroutine(countdown.StartCountdown());
                TimeLimitController.Instance.ResetTimer();
                TimeLimitController.Instance.StopTimer();
                dropOnStart = false;
            }

            Debug.Log("�L�����o�X���㏸���܂�");
        }

        if (leftJoycon != null && leftJoycon.GetButtonDown(Joycon.Button.DPAD_LEFT) && dropOnStart)
        {
            ScenesLoader.Instance.LoadStartMenu();
            Debug.Log("�X�^�[�g��ʂɖ߂�܂�");
        }
    }

    bool ShouldDropCanvas()
    {
        return false;
    }

    void InitializeDrop()
    {
        isFalling = true;
        isBouncingComplete = false; // �o�E���h�A�j���[�V�����̃t���O�����Z�b�g
    }

    void DropCanvas()
    {
        InitializeDrop();

        // �L�����o�X���A�N�e�B�u�ɂ���
        canvasObject.SetActive(true);

        TimeLimitController.Instance.StopTimer();

        // �L�����o�X�������̍����ɐݒ�
        canvasRectTransform.anchoredPosition = new Vector2(canvasRectTransform.anchoredPosition.x, initialDropHeight);

        // �����A�j���[�V����
        canvasRectTransform.DOAnchorPosY(groundY, initialBounceDuration).SetEase(Ease.InQuad).OnComplete(Bounce);
    }

    void Bounce()
    {
        float currentBounceHeight = bounceHeight;
        float currentBounceDuration = initialBounceDuration;
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < bounceCount; i++)
        {
            // �o�E���h�A�j���[�V�������I�������ɓ���̃��\�b�h���Ă�
            sequence.AppendCallback(() => ScenesAudio.FallSe());

            sequence.Append(canvasRectTransform.DOAnchorPosY(groundY + currentBounceHeight, currentBounceDuration).SetEase(Ease.OutQuad));
            sequence.Append(canvasRectTransform.DOAnchorPosY(groundY, currentBounceDuration).SetEase(Ease.InQuad));

            // �e�ލ����Ǝ��Ԃ�����������
            currentBounceHeight *= heightDampingFactor;
            currentBounceDuration *= durationDampingFactor;
        }

        sequence.OnComplete(() =>
        {
            isFalling = false; // �����t���O�����Z�b�g
            isBouncingComplete = true; // �o�E���h�A�j���[�V���������������t���O��ݒ�
            ScenesAudio.FallSe();
        });

        sequence.Play();
    }

    void RiseCanvas()
    {
        if (!isFalling)
        {
            // �L�����o�X��n�ʂ̈ʒu�ɐݒ�
            canvasRectTransform.anchoredPosition = new Vector2(canvasRectTransform.anchoredPosition.x, groundY);

            // �㏸�A�j���[�V����
            canvasRectTransform.DOAnchorPosY(initialDropHeight, riseDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                if (dropOnStart)
                    Destroy(this);
                // �A�j���[�V����������A�L�����o�X���A�N�e�B�u�ɐݒ�
                canvasObject.SetActive(false);

            });
        }
    }
}
