using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Manager Manager;

    private int FindTravelDistance( string Direction ) {
            int Distance = 0;

            int PlayerX, PlayerY;
            PlayerX = PlayerY = 0;

            for ( int i = 0; i < Manager.MapHeight; i++ ) {
                for ( int j = 0; j < Manager.MapWidth; j++ ) {
                    if ( Manager.Map[i, j] == 2 ) {
                        PlayerX = i;
                        PlayerY = j;
                    }
                }
            }

            if ( Direction == "Up" ) {
                while ( Manager.Map[ PlayerX - 1, PlayerY] != 1 ) {
                    Manager.Map[ PlayerX - 1, PlayerY ] = 2;
                    Manager.Map[ PlayerX, PlayerY] = 0;
                    PlayerX = PlayerX - 1;
                    Distance++;
                }
            } else 
            if ( Direction == "Down" ) {
                while ( Manager.Map[ PlayerX + 1, PlayerY] != 1 ) {
                    Manager.Map[ PlayerX + 1, PlayerY ] = 2;
                    Manager.Map[ PlayerX, PlayerY] = 0;
                    PlayerX = PlayerX + 1;
                    Distance++;
                }
            } else 
            if ( Direction == "Left" ) {
                while ( Manager.Map[ PlayerX, PlayerY - 1] != 1 ) {
                    Manager.Map[ PlayerX, PlayerY - 1] = 2;
                    Manager.Map[ PlayerX, PlayerY] = 0;
                    PlayerY = PlayerY - 1;
                    Distance++;
                }
            } else 
            if ( Direction == "Right" ) {
                while ( Manager.Map[ PlayerX, PlayerY + 1] != 1 ) {
                    Manager.Map[ PlayerX, PlayerY + 1] = 2;
                    Manager.Map[ PlayerX, PlayerY] = 0;
                    PlayerY = PlayerY + 1;
                    Distance++;
                }
            }

            if ( Distance != 0 ) {
                Manager.UpdateMoves();
            }

            return Distance;
        }


    private bool _CanMove = true;
    private bool _CanMoveFinger = false;

    private Touch _UserTouch;
    private Vector2 _UserTouchBeginPosition;
    private Vector2 _UserTouchEndPosition;

    void Update() {
        if ( !Manager.OverlayOpen ) {
        if ( _CanMove ) {
            if ( Input.touchCount == 0 ) _CanMoveFinger = false;
                if ( Input.touchCount > 0 ) {
                    _UserTouch = Input.GetTouch( 0 );
                    if ( !_CanMoveFinger ) {
                        _UserTouchBeginPosition = _UserTouch.position;
                        _CanMoveFinger = true;
                    }
                    if ( _UserTouch.phase == TouchPhase.Ended ) {
                        _UserTouchEndPosition = _UserTouch.position;

                        if ( Vector2.Distance( _UserTouchEndPosition, _UserTouchBeginPosition ) >= 10f ) {
                            float _UserSwipeXNormalized = _UserTouchEndPosition.x - _UserTouchBeginPosition.x;
                            float _UserSwipeYNormalized = _UserTouchEndPosition.y - _UserTouchBeginPosition.y;

                            if ( Mathf.Abs( _UserSwipeXNormalized ) > Mathf.Abs( _UserSwipeYNormalized ) ) {
                                if ( _UserSwipeXNormalized < 0f ) {
                                    Debug.Log("User swipe detected: Left");
                                    Manager.TargetPosition.transform.position += new Vector3( -FindTravelDistance("Left"), 0, 0 );
                                    _CanMove = false;
                                } else {
                                     Debug.Log("User swipe detected: Right");
                                    Manager.TargetPosition.transform.position += new Vector3( FindTravelDistance("Right"), 0, 0 );
                                    _CanMove = false;
                                }
                            } else {
                                if ( _UserSwipeYNormalized < 0f ) {
                                    Debug.Log("User swipe detected: Down");
                                    Manager.TargetPosition.transform.position += new Vector3( 0, -FindTravelDistance("Down"), 0 );
                                    _CanMove = false;
                                } else {
                                    Debug.Log("User swipe detected: Up");
                                    Manager.TargetPosition.transform.position += new Vector3( 0, FindTravelDistance("Up"), 0 );
                                    _CanMove = false;
                            }
                        }
                    }
                }
            }
        }
        }

        if ( !_CanMove ) {
                Manager.Player.GetComponent<Rigidbody2D>().position = 
                    Vector3.MoveTowards( 
                        Manager.Player.transform.position, 
                        Manager.TargetPosition.transform.position, 
                        Time.deltaTime * 35f 
                    );
                
                if ( Manager.Player.transform.position == Manager.TargetPosition.transform.position ) {
                    _CanMove = true;
                }
            }
    }

}
