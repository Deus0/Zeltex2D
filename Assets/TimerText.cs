using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    Text MyText;
    string MyString;

	// Use this for initialization
	void Start ()
    {
        MyText = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        int TimeSeconds = (int) Time.time;
        int TimeMinutes = TimeSeconds / 60;
        int Hours = TimeMinutes / 60;
        if (Hours != 0)
        {
            TimeMinutes %= Hours;
        }
        if (Hours * 60 + TimeMinutes != 0)
        {
            TimeSeconds %= (Hours * 60 + TimeMinutes);
        }
        string NewString = "";
        if (Hours != 0)
        {
            NewString = Hours + "h " + TimeMinutes + "m " + TimeSeconds + "s";
        }
        else if (TimeMinutes != 0)
        {
            NewString = TimeMinutes + "m " + TimeSeconds + "s";
        }
        else
        {
            NewString = TimeSeconds + "s";
        }
        if (MyString != NewString)
        {
            MyString = NewString;
            MyText.text = NewString;
        }
    }
}
