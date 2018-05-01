using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponControl : MonoBehaviour {
    public Animator anim;//xxxx
    public GameObject pre;
    public Canvas dotPlace;
    public GameObject pann;
    private double shootInterval;
    private int dmg;
    private float timef = 0.3f;//fire interval
    private float time;
    public Camera cam;
    public Text te;
    private float soundInterval;
    public AudioSource gunAu;
    public AudioSource gunHit;
    public bool trace = false;
    public float totalTime;
    private bool shootDis = staticConfig.shootDisapear;
    private static bool over = false;
    public static Track track;
   

	// Use this for initialization
	void Start () {
        
        over = false;
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.GameOver = false;
       
        totalTime = staticConfig.totalTime;
        trace = staticConfig.isTrace;
        anim = GetComponentInChildren<Animator>();
        timef = staticConfig.fireRate;
        soundInterval = 0.7f;
        cam =GetComponentInParent<Camera>();
        var audios = GetComponents<AudioSource>();
        gunAu = audios[1];
        gunHit = audios[0];
        dmg = staticConfig.dmg;
        track = new Track(dotPlace, pre, staticConfig.mode, this);

    }
	
	// Update is called once per frame
	void Update () {
        if (over)
        {
            return;
        }
        totalTime -= Time.deltaTime;
        track.atUpdate(Time.time,Time.deltaTime);
        if (totalTime <= 0)
        {
            gameOver();
        }
        if (!trace)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > time)
            {
                
                fire();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                traceFire();
            }
        }

	}
    public void gameOver()
    {
        
        pann.SetActive(true);
        over = true;
        UnityStandardAssets.Characters.FirstPerson.FirstPersonController.GameOver = true ;
        te.text= track.finalResult();
    }

    private void fire()
    {
       
        RaycastHit hit;
        StartCoroutine(gunEffectMiss());
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {

            
            ShootableObject p = hit.collider.GetComponent<ShootableObject>();
            People pe = hit.collider.GetComponent<People>();
            if (p != null)
            {
                //StartCoroutine(gunEffectHit());
                bool h =p.gotHit(dmg);
                int dmg2 = dmg;
                if (h)
                {
                    dmg2 = dmg2 * 2;
                }
                track.atShoot(Time.time,Time.deltaTime,h,true,dmg2);

            }
            else if(pe!=null) {
                pe.gotHit(dmg);
                track.atShoot(Time.time, Time.deltaTime, false, true, dmg);
                //Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            }
           

            else if (shootDis)
            {
                objectControl.killAll();
                track.atShoot(Time.time, Time.deltaTime, false, false, 0);
            }
            else
            {
                track.atShoot(Time.time, Time.deltaTime, false, false, 0);
            }

        }
        else if (shootDis) // if it is shootDis bullet must be able to kill a full health target
        {
            
            objectControl.killAll();
            track.atShoot(Time.time, Time.deltaTime, false, false, 0);
        }
        
        StartCoroutine(gunAnim());
        



        time = timef+ Time.time;
    }

    private void traceFire()
    {
        RaycastHit hit;
        if(!trace)
        StartCoroutine(gunEffectMiss());//todo
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {

           
            ShootableObject p = hit.collider.GetComponent<ShootableObject>();
            People pe = hit.collider.GetComponent<People>();
            if (p != null)
            {
                
                //StartCoroutine(gunEffectHit());
                p.gotHit(dmg);

            }
            else if (pe != null)
            {
                track.atShoot(Time.time, Time.deltaTime, false, true, 0);
                pe.gotHit(dmg);
                //Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            }
            else
            {
                track.atShoot(Time.time, Time.deltaTime, false, false, 0);
            }





        }
        else
        {
            track.atShoot(Time.time, Time.deltaTime, false, false, 0);
        }
        //StartCoroutine(gunAnim());

    }

    
    public IEnumerator gunEffectMiss()
    {
        
        gunAu.Play();
        yield return soundInterval;
        
    }
    public IEnumerator gunAnim()
    {
        float t1 = Time.time;
        anim.SetBool("Fire", true);
        
        yield return new WaitForSeconds(timef-0.05f);
        anim.SetBool("Fire", false);
    }
    public IEnumerator gunEffectHit()
    {
        gunHit.Play();
        yield return soundInterval;
    }
}
