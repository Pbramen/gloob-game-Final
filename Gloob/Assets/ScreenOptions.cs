using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenOptions : MonoBehaviour
{
    public Dropdown resolution;
    public Toggle fullScreen;
    public Resolution[] resolutions;
    void Start() { 
                //list of all of resolution for current monitor
        resolutions = Screen.resolutions;
        fullScreen.isOn = Screen.fullScreen;
        for (int i = 0; i < resolutions.Length; i++) {
            string rsString = resolutions[i].width.ToString() + "x" + resolutions[i].height.ToString();
            resolution.options.Add(new Dropdown.OptionData(rsString));

            //set the default value in the drop down
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                resolution.value = i;
            }
        }
    }
    public void SetResolution() {
        Screen.SetResolution(resolutions[resolution.value].width, resolutions[resolution.value].height, fullScreen.isOn);
    }
}
