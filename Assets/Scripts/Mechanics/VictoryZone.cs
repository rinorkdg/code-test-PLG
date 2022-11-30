using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;
using Platformer.Mechanics;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    public class VictoryZone : MonoBehaviour
    {
        //score needed to be able to win
        [SerializeField] private int scoreToWin = 50;

        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                //win condition
                if (GameController.Instance.Score >= scoreToWin)
                {
                    var ev = Schedule<PlayerEnteredVictoryZone>();
                    ev.victoryZone = this;
                }
            }
        }
    }
}