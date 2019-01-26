using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundscape : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioClip rumble;
    [SerializeField] private AudioClip avenge_me_normal;
    [SerializeField] private AudioClip avenge_me_loud;
    [SerializeField] private AudioClip avenge_me_backwards;
    int clipIndex = 0;
    AudioSource source;

    private void Start() {
        source = transform.GetComponent<AudioSource>();
        source.PlayOneShot(rumble);
        StartCoroutine(playPing());
        StartCoroutine(playAM_normal());
        StartCoroutine(playAM_loud());
        StartCoroutine(playAM_backwards());
    }

    public IEnumerator playPing() {
        while(true) {
        yield return new WaitForSeconds(Random.Range(0.0f, 5.0f));
             clipIndex = Random.Range(0, clips.Length);
             source.PlayOneShot(clips[clipIndex], 1.0f);
        }
    }

    public IEnumerator playAM_normal() {
        while(true) {
        yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));
             source.PlayOneShot(avenge_me_normal);
        }
    }

    public IEnumerator playAM_loud() {
        while(true) {
        yield return new WaitForSeconds(Random.Range(10.0f, 15.0f));
             source.PlayOneShot(avenge_me_loud);
        }
    }

    public IEnumerator playAM_backwards() {
        while(true) {
        yield return new WaitForSeconds(Random.Range(10.0f, 15.0f));
             source.PlayOneShot(avenge_me_backwards);
        }
    }
}
