using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public Dropdown DropdownR;
    Resolution[] resolutions;

	private void Start()
	{
        resolutions = Screen.resolutions;
        DropdownR.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
		{
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
                currentResolutionIndex = i;
			}
		}

        DropdownR.AddOptions(options);
        DropdownR.value = currentResolutionIndex;
        DropdownR.RefreshShownValue();
    }

	public void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat("MusicVolume", volume);
    }
    public void SetSfxVolume(float volume)
    {
        AudioMixer.SetFloat("SfxVolume", volume);
    }

    public void SetMobsVolume(float volume)
    {
        AudioMixer.SetFloat("MobsVolume", volume);
    }

    public void Fullscreen(bool _fullscreenSet)
	{
        Screen.fullScreen = _fullscreenSet;
	}
}
