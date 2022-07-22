using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Cutscene : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;

    public void Init(AudioSource audioSource)
    {
        _audioSource = audioSource;
        _animator = GetComponent<Animator>();
    }

    public void Play()
    {
        _animator.SetTrigger("Play");
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
