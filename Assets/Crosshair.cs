using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Platformer.Mechanics;

public class Crosshair : MonoBehaviour
{
    private TMP_Text ammoCount;
    private PlayerController player;

    void Start()
    {
        ammoCount = GetComponentInChildren<TMP_Text>();
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        //move the crosshair to the mouse position
        transform.position = Input.mousePosition;

        //display the ammo of the current gun above the crosshair
        Gun gun = player.GetComponentInChildren<Gun>();
        if (gun != null)
        {
            ammoCount.SetText(gun.ammo + "/" + gun.maxAmmo);
        }
    }
}
