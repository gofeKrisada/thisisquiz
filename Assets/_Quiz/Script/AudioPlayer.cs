using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : Singleton<AudioPlayer>
{

    [SerializeField]
    public AudioClip sfx_wrong;
    public AudioClip sfx_correct;

    void Start()
    {
        sfx_wrong = Resources.Load("SFX/wrong") as AudioClip;
        sfx_correct = Resources.Load("SFX/correct") as AudioClip;
    }//start

    [SerializeField]
    public int playCount;
    public void Play(AudioClip audioClip)
    {
        this.transform.GetChild(playCount).GetComponent<AudioSource>().PlayOneShot(audioClip);

        playCount++;
        if (playCount > 5) playCount = 0;
    }//

    


}//class
