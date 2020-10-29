using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //OnTriggerEnter
    //give the player a coin
    //destroy this object
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.AddCoins();
            }

            Destroy(this.gameObject);
        }
    }

}
