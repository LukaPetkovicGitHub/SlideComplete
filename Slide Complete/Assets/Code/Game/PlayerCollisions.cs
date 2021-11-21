using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public Manager Manager;

    void OnTriggerEnter2D( Collider2D collision ) {
        if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
            if ( collision.gameObject.GetComponent<SpriteRenderer>().color == new Color32( 23, 23, 23, 255 ) ) {
                collision.gameObject.GetComponent<SpriteRenderer>().color = Manager.Variables.GetLastColor();
                Manager.TilesLeft--;
            }
        } else {
            if ( collision.gameObject.GetComponent<SpriteRenderer>().color == Color.white ) {
                collision.gameObject.GetComponent<SpriteRenderer>().color = Manager.Variables.GetLastColor();
                Manager.TilesLeft--;
            }
        }
    }
}
