using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerControl : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioClip MenuBGM;
    [SerializeField] private AudioClip[] GameBGM;

    [Header("UI")]
    [SerializeField] private AudioSource PressedSource;
    [SerializeField] private AudioClip[] PressedClips;
    [Space]
    [SerializeField] private AudioSource ReleasedSource;
    [SerializeField] private AudioClip[] ReleasedClips;
    [Space]
    [SerializeField] private AudioSource DraggedSource;
    [SerializeField] private AudioClip[] DraggedClips;
    [Space]
    [SerializeField] private AudioSource WooshSource;
    [SerializeField] private AudioClip[] WooshClips;
    [Space]
    [SerializeField] private AudioSource JingleUpSource;
    [SerializeField] private AudioClip[] JingleUpClips;
    [Space]
    [SerializeField] private AudioSource JingleDownSource;
    [SerializeField] private AudioClip[] JingleDownClips;

    [Header("Game")] 
    [SerializeField] private AudioSource TokenAttackSource;
    [SerializeField] private AudioClip[] TokenAttackClips;
    [Space] 
    [SerializeField] private AudioSource TokenDiedSource;
    [SerializeField] private AudioClip[] TokenDiedClips;
    [Space] 
    [SerializeField] private AudioSource BuildingDiedSource;
    [SerializeField] private AudioClip[] BuildingDiedClips;
    [Space] 
    [SerializeField] private AudioSource GameEndedSource;
    [SerializeField] private AudioClip[] GameEndedClips;

    public void PlayMenuBGM()
    {
        MusicSource.clip = MenuBGM;
        MusicSource.Play();
    }

    public void PlayGameBGM()
    {
        if (MusicSource.isPlaying)
        {
            MusicSource.Stop();
        }
        MusicSource.clip = GameBGM[Random.Range(0, GameBGM.Length)];
        MusicSource.Play();
    }

    public void PlayUIPressed()
    {
        PressedSource.PlayOneShot(PressedClips[Random.Range(0, PressedClips.Length)]);
    }

    public void PlayUIReleased()
    {
        ReleasedSource.PlayOneShot(ReleasedClips[Random.Range(0, ReleasedClips.Length)]);
    }

    public void PlayUIDragged()
    {
        if (DraggedSource.isPlaying)
            return;
        DraggedSource.PlayOneShot(DraggedClips[Random.Range(0, DraggedClips.Length)]);
    }

    public void PlayUIWoosh()
    {
        WooshSource.PlayOneShot(WooshClips[Random.Range(0, WooshClips.Length)]);
    }

    public void PlayerUIJingleUp()
    {
        JingleUpSource.PlayOneShot(JingleUpClips[Random.Range(0, JingleUpClips.Length)]);
    }

    public void PlayerUIJingleDown()
    {
        JingleDownSource.PlayOneShot(JingleDownClips[Random.Range(0, JingleDownClips.Length)]);
    }

    public void PlayGameTokenAttack()
    {
        TokenAttackSource.PlayOneShot(TokenAttackClips[Random.Range(0, TokenAttackClips.Length)]);
    }

    public void PlayGameTokenDied()
    {
        TokenDiedSource.PlayOneShot(TokenDiedClips[Random.Range(0, TokenDiedClips.Length)]);
    }

    public void PlayGameBuildingDied()
    {
        BuildingDiedSource.PlayOneShot(BuildingDiedClips[Random.Range(0, BuildingDiedClips.Length)]);
    }

    public void PlayGameGameEnded()
    {
        GameEndedSource.PlayOneShot(GameEndedClips[Random.Range(0, GameEndedClips.Length)]);
    }

}
