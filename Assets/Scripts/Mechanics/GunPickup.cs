using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class GunPickup : MonoBehaviour
{
    [SerializeField] private GameObject gunPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        //only exectue OnPlayerEnter if the player collides with this item
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null) OnPlayerEnter(player);
    }

    void OnPlayerEnter(PlayerController player)
    {
        //if th eplayer already has a gun, replenish their ammo
        Gun oldGun = player.GetComponentInChildren<Gun>();
        if (oldGun != null)
        {
            oldGun.ammo = oldGun.maxAmmo;
            oldGun.audioSource.Play();
        }
        //if they don't have a gun anymore, give them a new one
        else
        {
            GameObject newGun = Instantiate(gunPrefab);
            newGun.transform.parent = player.transform;
            newGun.transform.localPosition = Vector3.zero;
        }
        Destroy(gameObject);
    }
}
