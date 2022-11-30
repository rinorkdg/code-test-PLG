using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Model;
using Platformer.Core;
using Platformer.Mechanics;

[RequireComponent(typeof(AudioSource))]
public class Checkpoint : MonoBehaviour
{
    PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    [SerializeField] private float duration = 2f;


    private bool hasBeenActivated;

    private AudioSource audioSource;
    private GameObject text;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        text = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var p = collider.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            if (!hasBeenActivated)
            {
                model.spawnPoint = transform;
                hasBeenActivated = true;
                audioSource.Play();
                StartCoroutine(TextAnimation());
            }
        }
    }

    private IEnumerator TextAnimation()
    {
        text.SetActive(true);
        
        yield return new WaitForSeconds(duration);

        text.SetActive(false);
    }
}
