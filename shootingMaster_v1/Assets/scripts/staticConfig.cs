using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public  class staticConfig : MonoBehaviour {
    public static bool headShot = false;
    public static int mode = 0;
    public static float objectSpeed = 1.3f;
    public static bool objectMove = false;
    public static float sensitive = 1.5f;
    public static float jumpSpeed = 1;
    public static float objectScale = 0.6f;
    public static float totalTime = 20f;
    public static int dmg = 100;

    public static bool healthEnable = true;
    public static float fireRate = 0.3f;
    public static bool isTrace = false;
    public static int health = 2;

    public static float NextInterval = 3.0f; // if not shoot diable mode, the total time wait for to update z location.
    public static float respawnInterval = 0.6f;
    //if move
    public static float maxMoveInterval = 0.9f;
    public static float minMoveInterval = 0.3f;
    public static int[] moveDistribution = new int[]{10, 10,2, 2};// left right up down
    public static int[] rangeDistribution = new int[] { 5, 5, 3, 3, 1, 1 };
    public static int[] rangeArr = new int[] { 10, 10, 1, 1, 1, 1 };
    public static int robot = 0;
    public static int ball = 1;
    public static bool shootDisapear = true;

    private void Start()
    {
        aggRange();
       
    }
    /*for shoot and disappear need to set healthenable to True  xxxxxxxx
   *  if it is shootDis bullet must be able to kill a full health target
   */
    


    // config.txt format: "sensitive,"
    private static string configFile = "Assets/scripts/config.txt";

    
    public static void shootDis()
    {
        ball = 1;
        robot = 0;
        headShot = false;
        dmg = 100;
        healthEnable = true;
        health = 2; 
        fireRate = 0.4f;
        mode = 0;
        shootDisapear = true;
        isTrace = false;


    }
    public static void normalShoot(int healt)
    {
        ball = 1;
        robot = 0;
        healthEnable = true;
        headShot = false;
        dmg = 1;
        healthEnable = true;
        health = healt;
        fireRate = 0.4f;
        mode = 1;
        shootDisapear = false;
        isTrace = false;
        fireRate = 0.2f;
        

    }
    public static void traceM()
    {
        ball = 1;
        robot = 0;
        isTrace = true;
        healthEnable = false;
        headShot = false;
        fireRate = 0.0f;
        mode = 2;
        shootDisapear = false;


    }
    public static void aggRange()
    {
        print("dd");
        rangeArr[0] = rangeDistribution[0];
        for (int i = 1; i < rangeDistribution.Length; i++)
        {
            rangeArr[i] = rangeDistribution[i] + rangeArr[i - 1];
        }
    }
    public static void LoadValue()
    {
        StreamReader sr = new StreamReader(configFile);
        string line = sr.ReadLine();
        string[] values = line.Split(',');
        float sensitive_ = float.Parse(values[0]);
        sensitive = sensitive_;
        sr.Close();
    }

    public static void UpdateValue()
    {
        string[] arrLine = File.ReadAllLines(configFile);
        string newLine = sensitive.ToString() + ",";
        arrLine[0] = newLine;
        File.Create(configFile).Dispose();
        File.WriteAllLines(configFile, arrLine);
    }
}