using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonHandler
{
    static Dictionary<System.Type, string> dictionary = new Dictionary<System.Type, string>() { { typeof(Settings), "设置" } };
    static public T LoadFile<T>(ref T data) where T : new()
    {
        string filePath = Application.persistentDataPath + "/" + typeof(T).ToString() + ".txt";
        string typeName = dictionary.ContainsKey(typeof(T)) ? dictionary[typeof(T)] : "Unregeisted Type";
        if (File.Exists(filePath))
        {
            data = JsonMapper.ToObject<T>(File.ReadAllText(filePath));            
            Debug.Log("[" + typeName + "]读取成功...");
            return data;
        }
        else
        {
            SaveFile(data);
            Debug.Log("[" + typeName + "]初始化成功...");
            return data;
        }
    }

    static public T SaveFile<T>(T data)
    {
        string filePath = Application.persistentDataPath + "/" + typeof(T).ToString() + ".txt";
        string typeName = dictionary.ContainsKey(typeof(T)) ? dictionary[typeof(T)] : "Unregisted Type";
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(JsonMapper.ToJson(data));
        sw.Close();
        sw.Dispose();
        Debug.Log("[" + typeName + "]保存成功...");
        return data;
    }
}
