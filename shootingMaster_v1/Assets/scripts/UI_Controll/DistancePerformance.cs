using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DistancePerformance : MonoBehaviour {

    public DD_DataDiagram historyChart;

    private GameObject yourDPLine;
    private GameObject avgDPLine;
    private string dataPath = "Assets/history/";

    // Use this for initialization
    void Start()
    {
        yourDPLine = historyChart.AddLine("yours", Color.green);
        avgDPLine = historyChart.AddLine("global average", Color.yellow);
    }

    public void UpdateData()
    {
        historyChart.DestroyLine(yourDPLine);
        historyChart.DestroyLine(avgDPLine);

        yourDPLine = historyChart.AddLine("yours", Color.green);
        avgDPLine = historyChart.AddLine("globalAverage", Color.yellow);

        LoadDPData();
    }

    private void LoadDPData()
    {
        StreamReader sr = new StreamReader(dataPath + "distancePerformance.txt");
        string line = sr.ReadLine();
        string[] values = line.Split(',');

        int i;
        int s;
        for (i = 13; i > 6; i--)
        {
            s = int.Parse(values[i]);
            historyChart.InputPoint(avgDPLine, new Vector2(1.0f, normalizePerformance(s)));
        }
        for (i = 6; i > -1; i--)
        {
            s = int.Parse(values[i]);
            historyChart.InputPoint(yourDPLine, new Vector2(1.0f, normalizePerformance(s)));
        }

        sr.Close();
    }

    private float normalizePerformance(int s)
    {
        return  (0.00917f * s - 14.38f)-0.8f;
    }

}
