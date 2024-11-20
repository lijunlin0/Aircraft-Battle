using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager sCurrent;
    private AudioSource mMusicAudioSource;
    private AudioSource mSoundAudioSource;
    public static AudioManager GetCurrent()
    {
        return sCurrent;
    }
    public static void AudioManagerInit(AudioManager audioManager)
    {
        sCurrent=audioManager;
    }
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        AudioSource[] audioSources=GetComponents<AudioSource>();
        mMusicAudioSource=audioSources[0];
        mSoundAudioSource=audioSources[1];
        mMusicAudioSource.loop=true;
        mSoundAudioSource.loop=false;
    }
    public void PlayFightMusic()
    {
        mMusicAudioSource.clip=Resources.Load<AudioClip>("Sound/FightMusic");
        mMusicAudioSource.Play();
    }
    public void PlayCharacterHurtSound()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/Hurt");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void PlayCharacterExplosionSound()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/Explosion");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void PlayButtonOnClick()
    {
        AudioClip clip=Resources.Load<AudioClip>("Sound/Button");
        mSoundAudioSource.PlayOneShot(clip);
    }
    public void StopPlay()
    {
        mSoundAudioSource.Stop();
    }
    public void IsPauseFightMusicPlay(bool pause)
    {
        if(pause)
        {
            mMusicAudioSource.Pause();
        }
        else
        {
            mMusicAudioSource.UnPause();
        }
        
    }    
}