using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
     private Data() { }

        static private Data _instance;
        static public Data instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Data();
                return _instance;
            }
        }

        public float accuracy;
        public float score;

    public string MapFileName;
    public AudioClip music;
    public int bpm;
    public float sfxValue;
    public float musicValue;
}
