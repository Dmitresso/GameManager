using System;
using System.Collections.Generic;
using Data;
using UnityEngine;


public class RegistryManager : Singleton<RegistryManager> {
    private List<(string op, string key, string value, string time)> registryOperations;
    
    
    private void Awake() {
        Init();
    }


    private void Init() {
        registryOperations = new List<(string op, string key, string value, string time)>();
    }

    
    
    /*
    public void SaveRegistryOpInfo(string operation, string key, dynamic value) {
        registryOperations.Add((operation, key, value.ToString(), DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")));
    }
    */
    
    public void SaveRegistryOpInfo(string operation, string key, string value) {
        registryOperations.Add((operation, key, value, DateTime.Now.ToString(Settings.dateTimeFormat)));
    }
    
    public void SaveRegistryOpInfo(string operation, string key, float value) {
        registryOperations.Add((operation, key, value.ToString(), DateTime.Now.ToString(Settings.dateTimeFormat)));
    }    
    
    public void SaveRegistryOpInfo(string operation, string key, int value) {
        registryOperations.Add((operation, key, value.ToString(), DateTime.Now.ToString(Settings.dateTimeFormat)));
    }

    public void SaveToRegistry(string key, string value, bool json = false) {
        PlayerPrefs.SetString(key, json ? JsonUtility.ToJson(value) : value);
        PlayerPrefs.Save();
        SaveRegistryOpInfo("w", key, value);
    }

    public void SaveToRegistry(string key, float value) {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
        SaveRegistryOpInfo("w", key, value);
    }
    
    public void SaveToRegistry(string key, int value) {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
        SaveRegistryOpInfo("w", key, value);
    }

    public string LoadStringFromRegistry(string key, bool json = false) {
        string s = PlayerPrefs.GetString(key);
        string value = json ? JsonUtility.FromJson<string>(s) : s;
        SaveRegistryOpInfo("r", key, value);
        return value;
    }
    
    public float LoadFloatFromRegistry(string key) {
        float value = PlayerPrefs.GetFloat(key);
        SaveRegistryOpInfo("r", key, value);
        return value;
    }

    public int LoadIntFromRegistry(string key) {
        int value = PlayerPrefs.GetInt(key);
        SaveRegistryOpInfo("r", key, value);
        return value;
    }

}