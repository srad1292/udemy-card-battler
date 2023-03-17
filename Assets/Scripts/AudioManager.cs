using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SfxTrack { ButtonPress, CardAttack, CardDefeat, CardDraw, CardPlace, HurtEnemy, HurtPlayer };

    public AudioSource menuMusic;
    public AudioSource battleSelectMusic;
    public AudioSource[] soundtrack;
    public AudioSource[] sfx;

    private int currentTrack;
    private bool playingSoundTrack;

    
    public static AudioManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update() {
        if(playingSoundTrack) {
            if(soundtrack[currentTrack].isPlaying == false) {
                currentTrack++;
                if(currentTrack >= soundtrack.Length) {
                    currentTrack = 0;
                }
                soundtrack[currentTrack].Play();
            }
        }
    }

    public void StopMusic() {
        menuMusic.Stop();
        battleSelectMusic.Stop();
        foreach(AudioSource track in soundtrack) {
            track.Stop();
        }
        playingSoundTrack = false;
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
        currentTrack = Random.Range(0, soundtrack.Length);
        soundtrack[currentTrack].Play();
        playingSoundTrack = true;
        
        currentTrack++;
    }

    public void PlaySFX(SfxTrack sfxToPlay) {
        int sfxNum = (int)sfxToPlay;
        sfx[sfxNum].Stop();
        sfx[sfxNum].Play();
    }
}
