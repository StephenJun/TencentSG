using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CWindow;

public class SettingMenu : BaseWindow {


    //private float _brightness;
    //public float Brightness
    //{
    //    get
    //    {
    //        _brightness = PlayerPrefs.GetFloat("Brightness", 0.0f);
    //        return _brightness;
    //    }
    //    set
    //    {
    //        _brightness = value;
    //        PlayerPrefs.SetFloat("Brightness", _brightness);
    //    }
    //}


    //private int _quality;
    //public int Quality
    //{
    //    get
    //    {
    //        _quality = PlayerPrefs.GetInt("Quality", QualitySettings.names.Length - 1);
    //        return _quality;
    //    }
    //    set
    //    {
    //        _quality = value;
    //        PlayerPrefs.SetInt("Quality", _quality);
    //    }
    //}

    //[SerializeField]
    //private Color pressedColor;
    //[SerializeField]
    //private Color normalColor;

    //[SerializeField]
    //private Button gameplayButton;
    //[SerializeField]
    //private Button controlButton;
    //[SerializeField]
    //private Button videoButton;
    //[SerializeField]
    //private Button audioButton;


    ////setting panels
    //[SerializeField]
    //private GameObject gameplaySettings;
    //[SerializeField]
    //private GameObject controlSettings;
    //[SerializeField]
    //private GameObject videoSettings;
    //[SerializeField]
    //private GameObject audioSettings;


    ////gaphics settings
    //[SerializeField]
    //private Slider brightnessSlider;
    //[SerializeField]
    //private Dropdown qualityDropdown;

    ////audio settings
    //[SerializeField]
    //private Slider BGMVolumeSlider;
    //[SerializeField]
    //private Slider UIVolumeSlider;
    //[SerializeField]
    //private Slider GameplayVolumeSlider;





    ////[SerializeField]
    ////private PostProcessVolume globalPPV;
    ////private ColorGrading colorGrading;



    //private void OnBrightnessChanged()
    //{
    //    Brightness = brightnessSlider.value;
    //    //colorGrading.brightness.value = Brightness;
    //}

    //private void OnQualityChanged()
    //{
    //    Quality = qualityDropdown.value;
    //    QualitySettings.SetQualityLevel(Quality);
    //}

    //private void OnBGMVolChanged()
    //{
       
    //}

    //private void OnUIVolChanged()
    //{
       
    //}

    //private void OnGameplayVolChanged()
    //{
       
    //}




    //public void OnGameplaySelected()
    //{
    //    UnSelectAllSettings();
    //    gameplaySettings.SetActive(true);
    //    gameplayButton.image.color = pressedColor;
       
    //}

    //public void OnControlSelected()
    //{
    //    UnSelectAllSettings();
    //    controlSettings.SetActive(true);
    //    controlButton.image.color = pressedColor;
        
    //}


    //public void OnVideoSelected()
    //{
    //    UnSelectAllSettings();
    //    videoSettings.SetActive(true);
    //    videoButton.image.color = pressedColor;
        
    //}

    //public void OnAudioSelected()
    //{
    //    UnSelectAllSettings();
    //    audioSettings.SetActive(true);
    //    audioButton.image.color = pressedColor;
        
    //}

    //private void UnSelectAllSettings()
    //{
    //    gameplayButton.image.color = normalColor;
    //    controlButton.image.color = normalColor;
    //    videoButton.image.color = normalColor;
    //    audioButton.image.color = normalColor;

    //    gameplaySettings.SetActive(false);
    //    controlSettings.SetActive(false);
    //    videoSettings.SetActive(false);
    //    audioSettings.SetActive(false);
    //}

    //public void OnEnable()
    //{
    //    OnGameplaySelected();


    //    //~~~~~~~~~~~~~~~~~~~ Graphic Setting Inits ~~~~~~~~~~~~~~~~~
    //    //globalPPV.profile.TryGetSettings<ColorGrading>(out colorGrading);
    //    //colorGrading.brightness.value = Brightness;
    //    brightnessSlider.value = Brightness;
    //    brightnessSlider.onValueChanged.AddListener(delegate {
    //        OnBrightnessChanged();
    //    });

    //    //init quality dropdown
    //    qualityDropdown.ClearOptions();
    //    qualityDropdown.AddOptions(QualitySettings.names.ToList());
    //    qualityDropdown.value = Quality;
    //    qualityDropdown.onValueChanged.AddListener(delegate
    //    {
    //        OnQualityChanged();
    //    });

    //    //~~~~~~~~~~~~~~~~~~~~ Audio Setting Inits ~~~~~~~~~~~~~~~~~~~
    //    //BGMVolumeSlider.value = GameManager.Instance.AudioManager.BGMVolume;
    //    //BGMVolumeSlider.onValueChanged.AddListener(delegate {
    //    //    OnBGMVolChanged();
    //    //});
    //    //UIVolumeSlider.value = GameManager.Instance.AudioManager.UIVolume;
    //    //UIVolumeSlider.onValueChanged.AddListener(delegate {
    //    //    OnUIVolChanged();
    //    //});
    //    //GameplayVolumeSlider.value = GameManager.Instance.AudioManager.GameplayVolume;
    //    //GameplayVolumeSlider.onValueChanged.AddListener(delegate {
    //    //    OnGameplayVolChanged();

    //    //});
    //}

    //public void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        OnQuitSetting();
    //    }
    //}


    //public void OnQuitSetting()
    //{
    //    //GameManager.Instance.UIManager.Pop();
    //    //GameManager.Instance.AudioManager.PlayButtonClickedAudio();
    //}
}
