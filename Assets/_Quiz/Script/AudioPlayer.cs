using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : Singleton<AudioPlayer>
{

    [HideInInspector]
    public AudioClip sfx_wrong;
    [HideInInspector]
    public AudioClip sfx_correct;
    [HideInInspector]
    public AudioClip sfx_button;
    [HideInInspector]
    public AudioClip sfx_result;
    [HideInInspector]
    public AudioClip bgm_theme;
    [HideInInspector]
    public AudioClip bgm_gameplay;

    void Awake()
    {
        if (GameObject.Find("(singleton) [AudioPlayer]") != null)
            Destroy(this.gameObject);

        sfx_wrong = Resources.Load("SFX/wrong") as AudioClip;
        sfx_correct = Resources.Load("SFX/correct") as AudioClip;
        sfx_button = Resources.Load("SFX/button") as AudioClip;
        sfx_result = Resources.Load("SFX/result") as AudioClip;

       bgm_theme = Resources.Load("bgm_theme") as AudioClip;
       bgm_gameplay = Resources.Load("bgm_gameplay") as AudioClip;

    }//start

    [HideInInspector]
    public int playCount;
    public void Play(AudioClip audioClip)
    {
        this.transform.GetChild(playCount).GetComponent<AudioSource>().PlayOneShot(audioClip);

        playCount++;
        if (playCount > 5) playCount = 0;
    }//

    public void PlayBGM(AudioClip audioClip)
    {
        this.GetComponent<AudioSource>().clip= audioClip;
        this.GetComponent<AudioSource>().Play();
    }//

    public void PlayBGM(AudioClip audioClip,float delay)
    {
        this.GetComponent<AudioSource>().clip = audioClip;
        this.GetComponent<AudioSource>().PlayDelayed(delay);
    }//

    public void StopBGM()
    {

        this.GetComponent<AudioSource>().Stop();
    }//




}//class
