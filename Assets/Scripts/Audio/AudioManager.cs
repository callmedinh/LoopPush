using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Utilities;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioMixer audioMixer;
        private readonly Dictionary<string, AudioClip> audioClips = new();

        protected override void Awake()
        {
            base.Awake();
            LoadAudioClips();
        }

        private void LoadAudioClips()
        {
            var clips = Resources.LoadAll<AudioClip>("Audio");
            foreach (var clip in clips)
                if (!audioClips.ContainsKey(clip.name))
                    audioClips.Add(clip.name, clip);
        }

        public void PlaySfx(string name)
        {
            if (audioClips.TryGetValue(name, out var clip))
                sfxSource.PlayOneShot(clip);
            else
                Debug.LogWarning($"Audio clip '{name}' not found.");
        }

        public void PlayBGMMusic(string name, bool loop = false)
        {
            if (audioClips.TryGetValue(name, out var clip))
            {
                musicSource.clip = clip;
                musicSource.loop = loop;
                musicSource.Play();
            }
            else
            {
                Debug.LogWarning($"Audio clip '{name}' not found.");
            }
        }

        public void StopBGMMusic()
        {
            musicSource.Stop();
        }
    }
}