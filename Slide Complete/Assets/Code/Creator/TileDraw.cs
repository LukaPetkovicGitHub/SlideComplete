using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDraw : MonoBehaviour
{
    public CreateControls Controls;

    void OnMouseOver() {
        if ( !Controls.GetTestingState() ) {
            if ( Controls.GetDrawType() == "Wall" ) {

                int _ObjectX, _ObjectY;
                _ObjectX = _ObjectY = 0;

                for ( int i = 0; i < Controls.GetMapHeight(); i++ ) {
                    for ( int j = 0; j < Controls.GetMapWidth(); j++ ) {
                        if ( gameObject == Controls.GetObjectMap()[i, j] ) {
                            _ObjectX = i;
                            _ObjectY = j;
                        }
                    }
                }

                if ( _ObjectX != 0 && _ObjectX != Controls.GetMapHeight() - 1 && _ObjectY != 0 && _ObjectY != Controls.GetMapWidth() - 1 ) {
                    Controls.GetMap()[_ObjectX, _ObjectY] = 1; 
                    if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
                        GetComponent<SpriteRenderer>().color = Color.white;
                    } else {
                        GetComponent<SpriteRenderer>().color = new Color32( 23, 23, 23, 255 );
                    }
                }
            } else 
            if ( Controls.GetDrawType() == "Tile" ) {

                int _ObjectX, _ObjectY;
                _ObjectX = _ObjectY = 0;

                for ( int i = 0; i < Controls.GetMapHeight(); i++ ) {
                    for ( int j = 0; j < Controls.GetMapWidth(); j++ ) {
                        if ( gameObject == Controls.GetObjectMap()[i, j] ) {
                            _ObjectX = i;
                            _ObjectY = j;
                        }
                    }
                }

                if ( _ObjectX != 0 && _ObjectX != Controls.GetMapHeight() - 1 && _ObjectY != 0 && _ObjectY != Controls.GetMapWidth() - 1 ) { 
                    Controls.GetMap()[_ObjectX, _ObjectY] = 0; 
                    if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) { 
                        GetComponent<SpriteRenderer>().color = new Color32( 23, 23, 23, 255 );
                    } else {
                        GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            } else 
            if ( Controls.GetDrawType() == "Player" ) {
                if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
                    if ( GetComponent<SpriteRenderer>().color != Color.white ) {
                        Controls.Player.transform.position = gameObject.transform.position + new Vector3( 0, 0, -3 );
                        Controls.TargetPosition.transform.position = gameObject.transform.position + new Vector3( 0, 0, -3 );
                        Controls.SetPlayerStart( gameObject.transform.position + new Vector3( 0, 0, -3 ) );

                        if ( Controls.GetPlayerStartX() != 0 && Controls.GetPlayerStartY() != 0 ) {
                            Controls.GetMap()[ Controls.GetPlayerStartX(), Controls.GetPlayerStartY() ] = 0;
                        } else {
                            Controls.GetMap()[ Controls.GetPlayerStartX(), Controls.GetPlayerStartY() ] = 1;
                        }
                        Controls.SetPlayerStart( 
                            (int)( gameObject.transform.position.y * -1 + Mathf.FloorToInt( Controls.GetMapHeight() / 2 )), 
                            (int)(gameObject.transform.position.x + Mathf.FloorToInt( Controls.GetMapWidth() / 2 )) 
                        );

                        Controls.GetMap()[ 
                            (int)( gameObject.transform.position.y * -1 + Mathf.FloorToInt( Controls.GetMapHeight() / 2 )), 
                            (int)(gameObject.transform.position.x + Mathf.FloorToInt( Controls.GetMapWidth() / 2 )) 
                        ] = 2;

                    }
                }

                if ( PlayerPrefs.GetInt("DarkMode", 0) == 1 ) {
                    if ( GetComponent<SpriteRenderer>().color != new Color32( 23, 23, 23, 255 ) ) {
                        Controls.Player.transform.position = gameObject.transform.position + new Vector3( 0, 0, -3 );
                        Controls.TargetPosition.transform.position = gameObject.transform.position + new Vector3( 0, 0, -3 );
                        Controls.SetPlayerStart( gameObject.transform.position + new Vector3( 0, 0, -3 ) );

                        if ( Controls.GetPlayerStartX() != 0 && Controls.GetPlayerStartY() != 0 ) {
                            Controls.GetMap()[ Controls.GetPlayerStartX(), Controls.GetPlayerStartY() ] = 0;
                        } else {
                            Controls.GetMap()[ Controls.GetPlayerStartX(), Controls.GetPlayerStartY() ] = 1;
                        }
                        Controls.SetPlayerStart( 
                            (int)( gameObject.transform.position.y * -1 + Mathf.FloorToInt( Controls.GetMapHeight() / 2 )), 
                            (int)(gameObject.transform.position.x + Mathf.FloorToInt( Controls.GetMapWidth() / 2 )) 
                        );

                        Controls.GetMap()[ 
                            (int)( gameObject.transform.position.y * -1 + Mathf.FloorToInt( Controls.GetMapHeight() / 2 )), 
                            (int)(gameObject.transform.position.x + Mathf.FloorToInt( Controls.GetMapWidth() / 2 )) 
                        ] = 2;

                    }
                }
            }
        }
    }
}
