using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Unity.Profiling;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{

    // for fps calculation.
    private int frameCount;
    private float elapsedTime;
    private double frameRate;

    // initialization
    string displayText;
    ProfilerRecorder memoryRecorder;


    void OnEnable()
    {
        // creating a memory recorder
        memoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
    }

    void OnDisable()
    {
        memoryRecorder.Dispose();
    }


    void Update()
    {


        // fps calculation
        frameCount++;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5f)
        {
            frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);
            frameCount = 0;
            elapsedTime = 0;
        }

        // creating display text 
        var text = new StringBuilder(200);
        if (memoryRecorder.Valid)
        {
            text.AppendLine($"Total Memory Used: {memoryRecorder.LastValue / 1024 / 1024} MB");
        }
        text.AppendLine($"FPS: {frameRate}");

        displayText = text.ToString();
    }

    void OnGUI()
    {
        // displaying the stats on screen  (GUI)
        GUI.TextArea(new Rect(10, 30, 190, 50), displayText);
    }
}
