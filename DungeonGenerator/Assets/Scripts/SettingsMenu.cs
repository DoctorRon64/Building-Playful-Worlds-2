using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public AudioMixerGroup[] AudioMixerGroup = new AudioMixerGroup[3];
    public Slider[] VolumeSlider = new Slider[3];
    public Dropdown DropdownR;
    [SerializeField] private GameObject OptionsMenu;
    Resolution[] resolutions;

	private void Awake()
	{
        float[] volumes = new float[3];

        AudioMixer.GetFloat("MusicVolume", out volumes[0]);
        AudioMixer.GetFloat("SfxVolume", out volumes[1]);
        AudioMixer.GetFloat("MobsVolume", out volumes[2]);

        for (int i = 0; i < AudioMixerGroup.Length; i++)
        {
            VolumeSlider[i].value = volumes[i];
        }
    }

	private void Start()
	{
        OptionsMenu.SetActive(false);

        resolutions = Screen.resolutions;
        DropdownR.ClearOptions();

        List<string> _options = new List<string>();
        int _currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
		{
            string _option = resolutions[i].width + "x" + resolutions[i].height;
            _options.Add(_option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
			{
                _currentResolutionIndex = i;
			}
		}

        DropdownR.AddOptions(_options);
        DropdownR.value = _currentResolutionIndex;
        DropdownR.RefreshShownValue();
    }

    public void SetResolutions(int _resolutionIndex)
	{
        Resolution _resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen);
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

    public void OptionsMenuHide(bool _OptionsBool)
	{
        OptionsMenu.SetActive(_OptionsBool);
	}
}
