using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateControls : MonoBehaviour
{   
    [Header("Map Size Objects")]
    public Dropdown MapSizeDropdown;

    [Header("Instantiates")]
    public GameObject Tile;
    public GameObject Player;
    public GameObject TargetPosition;

    [Space(20)]
    public Image Paintbrush;

    public GlobalVariables Variables;
    public GameObject PlayerObject;
    void Start() {
        PlayerObject.GetComponent<SpriteRenderer>().color = Variables.GetLastPlayerColor();
    }

    public void SetupMap() {
        if ( MapSizeDropdown.value == 0 ) {
            MapSizeString = "SMALL";
            MapHeight = 21;
            MapWidth = 15;
            Camera.main.orthographicSize = 17;
        } else 
         if ( MapSizeDropdown.value == 1 ) {
            MapSizeString = "MEDIUM";
            MapHeight = 25;
            MapWidth = 19;
            Camera.main.orthographicSize = 21;
        } else
         if ( MapSizeDropdown.value == 2 ) {
            MapSizeString = "LARGE";
            MapHeight = 31;
            MapWidth = 25;
            Camera.main.orthographicSize = 27;
        } 

        Debug.Log("Map Variables Collected");

        IEnumerator InitiateIEnumerator() {
            yield return new WaitForSeconds(0.5f);
            InitiateMap();
        }
        StartCoroutine( InitiateIEnumerator() );
    }

    private void InitiateMap() {
        Debug.Log("Initiated Map Creation");

        Debug.Log("Saved info: \n " + MapHeight + "\n" + MapWidth + "\n" + MapSizeString );

        Map = new int[ MapHeight, MapWidth ];
        ObjectMap = new GameObject[ MapHeight, MapWidth ];

        for ( int i = 0; i < MapHeight; i++ ) {
            for ( int j = 0; j < MapWidth; j++ ) {
                Map[i, j] = 0;

                Vector3 InstancePosition = new Vector3( j - Mathf.FloorToInt( MapWidth / 2), ( i - Mathf.FloorToInt( MapHeight / 2 )) * -1, 0 );
                ObjectMap[i, j] = Instantiate<GameObject>( Tile, InstancePosition, Tile.transform.rotation );

                if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
                    ObjectMap[i, j].GetComponent<SpriteRenderer>().color = new Color32( 23, 23, 23, 255 );
                } else {
                    ObjectMap[i, j].GetComponent<SpriteRenderer>().color = Color.white;
                }


                if ( i == 0 || i == MapHeight - 1 || j == 0 || j == MapWidth - 1 ) {
                    Map[i, j] = 1;
                    if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
                        ObjectMap[i, j].GetComponent<SpriteRenderer>().color = Color.white;
                    } else {
                        ObjectMap[i, j].GetComponent<SpriteRenderer>().color = new Color32( 23, 23, 23, 255 );
                    }
                }
            }
        }

        Debug.Log("Map Created");
    }

    private string MapSizeString;

    private int MapHeight;
    private int MapWidth;

    private Vector3 PlayerStartPosition;
    private int PlayerStartX;
    private int PlayerStartY;

    public void SetPlayerStart( int _PlayerX, int _PlayerY ) {
        PlayerStartX = _PlayerX;
        PlayerStartY = _PlayerY;
    }
    public int GetPlayerStartX() {
        return PlayerStartX;
    }
    public int GetPlayerStartY() {
        return PlayerStartY;
    }
    public void SetPlayerStart( Vector3 _PlayerStartPosition ) {
        PlayerStartPosition = _PlayerStartPosition;
    }

    public int GetMapWidth() {
        return MapWidth;
    }
    public int GetMapHeight() {
        return MapHeight;
    }

    private int[,] Map;
    private int[,] _TempMap;
    private GameObject[,] ObjectMap;

    public GameObject[,] GetObjectMap() {
        return ObjectMap;
    }
    public int[,] GetMap() {
        return Map;
    }

    private bool Testing = false;
    public bool GetTestingState() {
        return Testing;
    }

    private string DrawType = "Tile";
    public string GetDrawType() {
        return DrawType;
    }

    public Image TileImage;
    public Image WallImage;
    public Image PlayerImage;

    public void SetTileDraw() {
        Paintbrush.transform.position = TileImage.transform.position;
        DrawType = "Tile";
    }
    public void SetWallDraw() {
        Paintbrush.transform.position = WallImage.transform.position;
        DrawType = "Wall";
    }
    public void SetPlayerDraw() {
        Paintbrush.transform.position = PlayerImage.transform.position;
        DrawType = "Player";
    }

    private bool PlayerPlaced() {
            for ( int i = 0; i < MapHeight; i++ ) {
                for ( int j = 0; j < MapWidth; j++ ) {
                    if ( Map[i, j] == 2 ) {
                        return true;
                    }
                }
            }
            return false;
        }

    private bool _CanMove = true;
        private bool _CanMoveFinger = false;

        private Touch _UserTouch;
        private Vector2 _UserTouchBeginPosition;
        private Vector2 _UserTouchEndPosition;


    [Space(20)]

    public Text TestButtonText;
    public Button TestButton;
    public Sprite StartTestSprite;
    public Sprite ExitTestSprite;

    public void ToggleTest() {
            if ( !Testing && PlayerPlaced() ) {
                TestButtonText.text = "Exit";
                TestButton.image.sprite = ExitTestSprite;
                TestButton.GetComponent<RectTransform>().sizeDelta = new Vector2( 75, 75);
                Testing = true;

                _TempMap = new int[ MapHeight, MapWidth];
                for ( int i = 0; i < MapHeight; i++ ) {
                    for ( int j = 0; j < MapWidth; j++ ) {
                        _TempMap[i, j] = Map[i, j];
                    }
                }
            } else {
                TestButtonText.text = "Test";
                TestButton.image.sprite = StartTestSprite;
                TestButton.GetComponent<RectTransform>().sizeDelta = new Vector2( 65, 75);
                Testing = false;

                for ( int i = 0; i < MapHeight; i++ ) {
                    for ( int j = 0; j < MapWidth; j++ ) {
                        if ( Map[ i, j ] == 0 ) {
                            if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
                                ObjectMap[ i, j ].GetComponent<SpriteRenderer>().color = new Color32( 23, 23, 23, 255 );
                            } else {
                                ObjectMap[ i, j ].GetComponent<SpriteRenderer>().color = Color.white;
                            }
                        }
                    }
                }

                Player.transform.position = PlayerStartPosition;
                TargetPosition.transform.position = PlayerStartPosition;

                _TempMap[ PlayerStartX, PlayerStartY ] = 2;
            }
        }


    private int FindTravelDistance( string Direction ) {
            int Distance = 0;

            int PlayerX, PlayerY;
            PlayerX = PlayerY = 0;

            for ( int i = 0; i < MapHeight; i++ ) {
                for ( int j = 0; j < MapWidth; j++ ) {
                    if ( _TempMap[i, j] == 2 ) {
                        PlayerX = i;
                        PlayerY = j;
                    }
                }
            }

            if ( Direction == "Up" ) {
                while ( _TempMap[ PlayerX - 1, PlayerY] != 1 ) {
                    _TempMap[ PlayerX - 1, PlayerY ] = 2;
                    _TempMap[ PlayerX, PlayerY] = 0;
                    PlayerX = PlayerX - 1;
                    Distance++;
                }
            } else 
            if ( Direction == "Down" ) {
                while ( _TempMap[ PlayerX + 1, PlayerY] != 1 ) {
                    _TempMap[ PlayerX + 1, PlayerY ] = 2;
                    _TempMap[ PlayerX, PlayerY] = 0;
                    PlayerX = PlayerX + 1;
                    Distance++;
                }
            } else 
            if ( Direction == "Left" ) {
                while ( _TempMap[ PlayerX, PlayerY - 1] != 1 ) {
                    _TempMap[ PlayerX, PlayerY - 1] = 2;
                    _TempMap[ PlayerX, PlayerY] = 0;
                    PlayerY = PlayerY - 1;
                    Distance++;
                }
            } else 
            if ( Direction == "Right" ) {
                while ( _TempMap[ PlayerX, PlayerY + 1] != 1 ) {
                    _TempMap[ PlayerX, PlayerY + 1] = 2;
                    _TempMap[ PlayerX, PlayerY] = 0;
                    PlayerY = PlayerY + 1;
                    Distance++;
                }
            }

            return Distance;
        }

        void Update() {
            if ( Testing ) {
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
                                        TargetPosition.transform.position += new Vector3( -FindTravelDistance("Left"), 0, 0 );
                                        _CanMove = false;
                                    } else {
                                        Debug.Log("User swipe detected: Right");
                                        TargetPosition.transform.position += new Vector3( FindTravelDistance("Right"), 0, 0 );
                                        _CanMove = false;
                                    }
                                } else {
                                    if ( _UserSwipeYNormalized < 0f ) {
                                        Debug.Log("User swipe detected: Down");
                                        TargetPosition.transform.position += new Vector3( 0, -FindTravelDistance("Down"), 0 );
                                        _CanMove = false;
                                    } else {
                                        Debug.Log("User swipe detected: Up");
                                        TargetPosition.transform.position += new Vector3( 0, FindTravelDistance("Up"), 0 );
                                        _CanMove = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if ( !_CanMove ) {
                Player.GetComponent<Rigidbody2D>().position = 
                    Vector3.MoveTowards( 
                        Player.transform.position, 
                        TargetPosition.transform.position, 
                        Time.deltaTime * 35f 
                    );
                
                if ( Player.transform.position == TargetPosition.transform.position ) {
                    _CanMove = true;
                }
            }

        }

        public InputField MapCreatorTag;

        public void CreateMapCode() {
            string MapCode = "";


            MapCode += 
            "new int[ " + MapHeight + ", " + MapWidth + "] \n { \n";

            for ( int i = 0; i < MapHeight; i++ ) {
                MapCode += "{ ";
                for ( int j = 0; j < MapWidth; j++ ) {
                    if ( j != MapWidth - 1 ) {
                        if ( Map[i, j] == 0 ) {
                            MapCode += "0";
                        } else 
                        if ( Map[i, j] == 1 ) {
                            MapCode += "1";
                        } else
                        if ( Map[i, j] == 2 ) {
                            MapCode += "2";
                        }
                    } else {
                        if ( Map[i, j] == 0 ) {
                            MapCode += "0";
                        } else 
                        if ( Map[i, j] == 1 ) {
                            MapCode += "1";
                        } else
                        if ( Map[i, j] == 2 ) {
                            MapCode += "2";
                        }
                    }
                }
                if ( i != MapHeight - 1 ) {
                    MapCode += "}, \n";
                } else {
                    MapCode += "} \n";
                }

            }

            MapCode += "}, \n";
            MapCode += "0, \n";
            MapCode += "\"" + MapCreatorTag.transform.Find("Text").GetComponent<Text>().text + "\", \n";
            MapCode += "\"" + MapSizeString + "\"";

            Debug.Log( MapCode );

            GUIUtility.systemCopyBuffer = MapCode;
        }

        public void SendToInsta() {
            CreateMapCode();
            Application.OpenURL("https://www.instagram.com/penguin_apps");
        }

        public void SendToTiktok() {
            CreateMapCode();
            Application.OpenURL("https://vm.tiktok.com/ZM8bL53Cd/");
        }


    


}
