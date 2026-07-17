using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    private GameObject settingsPanel;
    private float lastSFXPlayTime = 0f;

    void Start()
    {
        CreateSettingsUI();
    }

    private void CreateSettingsUI()
    {
        // Find Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[SettingsController] No Canvas found in the scene!");
            return;
        }

        // 1. CREATE GEAR BUTTON
        GameObject gearBtnObj = new GameObject("SettingsGearButton");
        gearBtnObj.transform.SetParent(canvas.transform, false);

        RectTransform gearRect = gearBtnObj.AddComponent<RectTransform>();
        gearRect.anchorMin = new Vector2(1f, 1f);
        gearRect.anchorMax = new Vector2(1f, 1f);
        gearRect.pivot = new Vector2(1f, 1f);
        gearRect.anchoredPosition = new Vector2(-20f, -20f); // Inset 20px from top-right
        gearRect.sizeDelta = new Vector2(55f, 55f);

        Image gearImg = gearBtnObj.AddComponent<Image>();
        Sprite gearSprite = Resources.Load<Sprite>("settings_gear");
        if (gearSprite != null)
        {
            gearImg.sprite = gearSprite;
            gearImg.color = Color.white;
        }
        else
        {
            // Fallback if sprite not loaded
            gearImg.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            GameObject textFallback = new GameObject("Text");
            textFallback.transform.SetParent(gearBtnObj.transform, false);
            var textComp = textFallback.AddComponent<TextMeshProUGUI>();
            textComp.text = "⚙";
            textComp.fontSize = 30;
            textComp.alignment = TextAlignmentOptions.Center;
            textComp.color = Color.black;
        }

        Button gearBtn = gearBtnObj.AddComponent<Button>();
        gearBtn.transition = Selectable.Transition.ColorTint;
        ColorBlock gearColors = gearBtn.colors;
        gearColors.normalColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        gearColors.highlightedColor = Color.white;
        gearColors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        gearBtn.colors = gearColors;

        // 2. CREATE SETTINGS PANEL
        settingsPanel = new GameObject("SettingsPanel");
        settingsPanel.transform.SetParent(canvas.transform, false);
        settingsPanel.SetActive(false); // Hidden by default

        RectTransform panelRect = settingsPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(400f, 320f);

        Image panelImg = settingsPanel.AddComponent<Image>();
        // Load default UI background sprite for rounded corners
        Sprite panelBG = Resources.GetBuiltinResource<Sprite>("UI/Skin/Background.psd");
        if (panelBG != null)
        {
            panelImg.sprite = panelBG;
            panelImg.type = Image.Type.Sliced;
        }
        panelImg.color = new Color(0.08f, 0.09f, 0.13f, 0.96f); // Slick dark glassmorphic color

        // Add a clean border effect
        GameObject borderObj = new GameObject("Border");
        borderObj.transform.SetParent(settingsPanel.transform, false);
        RectTransform borderRect = borderObj.AddComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.sizeDelta = new Vector2(4f, 4f); // Slightly larger for outline effect
        Image borderImg = borderObj.AddComponent<Image>();
        if (panelBG != null)
        {
            borderImg.sprite = panelBG;
            borderImg.type = Image.Type.Sliced;
        }
        borderImg.color = new Color(0.18f, 0.22f, 0.32f, 0.5f); // Soft blue-grey border
        borderObj.transform.SetAsFirstSibling();

        // 3. TITLE TEXT
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(settingsPanel.transform, false);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.pivot = new Vector2(0.5f, 1f);
        titleRect.anchoredPosition = new Vector2(0f, -25f);
        titleRect.sizeDelta = new Vector2(300f, 40f);

        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "CÀI ĐẶT ÂM THANH";
        titleText.fontStyle = FontStyles.Bold;
        titleText.fontSize = 26f;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = new Color(0.95f, 0.75f, 0.2f, 1f); // Gold color

        // UI Resources for Sliders
        DefaultControls.Resources uiResources = new DefaultControls.Resources();
        uiResources.background = Resources.GetBuiltinResource<Sprite>("UI/Skin/Background.psd");
        uiResources.standard = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
        uiResources.knob = Resources.GetBuiltinResource<Sprite>("UI/Skin/Knob.psd");

        // 4. MUSIC ROW
        // Music Note Icon
        GameObject musicIconObj = new GameObject("MusicIcon");
        musicIconObj.transform.SetParent(settingsPanel.transform, false);
        RectTransform musicIconRect = musicIconObj.AddComponent<RectTransform>();
        musicIconRect.anchorMin = new Vector2(0.5f, 0.5f);
        musicIconRect.anchorMax = new Vector2(0.5f, 0.5f);
        musicIconRect.pivot = new Vector2(0.5f, 0.5f);
        musicIconRect.anchoredPosition = new Vector2(-130f, 35f);
        musicIconRect.sizeDelta = new Vector2(40f, 40f);
        Image musicImg = musicIconObj.AddComponent<Image>();
        Sprite musicSprite = Resources.Load<Sprite>("music_icon");
        if (musicSprite != null)
        {
            musicImg.sprite = musicSprite;
            musicImg.color = Color.white;
        }
        else
        {
            var txt = musicIconObj.AddComponent<TextMeshProUGUI>();
            txt.text = "🎵";
            txt.fontSize = 28;
            txt.alignment = TextAlignmentOptions.Center;
        }

        // Music Slider
        GameObject musicSliderObj = DefaultControls.CreateSlider(uiResources);
        musicSliderObj.name = "MusicSlider";
        musicSliderObj.transform.SetParent(settingsPanel.transform, false);
        RectTransform musicSliderRect = musicSliderObj.GetComponent<RectTransform>();
        musicSliderRect.anchorMin = new Vector2(0.5f, 0.5f);
        musicSliderRect.anchorMax = new Vector2(0.5f, 0.5f);
        musicSliderRect.pivot = new Vector2(0.5f, 0.5f);
        musicSliderRect.anchoredPosition = new Vector2(35f, 35f);
        musicSliderRect.sizeDelta = new Vector2(210f, 20f);

        Slider musicSlider = musicSliderObj.GetComponent<Slider>();
        var musicBgImg = musicSliderObj.transform.Find("Background").GetComponent<Image>();
        musicBgImg.color = new Color(1f, 1f, 1f, 0.12f);
        var musicFillImg = musicSliderObj.transform.Find("Fill Area/Fill").GetComponent<Image>();
        musicFillImg.color = new Color(0f, 0.95f, 1f, 1f); // Cyan
        var musicHandleImg = musicSliderObj.transform.Find("Handle Slide Area/Handle").GetComponent<Image>();
        musicHandleImg.color = Color.white;

        if (AudioController.Instance != null)
        {
            musicSlider.value = AudioController.Instance.MusicVolume;
        }
        else
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        }
        musicSlider.onValueChanged.AddListener((val) =>
        {
            if (AudioController.Instance != null)
            {
                AudioController.Instance.MusicVolume = val;
            }
        });


        // 5. SOUND FX ROW
        // Sound Icon
        GameObject soundIconObj = new GameObject("SoundIcon");
        soundIconObj.transform.SetParent(settingsPanel.transform, false);
        RectTransform soundIconRect = soundIconObj.AddComponent<RectTransform>();
        soundIconRect.anchorMin = new Vector2(0.5f, 0.5f);
        soundIconRect.anchorMax = new Vector2(0.5f, 0.5f);
        soundIconRect.pivot = new Vector2(0.5f, 0.5f);
        soundIconRect.anchoredPosition = new Vector2(-130f, -30f);
        soundIconRect.sizeDelta = new Vector2(40f, 40f);
        Image soundImg = soundIconObj.AddComponent<Image>();
        Sprite soundSprite = Resources.Load<Sprite>("sound_icon");
        if (soundSprite != null)
        {
            soundImg.sprite = soundSprite;
            soundImg.color = Color.white;
        }
        else
        {
            var txt = soundIconObj.AddComponent<TextMeshProUGUI>();
            txt.text = "🔊";
            txt.fontSize = 28;
            txt.alignment = TextAlignmentOptions.Center;
        }

        // Sound Slider
        GameObject soundSliderObj = DefaultControls.CreateSlider(uiResources);
        soundSliderObj.name = "SoundSlider";
        soundSliderObj.transform.SetParent(settingsPanel.transform, false);
        RectTransform soundSliderRect = soundSliderObj.GetComponent<RectTransform>();
        soundSliderRect.anchorMin = new Vector2(0.5f, 0.5f);
        soundSliderRect.anchorMax = new Vector2(0.5f, 0.5f);
        soundSliderRect.pivot = new Vector2(0.5f, 0.5f);
        soundSliderRect.anchoredPosition = new Vector2(35f, -30f);
        soundSliderRect.sizeDelta = new Vector2(210f, 20f);

        Slider soundSlider = soundSliderObj.GetComponent<Slider>();
        var soundBgImg = soundSliderObj.transform.Find("Background").GetComponent<Image>();
        soundBgImg.color = new Color(1f, 1f, 1f, 0.12f);
        var soundFillImg = soundSliderObj.transform.Find("Fill Area/Fill").GetComponent<Image>();
        soundFillImg.color = new Color(1f, 0.55f, 0f, 1f); // Orange
        var soundHandleImg = soundSliderObj.transform.Find("Handle Slide Area/Handle").GetComponent<Image>();
        soundHandleImg.color = Color.white;

        if (AudioController.Instance != null)
        {
            soundSlider.value = AudioController.Instance.SFXVolume;
        }
        else
        {
            soundSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.8f);
        }
        soundSlider.onValueChanged.AddListener((val) =>
        {
            if (AudioController.Instance != null)
            {
                AudioController.Instance.SFXVolume = val;
            }
            PlayTestSFX();
        });


        // 6. CLOSE BUTTONS
        // Top-right 'X' Button
        GameObject xBtnObj = new GameObject("XButton");
        xBtnObj.transform.SetParent(settingsPanel.transform, false);
        RectTransform xBtnRect = xBtnObj.AddComponent<RectTransform>();
        xBtnRect.anchorMin = new Vector2(1f, 1f);
        xBtnRect.anchorMax = new Vector2(1f, 1f);
        xBtnRect.pivot = new Vector2(1f, 1f);
        xBtnRect.anchoredPosition = new Vector2(-12f, -12f);
        xBtnRect.sizeDelta = new Vector2(32f, 32f);

        Image xImg = xBtnObj.AddComponent<Image>();
        if (uiResources.standard != null)
        {
            xImg.sprite = uiResources.standard;
            xImg.type = Image.Type.Sliced;
        }
        xImg.color = new Color(0.25f, 0.28f, 0.38f, 1f);

        GameObject xTextObj = new GameObject("Text");
        xTextObj.transform.SetParent(xBtnObj.transform, false);
        RectTransform xTextRect = xTextObj.AddComponent<RectTransform>();
        xTextRect.anchorMin = Vector2.zero;
        xTextRect.anchorMax = Vector2.one;
        xTextRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI xText = xTextObj.AddComponent<TextMeshProUGUI>();
        xText.text = "✕";
        xText.fontSize = 18f;
        xText.alignment = TextAlignmentOptions.Center;
        xText.color = Color.white;

        Button xBtn = xBtnObj.AddComponent<Button>();
        xBtn.transition = Selectable.Transition.ColorTint;
        ColorBlock xColors = xBtn.colors;
        xColors.normalColor = Color.white;
        xColors.highlightedColor = new Color(0.9f, 0.3f, 0.3f, 1f);
        xColors.pressedColor = new Color(0.7f, 0.2f, 0.2f, 1f);
        xBtn.colors = xColors;
        xBtn.onClick.AddListener(CloseSettings);


        // Bottom 'ĐÓNG' Button
        GameObject closeBtnObj = new GameObject("CloseButton");
        closeBtnObj.transform.SetParent(settingsPanel.transform, false);
        RectTransform closeBtnRect = closeBtnObj.AddComponent<RectTransform>();
        closeBtnRect.anchorMin = new Vector2(0.5f, 0f);
        closeBtnRect.anchorMax = new Vector2(0.5f, 0f);
        closeBtnRect.pivot = new Vector2(0.5f, 0f);
        closeBtnRect.anchoredPosition = new Vector2(0f, 25f);
        closeBtnRect.sizeDelta = new Vector2(130f, 40f);

        Image closeImg = closeBtnObj.AddComponent<Image>();
        if (uiResources.standard != null)
        {
            closeImg.sprite = uiResources.standard;
            closeImg.type = Image.Type.Sliced;
        }
        closeImg.color = new Color(1f, 0.55f, 0f, 1f);

        GameObject shadowObj = new GameObject("Shadow");
        shadowObj.transform.SetParent(closeBtnObj.transform, false);
        RectTransform shadowRect = shadowObj.AddComponent<RectTransform>();
        shadowRect.anchorMin = Vector2.zero;
        shadowRect.anchorMax = Vector2.one;
        shadowRect.anchoredPosition = new Vector2(0f, -2f);
        shadowRect.sizeDelta = Vector2.zero;
        Image shadowImg = shadowObj.AddComponent<Image>();
        if (uiResources.standard != null)
        {
            shadowImg.sprite = uiResources.standard;
            shadowImg.type = Image.Type.Sliced;
        }
        shadowImg.color = new Color(0f, 0f, 0f, 0.3f);
        shadowObj.transform.SetAsFirstSibling();

        GameObject closeTextObj = new GameObject("Text");
        closeTextObj.transform.SetParent(closeBtnObj.transform, false);
        RectTransform closeTextRect = closeTextObj.AddComponent<RectTransform>();
        closeTextRect.anchorMin = Vector2.zero;
        closeTextRect.anchorMax = Vector2.one;
        closeTextRect.sizeDelta = Vector2.zero;
        TextMeshProUGUI closeText = closeTextObj.AddComponent<TextMeshProUGUI>();
        closeText.text = "ĐÓNG";
        closeText.fontStyle = FontStyles.Bold;
        closeText.fontSize = 18f;
        closeText.alignment = TextAlignmentOptions.Center;
        closeText.color = Color.white;

        Button closeBtn = closeBtnObj.AddComponent<Button>();
        closeBtn.transition = Selectable.Transition.ColorTint;
        ColorBlock closeColors = closeBtn.colors;
        closeColors.normalColor = Color.white;
        closeColors.highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        closeColors.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        closeBtn.colors = closeColors;
        closeBtn.onClick.AddListener(CloseSettings);


        // Button interaction to open settings
        gearBtn.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            if (AudioController.Instance != null)
            {
                AudioController.Instance.PlaySFX(SoundType.Click);
            }
        }
    }

    private void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
            if (AudioController.Instance != null)
            {
                AudioController.Instance.PlaySFX(SoundType.Click);
            }
        }
    }

    private void PlayTestSFX()
    {
        if (Time.unscaledTime - lastSFXPlayTime > 0.15f)
        {
            if (AudioController.Instance != null)
            {
                AudioController.Instance.PlaySFX(SoundType.Click);
            }
            lastSFXPlayTime = Time.unscaledTime;
        }
    }
}
