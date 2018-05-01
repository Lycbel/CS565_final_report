using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadData : MonoBehaviour {

    public Slider SensitivitySlider;

    public InputField DurationInput;
    public Slider TargetSizeSlider;

    public void LoadSenSitivity()
    {
        staticConfig.LoadValue();
        float sensitivity_ = staticConfig.sensitive;
        SensitivitySlider.value = sensitivity_;
    }

    public void LoadGameDefault()
    {
        //get values
        float objectScale_ = staticConfig.objectScale;
        float totalTime_ = staticConfig.totalTime;

        //set
        DurationInput.text = totalTime_.ToString();
        TargetSizeSlider.value = objectScale_;


    }
}
