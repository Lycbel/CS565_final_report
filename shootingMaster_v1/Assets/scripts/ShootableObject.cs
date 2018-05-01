using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableObject : MonoBehaviour {
    public int dmgToMe;
    
    public People parent;
    // Use this for initialization
    void Start () {
        parent = transform.parent.GetComponent<People>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool gotHit(int i)
    {
        
        parent.gotHit(i*dmgToMe);
        return (i==2);
    }
}
