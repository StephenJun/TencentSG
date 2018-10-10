using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingManager : Singleton<SettingManager> {
    
    public Settings setting;

    [SerializeField]
    Scrollbar BgmVolumeBar;
    [SerializeField]
    Scrollbar SoundVolumeBar;
	void Start () {
        JsonHandler.LoadFile(ref setting);
        Debug.Log(setting.BGMVolume);
	}
    void OnDisable()
    {
        Debug.Log(setting.BGMVolume);
        JsonHandler.SaveFile(setting);
    }
    public void OnSoundVolumeBarChange()
    {
        setting.SoundVolume = SoundVolumeBar.value;
    }
    public void OnBgmVolumeBarChange()
    {
        setting.BGMVolume = BgmVolumeBar.value;
    }
}
[System.Serializable]
public class Settings
{
    public double BGMVolume = 0.5f;
    public double SoundVolume = 0.5f;
    public bool bool1;
    public bool bool2;
    public Save save;
    public Settings()
    {
        save = new Save();
        save.a = 5;
    }
}
[System.Serializable]
public class Save
{
    public int a;
    int b;
    int c;
}
