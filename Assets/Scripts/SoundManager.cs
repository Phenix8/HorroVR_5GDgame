using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    public AudioSource snakeSource;
    public AudioSource whisperSource;
    public AudioSource radioSource;

    public float endRadioDelay;
    public float playWhisperDelay;

    public int snakeProbability = 80;
    public int whisperProbability = 50;

    private float startTime, lastWhisperTime = -1.0f;
    private bool hasSnakeSoundToPlay = false, snakeFadeIn = true, whisperFadeIn = true;

    public static bool _snakesoundtoplay;


    void Start ()
    {
        startTime = Time.time;
        whisperSource.volume = snakeSource.volume = 0.0f;
        Random.InitState(50000);
        _snakesoundtoplay = false;
    }
	

	void Update ()
    {
        float endRadioTime = startTime + endRadioDelay;

        if (Time.time > endRadioTime)
            radioSource.Stop();
        

       /* if (Time.time > endRadioTime + playWhisperDelay)
        {
            bool playWhisper = (Random.Range(0, whisperProbability)) == 0;

            if (playWhisper)
            {
                whisperFadeIn = true;
                InvokeRepeating("PlayWhisperSound", 0.0f, 0.8f);
            }
        }*/

        CheckSnakeSoundPlay();
	}


    void CheckSnakeSoundPlay()
    {
        if (snakeSource.volume > 0.1f || _snakesoundtoplay )
            return;

        bool playSnakeSound = (Random.Range(0, snakeProbability)) == 0;

        if (playSnakeSound)
        {
            snakeFadeIn = true;
            InvokeRepeating("PlaySnakeSound", 0.0f, 0.4f);
        }
    }


    void PlayWhisperSound()
    {
        if (whisperSource.volume < 0.0f)
        {
            print("stop whisper");
            whisperSource.volume = 0.0f;
            CancelInvoke();
        }
        else if(whisperSource.volume >= 1.0f)
        {
            whisperFadeIn = false;
        }

        if (whisperFadeIn)
            whisperSource.volume += 0.1f;           
        
        else
            whisperSource.volume -= 0.1f;                  
    }


    void PlaySnakeSound()
    {
        if (snakeSource.volume < 0.0f)
        {
            snakeSource.volume = 0.0f;
            CancelInvoke();
        }
        else if (snakeSource.volume >= 1.0f)
        {
            snakeFadeIn = false;
        }

        if (snakeFadeIn)
            snakeSource.volume += 0.1f; 
        else
            snakeSource.volume -= 0.1f;
    }




}
