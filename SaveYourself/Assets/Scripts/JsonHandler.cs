using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
public class JsonHandler {
    static Dictionary<System.Type, string> dictionary = new Dictionary<System.Type, string>() { {typeof(Settings),"设置"} };
	static public T LoadFile<T>(T setting) where T : new()
    {
        string filePath = Application.persistentDataPath + "/" + typeof(T).ToString() + ".txt";
        if (File.Exists(filePath))
        {
            setting = JsonMapper.ToObject<T>(File.ReadAllText(filePath));
            Debug.Log("["+ dictionary[typeof(T)] +"]读取成功...");
            return setting;
        }
        else
        {
            SaveFile(setting);
            Debug.Log("[" + dictionary[typeof(T)] + "]初始化成功...");
            return setting;
        }
        
    }
    static public T SaveFile<T>(T data) where T : new()
    {
        string filePath = Application.persistentDataPath + "/" + typeof(T).ToString() + ".txt";
        data = new T();
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(JsonMapper.ToJson(data));
        sw.Close();
        sw.Dispose();
        return data;
    }
}
