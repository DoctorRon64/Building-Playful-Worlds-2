using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource src;
    public List<AudioClip> sfxPlayer = new List<AudioClip>();
    public List<AudioClip> sfxZombie = new List<AudioClip>();
    public List<AudioClip> sfxGoblin = new List<AudioClip>();
    public List<AudioClip> sfxBoss = new List<AudioClip>();
    public List<AudioClip> sfxWalking = new List<AudioClip>();
    public AudioClip sfxDrinking;
    public AudioClip sfxStaffSound;
    public AudioClip sfxSwordSound;

    private void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlayAudio(int _indexMob)
    {
        switch (_indexMob)
        {
            case 0: PlayRandomAudio(sfxPlayer); break;
            case 1: PlayRandomAudio(sfxZombie); break;
            case 2: PlayRandomAudio(sfxGoblin); break;
            case 3: PlayRandomAudio(sfxBoss); break;
            case 4: PlayRandomAudio(sfxWalking); break;
            case 5: PlayAudioClip(sfxDrinking); break;
            case 6: PlayAudioClip(sfxStaffSound); break;
            case 7: PlayAudioClip(sfxSwordSound); break;
            default:
                Debug.Log("Geen goede indexvalue: " + _indexMob);
            break;
        }
    }

    private void PlayRandomAudio(List<AudioClip> audioClips)
    {
        if (audioClips.Count == 0)
        {
            Debug.Log("geen audio clips beschikbaar");
            return;
        }

        int randomIndex = Random.Range(0, audioClips.Count);
        PlayAudioClip(audioClips[randomIndex]);
    }

    private void PlayAudioClip(AudioClip audioClip)
    {
        src.clip = audioClip;
        src.Play();
    }
}
