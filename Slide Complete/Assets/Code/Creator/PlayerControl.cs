using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public CreateControls Controls;
    public GlobalVariables Variables;

    void OnTriggerEnter2D( Collider2D collision ) {
        if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
            if ( collision.gameObject.GetComponent<SpriteRenderer>().color == new Color32( 23, 23, 23, 255 ) && Controls.GetTestingState() ) {
                collision.gameObject.GetComponent<SpriteRenderer>().color = Variables.GetLastColor();
            }
        } else {
            if ( collision.gameObject.GetComponent<SpriteRenderer>().color == Color.white && Controls.GetTestingState() ) {
                collision.gameObject.GetComponent<SpriteRenderer>().color = Variables.GetLastColor();
            }
        }
    }
}
