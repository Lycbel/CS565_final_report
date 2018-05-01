using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameConfigControll : MonoBehaviour {

    public InputField DurationInput;
    public Slider TargetSizeSlider;
    public Slider PaceSlider;
    public Slider TargetMoveScaleSlider;

    public Dropdown ModeDropdown;

    private float m_objectSpeed;
    private float m_respawnInterval;
    private float m_maxMoveInterval;

    void Start()
    {
        m_objectSpeed = staticConfig.objectSpeed;
        m_respawnInterval = staticConfig.respawnInterval;
        m_maxMoveInterval = staticConfig.maxMoveInterval;
        staticConfig.shootDis();
        staticConfig.aggRange();
    }

    public void UpdateDuration()
    {
        float num = float.Parse(DurationInput.text);
        staticConfig.totalTime = num;
        Debug.Log("staticConfig.totalTime  " + staticConfig.totalTime.ToString());
    }

    public void UpdateTargetSize()
    {
        float v = TargetSizeSlider.value;
        staticConfig.objectScale = v;
        Debug.Log("staticConfig.objectScale  " + staticConfig.objectScale.ToString());
    }

    public void UpdatePace()
    {
        float v = PaceSlider.value;
        staticConfig.objectSpeed = m_objectSpeed * v;
        staticConfig.respawnInterval = m_respawnInterval / v;
        Debug.Log("staticConfig.objectSpeed  " + staticConfig.objectSpeed.ToString());
        Debug.Log("staticConfig.respawnInterval  " + staticConfig.respawnInterval.ToString());
    }

    public void UpdateTargetMoveScale()
    {
        float v = TargetMoveScaleSlider.value;
        staticConfig.maxMoveInterval = m_maxMoveInterval * v;
        Debug.Log("staticConfig.maxMoveInterval  " + staticConfig.maxMoveInterval.ToString());
    }

    public void UpdateMode()
    {
        int mode = ModeDropdown.value;
        Debug.Log(ModeDropdown.value);
        if (mode == 0)
        {
            staticConfig.shootDis();
           
        }
        else if (mode == 1)
        {
            staticConfig.traceM();
        }
        else
        {
            
            staticConfig.normalShoot(13);
        }
        Debug.Log(staticConfig.objectMove);
        Debug.Log(staticConfig.isTrace);
        Debug.Log(staticConfig.shootDisapear);
    }
}
