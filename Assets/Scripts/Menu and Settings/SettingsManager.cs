using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region Variables
    [Header("Resolution")]
    public Resolution[] resolutions;//Array of resolution options for the player
    public Dropdown resolutionDropdown;//The UI drop down object for resoltion settings
    [Header("Key Binding")]
    public Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();//Dictionary for keybindings
    public Text up, left, down, right, attack;//Text for keybinding buttons
    private GameObject currentKey;//Button that has been pressed to be changed key binding
    #endregion
    #region Functions
    public bool IntToBool(int integer)
    {
        if(integer == 0)
        {
            return false;
        }
        else if(integer == 1)
        {
            return true;
        }
        else
        {
            Debug.LogError("integer out of range");
            return false;
        }
    }
    public int BoolToInt(bool boolean)
    {
        if(boolean == true)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public void SetFullScreen(bool isFullScreen)//Sets fullscreen/windowed depending on toggle
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)//Sets resolution depending on selection from dropdown
    {
        Resolution res = resolutions[resolutionIndex];//res is set to equal the resolution selected from the dropdown
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);//the current resolution is set to equal res
    }

    public void Quality(int qualityIndex)//Sets quality depending on selection from dropdown
    {
        QualitySettings.SetQualityLevel(qualityIndex);//quality is set to option selected on the dropdown
    }
    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }
    public void SaveChanges()
    {
        PlayerPrefs.SetInt("fullScreen", BoolToInt(Screen.fullScreen));
        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
    }
    private void Start()
    {
        resolutions = Screen.resolutions;//sets resolution options for current screen
        resolutionDropdown.ClearOptions();//clears previous drop down options
        List<string> options = new List<string>();//creates new string of resolutions
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)//loops through the resolutions to set the dropdown options
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;//option is equal to current resolution
            options.Add(option);//current res, as option, is added to options list
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)//checks if current res is equal to screen dimensions
            {
                currentResolutionIndex = i;//if true, sets current res as resolution
            }
        }
        resolutionDropdown.AddOptions(options);//adds resolutions to dropdown options
        resolutionDropdown.value = currentResolutionIndex;//sets dropdowns current value to equal the current resolution
        resolutionDropdown.RefreshShownValue();
        //Below sets text on key binding buttons to equal current key bindings
        /*
        keyBindings.Add("Up", KeyCode.W);
        keyBindings.Add("Down", KeyCode.S);
        keyBindings.Add("Left", KeyCode.A);
        keyBindings.Add("Right", KeyCode.D);
        keyBindings.Add("Attack", KeyCode.Space);
        up.text = keyBindings["Up"].ToString();
        down.text = keyBindings["Down"].ToString();
        left.text = keyBindings["Left"].ToString();
        right.text = keyBindings["Right"].ToString();
        attack.text = keyBindings["Attack"].ToString();
        */
    }
    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keyBindings[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey = null;
            }
        }
    }
    #endregion
}
