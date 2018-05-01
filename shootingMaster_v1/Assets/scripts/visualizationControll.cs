using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class visualizationControll : MonoBehaviour {

    public DD_DataDiagram chart;
    public GameObject buttonPrefab;
    public GameObject panelToAttachButtonsTo;
    public Dropdown periodSelection;

    private GameObject performanceLine;
    private GameObject hitRate;
    private GameObject reflectionTime;
    private GameObject offsetRatio;
    private string dataPath = "Assets/data/";

	// Use this for initialization
	void Start () {
        performanceLine = chart.AddLine("performance", Color.green);
        hitRate = chart.AddLine("hitRate", Color.blue);
        reflectionTime = chart.AddLine("reflectionTime", Color.cyan);
        offsetRatio = chart.AddLine("offsetRatio", Color.yellow);
    }

    public void UpdateData()
    {
        chart.DestroyLine(performanceLine);
        chart.DestroyLine(hitRate);
        chart.DestroyLine(reflectionTime);
        chart.DestroyLine(offsetRatio);
        
        performanceLine = chart.AddLine("performance", Color.green);
        hitRate = chart.AddLine("hitRate", Color.blue);
        reflectionTime = chart.AddLine("reflectionTime", Color.cyan);
        offsetRatio = chart.AddLine("offsetRatio", Color.yellow);
        Debug.Log("here");
        int curID;

        for (curID = GetCurrentFileID(); curID > 0; curID -= 1)
        {
           LoadDataIntoChart(curID, 1.0f);
        }
    }

    public void UpdateLastWeekData()
    {
        chart.DestroyLine(performanceLine);
        chart.DestroyLine(hitRate);
        chart.DestroyLine(reflectionTime);
        chart.DestroyLine(offsetRatio);

        performanceLine = chart.AddLine("performance", Color.green);
        hitRate = chart.AddLine("hitRate", Color.blue);
        reflectionTime = chart.AddLine("reflectionTime", Color.cyan);
        offsetRatio = chart.AddLine("offsetRatio", Color.yellow);

        int count;
        int curID = GetCurrentFileID();

        for (count=0; count < 8; count += 1)
        {
            LoadDataIntoChart(curID-count, 1.0f);
        }
    }

    public void onChangePeriod()
    {
        if (periodSelection.value == 0)
        {
            UpdateData();
        }
        else if (periodSelection.value == 1)
        {
            UpdateLastWeekData();
        }
    }

    public void onBackResetPeriod()
    {
        periodSelection.value = 0;
    }

    public FileInfo[] GetAllFiles()
    {
        DirectoryInfo d = new DirectoryInfo(dataPath);//Assuming Test is your Folder
        FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
        return Files;
    }

    public void populate()
    {
        int id = GetCurrentFileID() + 1;
        int score = (int)Random.Range(0f, 2000.0f);
        float hitrate = Random.Range(0.2f, 0.5f);
        float reftime = Random.Range(0.05f, 0.2f);
        float offsetratio = Random.Range(10f, 60f);
        string newLine = score.ToString() + "," + hitrate.ToString() + "," + reftime.ToString() + "," + offsetratio.ToString();
        File.WriteAllText(dataPath + "file"+id+".txt", newLine);
        Debug.Log(newLine);

    }

    private int GetCurrentFileID()
    {
        FileInfo[] Files = GetAllFiles();
        int max = 1;
        foreach (FileInfo file in Files)
        {
            int cur = GetFileID(file.Name);
            if (cur >= max)
            {
                max = cur;
            }
        }
        return max;
    }

    private int GetFileID(string filename)
    {
        return int.Parse(filename.Split('.')[0].Substring(4));
    }

    private void LoadDataIntoChart(int ID, float x)
    {
        StreamReader sr = new StreamReader(dataPath + "file" + ID + ".txt");
        string line = sr.ReadLine();
        string[] values = line.Split(',');
        int p = int.Parse(values[0]);
        float h = float.Parse(values[1])-0.05f;
        float r = float.Parse(values[2]);
        float o = float.Parse(values[3]);

        chart.InputPoint(performanceLine, new Vector2(x, normalizeP(p)));
        chart.InputPoint(hitRate, new Vector2(x, normalizeH(h)));
        chart.InputPoint(reflectionTime, new Vector2(1f, normalizeR(r)));
        chart.InputPoint(offsetRatio, new Vector2(1f, normalizeO(o)+1.5f));
        sr.Close();
    }

    private float normalizeP(int p)
    {
        return 0.00917f * p - 14.38f;
    }

    private float normalizeH(float h)
    {
        return 22.5f * h - 7.25f;
    }

    private float normalizeR(float r)
    {
        return -36.3636f * r + 12.3636f;
    }

    private float normalizeO(float o)
    {
        return -0.5f * o + 8.0f;
    }

    void CreateButton(string text, float x, float y)//Creates a button and sets it up
    {
        GameObject button = (GameObject)Instantiate(buttonPrefab);
        button.transform.position = new Vector3(x, y, 0f);
        button.transform.localScale += new Vector3(-0.9f, -0.9f, 0);
        button.transform.SetParent(panelToAttachButtonsTo.transform);//Setting button parent
        button.GetComponent<Button>().onClick.AddListener(OnClick);//Setting what button does when clicked
                                                                   //Next line assumes button has child with text as first gameobject like button created from GameObject->UI->Button
        button.transform.GetChild(0).GetComponent<Text>().text = "test" + text;//Changing text
    }
    void OnClick()
    {
        Debug.Log("clicked!");
    }

}
