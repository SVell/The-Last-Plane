using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPause : MonoBehaviour
{

    public void MuteAllSounds()
    {
        AudioListener.pause = true;
    }

    public void UnMute()
    {
        AudioListener.pause = false;
    }
    
}
