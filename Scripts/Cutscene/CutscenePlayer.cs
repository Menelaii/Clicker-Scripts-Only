using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private Cutscene _current;

    public void Play(Cutscene cutscene)
    {
        if (_current != null)
            Destroy(_current.gameObject);

        _current = Instantiate(cutscene, transform);
        _current.Init(_audioSource);
        _current.Play();
    }
}
