using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zeltex2D
{
    public class MusicManager : MonoBehaviour
    {
        AudioSource MySource;
        public AudioClip MainTheme;
        public AudioClip CombatTheme;

        private void Awake()
        {
            MySource = GetComponent<AudioSource>();
            PlayMainTheme();
        }

        public void PlayMainTheme()
        {
            MySource.Stop();
            MySource.clip = MainTheme;
            MySource.Play();
        }
        public void PlayCombatTheme()
        {
            MySource.Stop();
            MySource.clip = CombatTheme;
            MySource.Play();
        }
    }

}