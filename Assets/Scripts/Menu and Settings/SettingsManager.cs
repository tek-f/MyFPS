using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyFPS.GameAdmin
{
    public class SettingsManager : MonoBehaviour
    {
        #region Variables
        [Header("Resolution")]
        /// <summary>
        /// Array of resolution options for the player
        /// </summary>
        public Resolution[] resolutions;
        /// <summary>
        /// The UI drop down object for resoltion settings
        /// </summary>
        public Dropdown resolutionDropdown;
        #endregion
        #region Functions
        /// <summary>
        /// Converts an int to a bool where a 1 = true and 0 = false.
        /// </summary>
        /// <param name="integer">The value to be converted.</param>
        /// <returns>Bool</returns>
        public bool IntToBool(int integer)
        {
            if (integer == 0)
            {
                return false;
            }
            else if (integer == 1)
            {
                return true;
            }
            else
            {
                Debug.LogError("integer out of range");
                return false;
            }
        }
        /// <summary>
        /// Converts a bool to an int where true = 1 and false = 0.
        /// </summary>
        /// <param name="boolean">Value to be converted</param>
        /// <returns>Integer</returns>
        public int BoolToInt(bool boolean)
        {
            if (boolean == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Sets fullscreen/windowed depending on toggle.
        /// </summary>
        /// <param name="isFullScreen">Toggel value.</param>
        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
        /// <summary>
        /// Sets resolution depending on selection from dropdown.
        /// </summary>
        /// <param name="resolutionIndex">Dropdown value.</param>
        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutions[resolutionIndex];//res is set to equal the resolution selected from the dropdown
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);//the current resolution is set to equal res
        }
        /// <summary>
        /// Sets quality depending on selection from dropdown.
        /// </summary>
        /// <param name="qualityIndex">Dropdown selection value.</param>
        public void Quality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);//quality is set to option selected on the dropdown
        }
        /// <summary>
        /// Saves the changes made to the settings using PlayerPrefs.
        /// </summary>
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
        }
        #endregion
    }
}