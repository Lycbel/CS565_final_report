using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityControll : MonoBehaviour {

    public Slider slider;
    public InputField input;

    public static float convertSensitivityStoO(float s)
    {
        return 4 * s + 3;
    }

    public static float convertSensitivityOtoS(float o)
    {
        return (o-3)/4f;
    }

    void Start () {
        input.text = convertSensitivityStoO(slider.value).ToString();
    }
	
    //called on sensitivity slide updated
	public void UpdateValue () {
        float v = slider.value;
        staticConfig.sensitive = v;
        staticConfig.UpdateValue();
        input.text = convertSensitivityStoO(v).ToString();
    }

    //called on sensitivity input updated
    public void UpdateSliderValue()
    {
        float num = float.Parse(input.text);
        num = convertSensitivityOtoS(num);
        staticConfig.sensitive = num;
        staticConfig.UpdateValue();
        slider.value = num;
    }

}
