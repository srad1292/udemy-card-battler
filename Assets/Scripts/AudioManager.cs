using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource menuMusic;
    public AudioSource battleSelectMusic;
    public AudioSource[] soundtrack;
    
    public static AudioManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public void StopMusic() {
        menuMusic.Stop();
        battleSelectMusic.Stop();
        foreach(AudioSource track in soundtrack) {
            track.Stop();
        }
    }

    public void PlayMenuMusic() {
        StopMusic();
        menuMusic.Play();
    }

    public void PlayBattleSelectMusic() {
        if(battleSelectMusic.isPlaying == false) {
            StopMusic();
            battleSelectMusic.Play();
        }
        
    }

    public void PlaySoundtrackMusic() {
        StopMusic();
        //soundtrack[Random.Range(0, soundtrack.Length)].Play();
    }
}
