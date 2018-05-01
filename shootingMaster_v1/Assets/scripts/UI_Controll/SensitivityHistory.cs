using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SensitivityHistory : MonoBehaviour {

    public DD_DataDiagram historyChart;

    private GameObject performanceLine;
    private GameObject sensitivityLine;
    private GameObject offsetLine;
    private string dataPath = "Assets/history/";

    // Use this for initialization
    void Start()
    {
        performanceLine = historyChart.AddLine("performance", Color.green);
        sensitivityLine = historyChart.AddLine("sensitivity", Color.yellow);
        offsetLine = historyChart.AddLine("offset", Color.blue);
    }

    public void UpdateData()
    {
        historyChart.DestroyLine(sensitivityLine);
        historyChart.DestroyLine(performanceLine);
        historyChart.DestroyLine(offsetLine);

        sensitivityLine = historyChart.AddLine("sensitivity", Color.green);
        performanceLine = historyChart.AddLine("performance", Color.yellow);
        offsetLine = historyChart.AddLine("offset", Color.blue);
        Debug.Log("here");

        LoadSensitivityData();
        LoadPerformanceData();
        LoadOffsetData();
    }

    private void LoadSensitivityData()
    {
        StreamReader sr = new StreamReader(dataPath + "sensitivity.txt");
        string line = sr.ReadLine();
        string[] values = line.Split(',');

        int i;
        float s;
        for(i=values.Length-1; i>-1; i--)
        {
            s = float.Parse(values[i]);
            historyChart.InputPoint(sensitivityLine, new Vector2(1.0f, normalizeSensitivity(s)));
        }

        sr.Close();
    }

    private void LoadPerformanceData()
    {
        StreamReader sr = new StreamReader(dataPath + "performance.txt");
        string line = sr.ReadLine();
        string[] values = line.Split(',');

        int i;
        int p;
        for (i = values.Length - 1; i > -1; i--)
        {
            p = int.Parse(values[i]);
            historyChart.InputPoint(performanceLine, new Vector2(1.0f, normalizePerformance(p)));
        }

        sr.Close();
    }

    private void LoadOffsetData()
    {
        StreamReader sr = new StreamReader(dataPath + "offset.txt");
        string line = sr.ReadLine();
        string[] values = line.Split(',');
        Debug.Log(line);

        int i;
        int o;
        for (i = values.Length - 1; i > -1; i--)
        {
            o = int.Parse(values[i]);
            historyChart.InputPoint(offsetLine, new Vector2(1.0f, normalizeOffset(o)));
        }

        sr.Close();
    }



    private float normalizeSensitivity(float s)
    {
        return s - 7f;
    }

    private float normalizePerformance(int p)
    {
        return 0.0215f * p - 31.2258f;
    }

    private float normalizeOffset(int o)
    {
        return -0.25f * o + 8.0f; ;
    }
}
