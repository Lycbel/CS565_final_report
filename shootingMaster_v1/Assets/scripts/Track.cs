using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Track 
{

    public int fileName;
    public int mode; //0 for shoot dis 1 for shoot  not dis 3 for head shot 2 for trace 
    public weaponControl user;
    public People target;
    private Canvas dotPlace;
    private GameObject prefabDot;
    private bool objectAlive = true;
    public int totalObject=1;
    private int totalShot;
    private int totalHit;
    private int totalDmg;
    private int totalHead;
    private List<Vector3> targetStartPositions;
    private List<Vector3> targetShotPositions ;
    private List<Vector3> viewShotDirection;
    private List<Vector3> viewStartDirection;
    private List<Vector3> userPositions ;
    private List<bool> hits;
    private List<float> targetShowUpTime;
    private List<Vector3> points;
    private float angleMulti = 5f;
    private float totalOff = 0;
    private float reaction = 0;
    private Queue<Vector3> userHistPosition;
    private int sec = 2;
    public Camera cam =null;

    private int totalShot1;
    private int totalHit1;

    private float timeInterval =0.1f;
    private float timeNext = 0;
    private float timeInterval1 = 7f;
    
    private float timeNext1 = 0;
    private float traceLeft = 0;
    private float traceTotal = 0.05f;
    private float ontarget = 0;
    private float offtar = 0;
    private int pIRange = 2;
    private string dataPath = "Assets/data/";
  
    public Track(Canvas dot,GameObject pre,int mode,weaponControl u)
    {
        userHistPosition = new Queue<Vector3>();
        targetStartPositions = new List<Vector3>();
        targetShotPositions = new List<Vector3>();
        viewShotDirection = new List<Vector3>();
        userPositions = new List<Vector3>();
        viewStartDirection = new List<Vector3>();
        hits = new List<bool>();
        targetShowUpTime = new List<float>();
        points = new List<Vector3>();
        this.dotPlace = dot;
        this.prefabDot = pre;
        this.mode = mode;
        this.user = u;
        cam = u.cam;
        timeNext1 = Time.time + timeInterval1;



    }

   public float accUp = 0.7f;
   public float accDown = 0.3f;
    public float timeUpIn = 10;
    public void drawDot()
    {
        
        foreach (Vector3 p in points)
        {
        
            GameObject gbo = MonoBehaviour.Instantiate(prefabDot);
            gbo.transform.SetParent(dotPlace.transform,true);

            gbo.transform.position = dotPlace.transform.position + new Vector3(p.x, p.y, 0);
        }
    }
    public void setTarget(People p)
    {
        this.target = p;
    }
    public string finalResult()
    {
        totalObject = target.totlaObject;
        target.gameOver = true;
        switch (mode) {
            case 0:
                int len = viewShotDirection.Count;
                for (int i = 0; i < len; i++)
                {
                    float of = offset(viewStartDirection[i],viewShotDirection[i],userPositions[i] ,targetShotPositions[i]);
                    totalOff += of;
                    
                }
                drawDot();
                populate(getScore(), (totalHit + 0.0f) / totalShot, reaction, totalOff*10);
                break;
            case 1:
                int len1 = viewShotDirection.Count;
                for (int i = 0; i < len1; i++)
                {
                    float of = offset(viewStartDirection[i], viewShotDirection[i], userPositions[i], targetShotPositions[i]);
                    totalOff += of;

                }
                drawDot();
                break;
            case 2:
                drawDot();
                break;


        }

       
        return toString();
    }
    public void atUpdate(float time, float delt)
    {
        switch (mode)
        {
            case 0:
                if (timeNext1 <= time&&totalShot1>=6)
                {

                    timeNext1 += timeInterval1;
                    if(totalHit1/ totalShot1<accDown)//bad
                    {

                        if (pIRange > 0)
                        {
                            pIRange--;
                        }
                        Debug.Log("bad");
                        target.upByPer(pIRange,1.1f,1.1f,1.1f);
                    }
                    if(accUp<totalHit1 / totalShot1)//good
                    {
                        if (pIRange < staticConfig.rangeArr.Length)
                        {
                            pIRange++;
                        }
                        Debug.Log("goood");
                        target.upByPer(pIRange, 0.9f, 0.9f, 0.9f);
                    }
                    totalShot1 =0;
                    totalHit1 =0;

                    }
                
                break;
            case 1:
                if (timeNext <= time)
                {

                    timeNext += timeInterval;
                    addUserHist(user.cam.transform.forward);
                }
               
                else
                {
                    
                }
                break;
            case 2:
                break;



        }

    }
    public void atShoot(float time,float delt,bool head,bool hitted,int dmg)
    {
        
        if (target.alive)
        {
           
            switch (mode)
            {
                case 0:
                    
                    totalShot += 1;
                    if (hitted)
                    {
                        
                        totalHit += 1;
                    }
                    
                  
                    totalShot1 += 1;
                    if (hitted)
                    {
                        totalHit1 += 1;
                    }
                    hits.Add(hitted);
                    targetStartPositions.Add(target.startPosition);
                    targetShotPositions.Add(target.transform.position);
                    reaction += Time.time - target.showUp;
                    viewShotDirection.Add(user.cam.transform.forward);
                    viewStartDirection.Add(target.viewStartDirection);
                    userPositions.Add(user.transform.position);
                    
                    if (hitted)
                    {
                        points.Add(getpoint(target.transform.position) / 2);
                    }
                    else
                    {
                        points.Add(getpoint(target.transform.position));
                    }

                    break;
                case 1:
                    totalShot += 1;
                    if (hitted)
                    {
                        totalHit += 1;
                    }
                    hits.Add(hitted);
                    //targetStartPositions.Add(target.startPosition);
                    targetShotPositions.Add(target.transform.position);
                    //reaction += Time.time - target.showUp;
                    viewShotDirection.Add(user.cam.transform.forward);
                    viewStartDirection.Add(userHistPosition.Dequeue());
                    userPositions.Add(user.transform.position);
                    if (hitted)
                    {
                        points.Add(getpoint(target.transform.position)/2);
                    }
                    else
                    {
                        points.Add(getpoint(target.transform.position));
                    }
                    
                    break;
                case 2:
                    offtar += 0.0001f;
                    if (hitted)
                    {
                        ontarget += 0.0001f;
                    }
                    else
                    {
                       
                    }
                   
                    if (Time.time < traceLeft)
                    {

                    }
                    else
                    {
                      
                        points.Add(getpoint(target.transform.position));
                        traceLeft =Time.time+ traceTotal;
                       
                    }
                    break;



            }

        }
      

    }




    public string toString()
    {
        switch (mode)
        {
            case 0:
                return "Accuracy:  " + (totalHit + 0.0f) / totalShot + "\n" +
            "Average reaction time:  " + reaction / totalShot + "\n" +
            "total offset:  " + (totalOff) + "\n" +
            "total shoot:  " + totalShot + "\n" +
            "total miss:  " + (totalShot - totalHit) + "\n" +
            "total object miss:  " + (totalObject - totalHit) + "\n" +
            "overall score:  " + getScore();
               
            case 1:
                
                return "Accuracy:  " + (totalHit + 0.0f) / totalShot + "\n" +
            "Average reaction time:  " + "not available" + "\n" +
            "total offset:  " + (totalOff) + "\n" +
            "total shoot:  " + totalShot + "\n" +
            "total miss:  " + (totalShot - totalHit) + "\n" +
            "overall score:  " + getScore();
            case 2:
                Debug.Log("ss"+ ontarget / offtar);
                return "Accuracy:  " + (ontarget ) / offtar + "\n" +
               "overall score:  " + (200+ ((ontarget / offtar)*3500 ));
                

        }

        return "";
    }

    public float[] twoAngle(Vector3 start, Vector3 end, Vector3 uP, Vector3 oP)
    {
        Vector3 trueDir = oP - uP;
        float lenth = trueDir.sqrMagnitude;

        float dot = Vector3.Dot(start, trueDir);
        float len = start.magnitude * trueDir.magnitude;
        float anShould = Mathf.Acos(dot / len);

        float dot1 = Vector3.Dot(start, end);
        float len1 = start.magnitude * end.magnitude;
        float anTrue1 = Mathf.Acos(dot1 / len1);


        return new float[]{ anShould, anTrue1 };


    }
    public float offset(Vector3 start,Vector3 end,Vector3 uP,Vector3 oP)
    {


        float[]angs =twoAngle( start,  end,  uP,  oP);

        return angleMulti*(angs[1]-angs[0])/(0.5f+ angs[0]/3);
    }

    public Vector3 getpoint(Vector3 position)
    {
        Vector3 screenPos = user.cam.WorldToScreenPoint(position);
        int w = Screen.currentResolution.width;
        int h = Screen.currentResolution.height;
       
        Vector3 mid = new Vector3(w/2.0f,h/2.0f,0);
        float max = w / 18.0f;
       
        Vector3 vec = (mid - screenPos);
       
        float len = vec.magnitude;
        float nemag = (len - len * len/ w );
        return vec*nemag/len;
       
       
        
    }
    public float offsetRatio(Vector3 start, Vector3 end, Vector3 uP, Vector3 oP)
    {
        float[] angs = twoAngle(start, end, uP, oP);


        return (angs[1] /( angs[0]+0.01f));
    }

    public int getScore()
    {
        return (int)(((totalHit + 0.3f) / totalShot)*1200 + (reaction / totalShot) * 800)/ ((pIRange+5)/(5));
    }
    public void addUserHist(Vector3 ec)
    {
        if (userHistPosition.Count == sec)
        {
            userHistPosition.Dequeue();

        }
        userHistPosition.Enqueue(ec);
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
    public FileInfo[] GetAllFiles()
    {
        DirectoryInfo d = new DirectoryInfo(dataPath);//Assuming Test is your Folder
        FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
        return Files;
    }
    private int GetFileID(string filename)
    {
        return int.Parse(filename.Split('.')[0].Substring(4));
    }

    public void populate(float score,float hitrate, float reftime, float offsetratio)
    {
        int id = GetCurrentFileID() + 1;
        string newLine = score.ToString() + "," + hitrate.ToString() + "," + (reftime/1.5f).ToString() + "," + offsetratio.ToString();
        File.WriteAllText(dataPath + "file" + id + ".txt", newLine);
        Debug.Log(newLine);

    }








}
