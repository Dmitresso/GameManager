using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Data;
using UnityEngine;


public class Tracker : Singleton<Tracker> {
    [SerializeField] private TextAsset log;

    private string fileName;
    private List<(string stateFrom, string stateTo, string caller, int sourceLineNumber, string time)> transitionOperations;
    private List<(string eventName, string caller, int sourceLineNumber, string time)> events;


    public bool ReportCompleted { get; set; }

    private void Awake() {
        Init();
    }

    
    private void Init() {
        transitionOperations = new List<(string stateFrom, string stateTo, string caller, int sourceLineNumber, string time)>();
        events = new List<(string eventName, string caller, int sourceLineNumber, string time)>();
        //if (log == null) { log = new TextAsset(); }

        fileName = Settings.appName + " " + Settings.dateFormat + ".log";
    }


    private void Write(string line) {
        try {
            using StreamWriter sw = new StreamWriter(new FileStream("Assets/Logs/" + fileName, FileMode.OpenOrCreate, FileAccess.Write));
            sw.Write(line);
        }
        catch (Exception e) {
            Debug.Log(e);
        }
    }
    
    public void LogStateTransitionOpInfo(string stateFrom, string stateTo, [CallerMemberName] string caller = "", [CallerLineNumber] int sourceLineNumber = 0) {
        transitionOperations.Add((stateFrom, stateTo, caller, sourceLineNumber, DateTime.Now.ToString(Settings.dateTimeFormat)));
    }

    public void Log(string eventName, [CallerMemberName] string caller = "", [CallerLineNumber] int sourceLineNumber = 0) {
        events.Add((eventName, caller, sourceLineNumber, DateTime.Now.ToString(Settings.dateTimeFormat)));
    }

    public void WriteAllEvents() {
        foreach (var s in events) {
            Debug.Log(s);
        }
        ReportCompleted = true;
    }
}