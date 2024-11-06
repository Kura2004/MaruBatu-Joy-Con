using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class GuideScaler : MonoBehaviour
{
    [SerializeField] private List<Image> guideImages;       // ガイドとして設定するImage
    [SerializeField] private float duration = 1f;           // スケールの補完時間
    [SerializeField] private Ease easing = Ease.InOutQuad;  // イージングをインスペクターで設定
    [SerializeField] private float scaleMultiplier = 1.5f;  // 拡大時の倍率をインスペクターで指定

    [SerializeField] private Image GuideBack;

    private List<Vector3> initialScales = new List<Vector3>();
    private bool isGuideVisible = true; // ガイドが表示されているかどうかのフラグ
    private bool isAnimating = false;

    private bool isGuideChanging = false;
    private int currentGuideIndex = 0;

    private Joycon leftJoycon;
    private Joycon rightJoycon;

    [SerializeField] private Button guideButton;

    private void Start()
    {
        // ガイドImageの初期化
        if (guideImages == null || guideImages.Count == 0)
        {
            Debug.LogError("ガイドImageが設定されていません！");
            return;
        }

        foreach (var image in guideImages)
        {
            initialScales.Add(image.transform.localScale);
            image.enabled = false;
        }

        isGuideVisible = false;
        currentGuideIndex = 0;

        // Joy-Con の初期化
        var joycons = JoyconManager.Instance.j;
        if (joycons == null || joycons.Count == 0)
        {
            Debug.LogWarning("Joy-Con が見つかりません。");
            return;
        }

        if (joycons.Count >= 2)
        {
            leftJoycon = joycons.Find(c => c.isLeft);
            rightJoycon = joycons.Find(c => !c.isLeft);
        }

        guideButton.onClick.AddListener(() => {
            JoyConButtonSelector.onGuide = true;
        });
    }

    // n秒でガイドのスケールを拡大するメソッド
    public void ShowGuide()
    {
        if (currentGuideIndex != 0)
        {
            Debug.LogWarning("ガイドのインデクスが異常" + currentGuideIndex);
            return;
        }

        isAnimating = true;
        guideImages[currentGuideIndex].enabled = true;
        guideImages[currentGuideIndex].DOFade(1f, duration);

        Vector3 targetScale = initialScales[currentGuideIndex] * scaleMultiplier;  // 拡大倍率を適用
        guideImages[currentGuideIndex].transform.localScale = Vector3.zero;
        guideImages[currentGuideIndex].transform.DOScale(targetScale, duration).SetEase(easing).OnComplete(() =>
        {
            isAnimating = false;
            for (int i = 1; i < guideImages.Count; i++)
            {
                guideImages[i].rectTransform.localScale = Vector3.one;
            }
        });
    }

    // n秒でガイドのスケールを初期値に戻す（非表示状態）メソッド
    public void HideGuide()
    {
        bool indexCheck = currentGuideIndex == guideImages.Count - 1 ||
            currentGuideIndex == 0;

        if (!indexCheck)
        {
            Debug.LogWarning("ガイドのインデクスが異常" + currentGuideIndex);
            return;
        }

        isAnimating = true;
        guideImages[currentGuideIndex].transform.DOScale(Vector3.zero, duration).SetEase(easing).OnComplete(() =>
        {
            isAnimating = false;
            guideImages[currentGuideIndex].enabled = false;
            currentGuideIndex = 0;
            JoyConButtonSelector.onGuide = false;
        });
    }

    // ガイドの表示/非表示をトグルするメソッド
    public void ToggleGuide()
    {
        ScenesAudio.ClickSe();

        if (isGuideVisible)
        {
            GuideBack.color = Color.clear;
            HideGuide();
        }
        else
        {
            GuideBack.DOColor(Color.black, 0.2f);
            ShowGuide();
        }
        isGuideVisible = !isGuideVisible;
    }

    // 現在のガイドImageをフェードアウトして次のガイドImageをフェードインするメソッド
    private void ShowNextGuideImage()
    {
        if (currentGuideIndex + 1 == guideImages.Count)
        {
            guideButton.onClick?.Invoke();
            return;
        }

        ScenesAudio.ClickSe();
        isGuideChanging = true;
        int prevIndex = currentGuideIndex;

        guideImages[currentGuideIndex].DOFade(0f, duration).SetEase(easing).OnComplete(() =>
        {
            guideImages[prevIndex].enabled = false;
        });

        currentGuideIndex = (currentGuideIndex + 1) % guideImages.Count;

        guideImages[currentGuideIndex].enabled = true;
        guideImages[currentGuideIndex].DOFade(1f, duration).SetEase(easing).OnComplete(() =>
        {
            isGuideChanging = false;
        });
    }

    private void ShowPrevGuideImage()
    {
        if (currentGuideIndex - 1 < 0)
        {
            guideButton.onClick?.Invoke();
            return;
        }

        ScenesAudio.ClickSe();

        isGuideChanging = true;
        int prevIndex = currentGuideIndex;

        guideImages[currentGuideIndex].DOFade(0f, duration).SetEase(easing).OnComplete(() =>
        {
            guideImages[prevIndex].enabled = false;
        });

        currentGuideIndex = (currentGuideIndex - 1) % guideImages.Count;

        guideImages[currentGuideIndex].enabled = true;
        guideImages[currentGuideIndex].DOFade(1f, duration).SetEase(easing).OnComplete(() =>
        {
            isGuideChanging = false;
        });
    }

    private void LateUpdate()
    {
        if (!isGuideVisible || isAnimating || isGuideChanging) return;

        if (leftJoycon.GetButtonDown(Joycon.Button.DPAD_DOWN))
        {
            ShowNextGuideImage();
            return;
        }

        if (leftJoycon.GetButtonDown(Joycon.Button.DPAD_LEFT))
        {
            ShowPrevGuideImage();
            return;
        }
    }
}
