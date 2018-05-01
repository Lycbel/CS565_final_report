using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectControl : MonoBehaviour {
    private  List<GameObject> gb;
    public static List<People> go ;// to store all objects 

    private List<float> times;
    private int total = 0;
	// Use this for initialization
	void Start () {
        times = new List<float>();
        gameObject.tag = "objectControl";
         gb = new List<GameObject>();
     go = new List<People>();// to store all objects 

}
	
	public void restartRegister( GameObject gbp,float time)
    {
        gb.Add(gbp);
        times.Add(time);
        total++;

    }
    public void OnDisable()
    {
        go = null;
    }
    public static void registerSelf(People gbs)
    {
      
        go.Add(gbs);
    }
     void Update()
    {
       for(int i = 0; i < total; i++)
        {
            if ((times[i] -= Time.deltaTime) <= 0)
            {
                gb[i].SetActive(true);
                times.RemoveAt(i);
                gb.RemoveAt(i);
                total--;
            }
        }
    }

    public static void killAll()
    {
        foreach(People p in go)
        {
            print("killed");
            if (p.isActiveAndEnabled)
            {
                p.killSelf();
            }
        }
    }


 


}
