using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    #region Instance
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }
    #endregion

    public enum AudioClipType { ENTER_DUNGEON, EXIT_DUNGEON, ENEMY_FOUND, ATTACK, ENEMY_DIE, VICTORY, GAMEOVER, GRAB_ITEM, ADVANCE}

    private AudioSource source;

    public AudioClip EnterDungeon;
    public AudioClip ExitDungeon;
    public AudioClip EnemyFound;
    public AudioClip Attack;
    public AudioClip GameOver;
    public AudioClip EnemyDie;
    public AudioClip Victory;
    public AudioClip GrabItem;
    public AudioClip Advance;


    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

   public void PlayAudioClipNow(AudioClipType type)
   {
        if (source.isPlaying)
            source.Stop();

        AudioClip clip = null;

        switch (type)
        {
            case AudioClipType.ENTER_DUNGEON:
                clip = EnterDungeon;
                break;
            case AudioClipType.EXIT_DUNGEON:
                clip = ExitDungeon;
                break;
            case AudioClipType.ENEMY_FOUND:
                clip = EnemyFound;
                break;
            case AudioClipType.ATTACK:
                clip = Attack;
                break;
            case AudioClipType.ENEMY_DIE:
                clip = EnemyDie;
                break;
            case AudioClipType.VICTORY:
                clip = Victory;
                break;
            case AudioClipType.GAMEOVER:
                clip = GameOver;
                break;
            case AudioClipType.GRAB_ITEM:
                clip = GrabItem;
                break;
            case AudioClipType.ADVANCE:
                clip = Advance;
                break;
        }

        source.clip = clip;
        source.Play();
    }
}
