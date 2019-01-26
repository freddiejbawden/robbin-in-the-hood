using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundscape : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip rumble;
    int clipIndex = 0;
    AudioSource source;

    private void Start() {
        source = transform.GetComponent<AudioSource>();
        source.PlayOneShot(rumble);
        StartCoroutine(playPing());
    }

    public IEnumerator playPing() {
        while(true) {
        yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));
             clipIndex = Random.Range(0, clips.Length);
             source.PlayOneShot(clips[clipIndex], 1.0f);
        }
    }
}
