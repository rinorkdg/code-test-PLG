using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    public class PlayerSniffer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                //when a player enter the detection trigger, pass it on to the attached enemy
                GetComponentInParent<EnemyController>().OnPlayerFound(player);
            }
        }
    }
}
