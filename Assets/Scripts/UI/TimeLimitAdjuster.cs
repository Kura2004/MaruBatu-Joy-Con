using UnityEngine;
using TMPro; // TextMeshPro���g�p���邽�߂̖��O���
using UnityEngine.UI;
using System.Collections;

public class TimeLimitAdjuster : MonoBehaviour
{
    [SerializeField] private TMP_Text valueDisplay;   // TextMeshPro���g�p����e�L�X�g
    [SerializeField] private float cooldownDuration = 1f; // �N�[���_�E������
    [SerializeField] private Image Arrow_Up;
    [SerializeField] private Image Arrow_Down;

    public static int timeLimit = 30; // ��������Q�[���̐�������
    private bool canAdjustTime = true;   // �^�C���������󂯕t���邩�ǂ���
    private bool inputCooldownActive = false; // ���̓N�[���_�E���̃t���O

    private Joycon leftJoycon;
    private Joycon rightJoycon;

    [System.Obsolete]
    private void Awake()
    {
        // ���̃C���X�^���X�����ɑ��݂���ꍇ�́A���݂̃C���X�^���X��j������
        if (FindObjectsOfType<TimeLimitAdjuster>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); // �V�[���؂�ւ����ɃI�u�W�F�N�g���ێ�
        }
    }

    private void Start()
    {
        // �����l��\��
        UpdateValueDisplay();

        Arrow_Up.enabled = true;
        Arrow_Down.enabled = true;

        // Joy-Con�̏�����
        var joycons = JoyconManager.Instance.j;
        if (joycons.Count >= 2)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }
    }

    private void Update()
    {
        // ������󂯕t���Ȃ��ꍇ�͏������Ȃ�
        if (inputCooldownActive) return;

        // L�̃X�e�B�b�N�̓��͂��擾
        if (leftJoycon != null)
        {
            float verticalInput = leftJoycon.GetStick()[0]; // �X�e�B�b�N��Y�l���擾

            // �c���̓��͂�0�ȊO�̂Ƃ��̂ݏ���
            if (Mathf.Abs(verticalInput) > 0.01f && canAdjustTime)
            {
                AdjustTimeLimit(verticalInput > 0 ? 10 : -10); // �\�̈ʂ𑝌�
                StartInputCooldown(); // �N�[���_�E�����J�n
            }
        }

        float debugInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(debugInput) > 0.01f && canAdjustTime)
        {
            AdjustTimeLimit(debugInput > 0 ? 10 : -10); // �\�̈ʂ𑝌�
            StartInputCooldown(); // �N�[���_�E�����J�n
        }
    }

    int prevTimeLimit = 0;
    private void AdjustTimeLimit(int amount)
    {
        if (!canAdjustTime) return;

        timeLimit += amount;
        timeLimit = Mathf.Clamp(timeLimit, 20, 50); // �ŏ��l20�A�ő�l40�ɐ���

        if (timeLimit != prevTimeLimit)
        {
            ScenesAudio.ClickSe();
        }

        UpdateValueDisplay();
        EraseArrow();
        Show_Arrow();

        // �^�C����������莞�Ԗ����ɂ���
        StartCoroutine(DisableTimeAdjustmentTemporarily());
        prevTimeLimit = timeLimit;
    }

    private void UpdateValueDisplay()
    {
        valueDisplay.text = timeLimit.ToString("F0") + "s"; // �����ŕ\��
        if (timeLimit > 40)
        {
            valueDisplay.text = Mathf.Infinity.ToString();
            Arrow_Up.enabled = false;
        }
    }

    private void EraseArrow()
    {
        if (timeLimit == 20)
        {
            Arrow_Down.enabled = false;
        }
    }

    private void Show_Arrow()
    {
        if (timeLimit <= 40)
        {
            Arrow_Up.enabled = true;
        }

        if (timeLimit >= 30)
        {
            Arrow_Down.enabled = true;
        }
    }

    // �N�[���_�E�����J�n����
    private IEnumerator DisableTimeAdjustmentTemporarily()
    {
        canAdjustTime = false;
        yield return new WaitForSeconds(cooldownDuration); // ��莞�ԑ҂�
        canAdjustTime = true;
    }

    // ���͂̃N�[���_�E�����J�n
    private void StartInputCooldown()
    {
        inputCooldownActive = true;
        StartCoroutine(ResetInputCooldown());
    }

    private IEnumerator ResetInputCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        inputCooldownActive = false;
    }
}