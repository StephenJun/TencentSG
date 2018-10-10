using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : Manager<SettingManager> {
    
    public Settings setting;
	void Start () {
        JsonHandler.LoadFile(setting);
	}
    void OnDestory()
    {
        JsonHandler.SaveFile(setting);
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
