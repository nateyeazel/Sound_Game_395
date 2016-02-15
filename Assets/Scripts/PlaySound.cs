using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {
    public AudioClip clip;
    public int delay;
    bool trigger;
    // Use this for initialization
    private AudioSource source;
    void Awake () {
        source = gameObject.GetComponent<AudioSource>();
        source.clip = clip;
        StartCoroutine(DelaySound());

    }

    // Update is called once per frame
    void Update () {
        if (trigger) {
            source.Play();
            StartCoroutine(DelaySound());

        }
    }

    public IEnumerator DelaySound()
    {
        trigger = false; // will make the update method pick up 
        yield return new WaitForSeconds(delay); // waits 3 seconds
        trigger = true; // will make the update method pick up 
    }

}
