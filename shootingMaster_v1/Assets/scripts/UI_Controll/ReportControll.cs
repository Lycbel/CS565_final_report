using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ReportControll : MonoBehaviour {
    public Text content;
    public Dropdown selection;


    public void Update()
    {
        int i = selection.value;
        string text = "";
        if (i == 0)
        {
            text = getPerformanceReport();
        }
        else if (i == 1)
        {
            text = getAccuracyReport();
        }
        else if (i == 2)
        {
            text = getReactionTimeReport();
        }
        else if(i == 3)
        {
            text = getShootingOffsetReport();
        }
        else
        {
            text = getSuggestions();
        }
        content.text = text;
    }

    public void IncSelection()
    {
        if (selection.value < 4)
        {
            selection.value = selection.value + 1;
        }
    }

    public void DecSelection()
    {
        if (selection.value > 0)
        {
            selection.value = selection.value - 1;
        }
    }

    private string getPerformanceReport()
    {
        return "Your overall performance is 1611, beated 43% of players." + Environment.NewLine + Environment.NewLine +

                   "Your current highest is 1770, beated 56 % of players." + Environment.NewLine + Environment.NewLine +

                    "Current world highest is 1911.The baseline for pro players is 1864." + Environment.NewLine + Environment.NewLine +

                    "YOUR RANK: C";
    }

    private string getAccuracyReport()
    {
        return "Your overall accuracy is 31%, beated 41% of players." + Environment.NewLine + Environment.NewLine +

                   "Your current highest accuracy is 44%, beated 50 % of players." + Environment.NewLine + Environment.NewLine +

                    "Current world highest accuracy is 66%.The baseline for pro players is 45%." + Environment.NewLine + Environment.NewLine +

                    "YOUR RANK: C";
    }

    private string getReactionTimeReport()
    {
        return "Your overall reaction time is 0.31s, beated 56% of players." + Environment.NewLine + Environment.NewLine +

                   "Your current highest is 0.34s, beated 69 % of players." + Environment.NewLine + Environment.NewLine +

                    "Current world highest is 0.23s.The baseline for pro players is 0.30." + Environment.NewLine + Environment.NewLine +

                    "YOUR RANK: B";
    }

    private string getShootingOffsetReport()
    {
        return "Your overall shooting offset is 22%, beated 38% of players." + Environment.NewLine + Environment.NewLine +

                  "Your current highest is 15%, beated 43 % of players." + Environment.NewLine + Environment.NewLine +

                    "Current world highest is 6%.The baseline for pro players is 9%." + Environment.NewLine + Environment.NewLine +

                    "YOUR RANK: C";
    }

    private string getSuggestions()
    {
        return "Your shortcoming is shooting offset. You should lower your mouse sensitivity, or keep using this software." + Environment.NewLine + 
                "Your prepfered shooting distance is 50. Your performance drops significantly when distance greater than 60." + Environment.NewLine + 
                "You can keep your focus for about 1min 20s. You should gradually increase the duration of futrue prectices. GO TO SETTINGS" + Environment.NewLine +
                "Your player type is: type 3. Click here to see configurations for pro players under the same type.";
    }

}
