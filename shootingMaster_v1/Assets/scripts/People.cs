using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour {
    public int health;
    public AudioSource aud;
    private AudioSource showUpAudio;
    //public float NextTimeInterval;
    private float interval = 0.5f;
    private objectControl gameControl;

    private bool isStart = true;
    private GameObject player;
    private int x = 8;
    private int z = 28;
    private Renderer[] renders;
    private float speed = 0.0f;
    private float totalTime = 0.0f;
    private Material mt; // now for only ball 
    private Color incolorMt;

    //new
    public bool alive = false;
    public float showUp = 0;
    public Vector3 startPosition;
    public Vector3 viewStartDirection;
    //if move able
    private float upBound = 2;
    public float maxTimeInterval = 0.9f;
    public float minTimeInterval = 0.2f;
    private float tempTimeLeft = 0;
    private Vector3 direction = new Vector3(0, 0, 0); // 0: left 1: right 2: up 3: down
    private bool healthEnable = true;
    private int[] moveDistri;
    private int[] moveAggre = new int[4];
    private int totalMoveCount = 0;
    private int startHealth = 0;
    //is trace
    private bool trace = false;
    private bool gotHitted = false;
    private bool shootdis = staticConfig.shootDisapear;
    public int totlaObject = 0;

    public bool gameOver = false;


    public float nextLeft;
    // Use this for initialization
    void Start() {

        
        if (gameControl == null)
            gameControl = GameObject.FindWithTag("objectControl").GetComponent<objectControl>();
        startHealth = staticConfig.health;
        nextLeft = staticConfig.NextInterval;
        trace = staticConfig.isTrace;
        speed = staticConfig.objectSpeed;
        totalTime = staticConfig.totalTime;
        healthEnable = staticConfig.healthEnable;
        maxTimeInterval = staticConfig.maxMoveInterval;
        minTimeInterval = staticConfig.minMoveInterval;
        upBound = Mathf.Max(upBound, maxTimeInterval * speed * 2);
        moveDistri = staticConfig.moveDistribution;
        alive = true;
        for (int i = 0; i < 4; i++)
        {
            totalMoveCount += moveDistri[i];
            if (i != 0)
                moveAggre[i] += moveAggre[i - 1] + moveDistri[i];
            else
                moveAggre[i] = moveDistri[i];
        }

        aud = GetComponent<AudioSource>();
        showUpAudio = GetComponents<AudioSource>()[1];
        gameObject.transform.localScale = gameObject.transform.localScale * staticConfig.objectScale;
        //NextTimeInterval = 0.8f;

        startHealth = staticConfig.health;
        player = GameObject.FindWithTag("MainCamera");
        renders = getRenders();
        resetMoveMent();

        if (staticConfig.robot == 0)
        {
            ShootableObject[] gb = gameObject.GetComponentsInChildren<ShootableObject>();
            if (gb.Length >= 4)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        if (staticConfig.ball == 0)
        {
            ShootableObject[] gb = gameObject.GetComponentsInChildren<ShootableObject>();
            if (gb.Length <= 5)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        if (staticConfig.headShot)
        {
            ShootableObject[] gb = gameObject.GetComponentsInChildren<ShootableObject>();
            if (gb.Length <= 5)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        if (trace)
        {
            mt = gameObject.GetComponent<Renderer>().material;
            incolorMt = mt.color;
        }
        StartCoroutine(waitObjectOconntrol());
        StartCoroutine(waitTrack());
        




    }

    public void upByPer(int r,float sc,float maxt,float minet)
    {
        print("up"+r);
        staticConfig.rangeDistribution = ranges[r];
        staticConfig.aggRange();
        transform.localScale *= sc;
        maxTimeInterval *= maxt;
        minTimeInterval *= minet;


    }
    private void Awake()
    {


    }

    // Update is called once per frame
    void Update() {
        move();
        if (gameOver)
        {
            return;
        }
        nextLeft -= Time.deltaTime;
        if (nextLeft <= 0)
        {
            killSelf();
        }

    }
    private void LateUpdate()
    {
        if (trace)
        {
            if (gotHitted)
            {
                mt.color = Color.red;
                gotHitted = false;
            }
            else
            {
                mt.color = incolorMt;
            }
        }
    }

    IEnumerator waitObjectOconntrol()
    {
        yield return new WaitUntil(() => gameControl != null);
        print("wait object");
        objectControl.registerSelf(this);
       

    }
    public void gotHit(int dmg)
    {

        health -= dmg;

        if (trace)
        {
            gotHitted = true;
        }
        else
        {
            StartCoroutine(play(aud));
        }
        if (healthEnable && health <= 0)
        {
            if (gameControl == null)
                gameControl = GameObject.FindWithTag("objectControl").GetComponent<objectControl>();
            StartCoroutine(Inactive());

        }

    }

    public void killSelf()
    {
        if (gameControl == null)
            gameControl = GameObject.FindWithTag("objectControl").GetComponent<objectControl>();
        StartCoroutine(Inactive());
    }
    void OnEnable()
    {
        nextLeft = staticConfig.NextInterval;
        totlaObject += 1;
        if (isStart)
        {
            
            isStart = false;
        }
        else
        {
            forTrack(1);

            restart();
        }


    }

    public void forTrack(int i)
    {

        startPosition = transform.position;
        viewStartDirection = weaponControl.track.cam.transform.forward;
        showUp = Time.time;
        alive = true;
    }
    public void restart()
    {
        float[] ran = getRange();

        float nx = 3 + x * ran[0] / z;
        gameObject.transform.position = new Vector3(Random.Range(-nx, nx), 0.5f, ran[0]);
        resetMoveMent();// will check movement enabled or not
        activeRenders();
        StartCoroutine(play(showUpAudio));
        health = startHealth;
        forTrack(2);

    }

    private IEnumerator play(AudioSource auds)
    {
        auds.Play();
        yield return interval;
    }
    private IEnumerator Inactive()
    {
        inactiveRenders();
        yield return new WaitForSeconds(interval);
        gameObject.SetActive(false);
        gameControl.restartRegister(gameObject, staticConfig.respawnInterval);
    }
    private IEnumerator waitTrack()
    {
        yield return new WaitUntil(() => weaponControl.track != null);
        weaponControl.track.setTarget(this);
        forTrack(3);


    }
    private void print(string content)
    {
        Debug.Log(content);
    }



    private void resetMoveMent()
    {
        if (speed != 0) {
            tempTimeLeft = Random.Range(minTimeInterval, maxTimeInterval);
            Vector3 pos = gameObject.transform.position;

            int moveNumber = (int)Mathf.Floor(Random.Range(0, totalMoveCount));

            while (moveNumber == totalMoveCount)
            {
                moveNumber = (int)Mathf.Floor(Random.Range(0, totalMoveCount));
            }

            if (moveNumber < moveAggre[0])
            {

                if (pos.x - tempTimeLeft * speed > -x)
                {
                    direction = Move.moveLeft();
                }
                else
                {
                    direction = Move.moveRight();
                }

            }
            else if (moveNumber < moveAggre[1])
            {
                if (pos.x + tempTimeLeft * speed < x)
                {
                    direction = Move.moveRight();
                }
                else
                {
                    direction = Move.moveLeft();
                }

            }
            else if (moveNumber < moveAggre[2])
            {
                if (pos.y + tempTimeLeft * speed <= upBound)
                {
                    direction = Move.moveUp();
                }
                else
                {
                    direction = Move.moveDown();
                }

            }
            else
            {
                if (pos.y - tempTimeLeft * speed >= 0)
                {
                    direction = Move.moveDown();
                }
                else
                {
                    direction = Move.moveUp();
                }

            }


        }
    }
    private void move()
    {
        if (speed != 0)
        {
            gameObject.transform.position = gameObject.transform.position + direction * speed * Time.deltaTime;
            tempTimeLeft -= Time.deltaTime;
            if (tempTimeLeft <= 0)
            {
                resetMoveMent();
            }
        }
    }

    /*
     * Reander part
     */
    private Renderer[] getRenders()
    {
        var eles = GetComponent<Renderer>();
        if (eles == null)
        {
            return GetComponentsInChildren<Renderer>();
        }
        var m = new Renderer[1];
        m[0] = eles;
        return m;
    }
    private void activeRenders()
    {
        foreach (Renderer r in renders)
        {
            r.enabled = true;
        }
    }
    private void inactiveRenders()
    {
        foreach (Renderer r in renders)
        {
            r.enabled = false;
        }
    }
    private float[] getRange()
    {
        float rnu = Mathf.Floor(Random.Range(0, staticConfig.rangeArr[staticConfig.rangeArr.Length - 1]) - 0.1f);
        int finalChoice = 0;
        float gsingle = z / staticConfig.rangeArr.Length;
        for (int i = 0; i < staticConfig.rangeArr.Length; i++)
        {
            if (rnu < staticConfig.rangeArr[i])
            {
                finalChoice = i;
                break;
            }
        }

        return new float[] { Random.Range(gsingle * finalChoice, gsingle * (finalChoice + 1)), finalChoice };
    }
    private void OnDisable()
    {
        alive = false;
    }

    public static int[][] ranges=new int[][]{new int[]{20,20,1,1,1,1 },new int[]{10,10,5,1,1,1 },new int[]{0,5,1,10,10,10 },new int[]{0,2,2,2,10,10 },new int[]{ 5, 2, 2, 2, 10, 10 }
    };

}
