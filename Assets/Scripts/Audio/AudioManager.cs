using UnityEngine;
using Utilities;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        public void PlaySfx()
        {
            sfxSource.Play();
        }
        public void PlayMusic(AudioClip clip)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }
    }
}