using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GlobalVariables Variables;

    [Space(20)]

    public GameObject Tile;
    public GameObject Player;
    public GameObject TargetPosition;


    [Space(20)]

    public int MapWidth;
    public int MapHeight;

    public int[,] Map;
    public GameObject[,] ObjectMap;

    [Space(20)]
    public MapCreator MapCreator;
    public Ads Ads;

    [Space(20)]
    public Animator Loader;
    public Animator LevelWon;
    public Animator LevelLost;
    public Animator AddMoves;
    public Animator LevelPackCompleted;

    public Animator CountdownAnimator;

    [Space(20)]
    public int TilesLeft;

    [Space(20)]
    public Text Creator;
    public Text Level;

    public Image Star1;
    public Image Star2;
    public Image Star3;

    public Sprite FullStar;
    public Sprite EmptyStar;

    [Space(20)]

    public int MovesUsed = 0;

    public int Star1Moves = 0;
    public int Star2Moves = 0;
    public int Star3Moves = 0;

    public bool OverlayOpen = false;

    [Space(20)]

    public Button Retry;
    public Button Menu;
    public Text CurrentLevel;
    public Text CurrentCreator;

    [Space(20)]

    public AudioSource WinSound;
    public AudioSource LoseSound;
    public AudioSource CountdownSound;

    void Start() {
        Loader.gameObject.GetComponent<Image>().color = Variables.GetLastColor();
        Loader.Play("ScaleClose");

        Variables.GenerateNewMapColor();

        Player.GetComponent<SpriteRenderer>().color = Variables.GetPlayerColor();

        MapCreator.DrawMap();

        Level.text = "Level " + ( PlayerPrefs.GetInt("Progressive_Level", 0) + 1 );

        if ( Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetCreator() != "" ) {
            if ( Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetCreator().Contains("@")) {
                Creator.text = Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetCreator();
            } else {
                Creator.text = "@" + Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetCreator();
            }
        }

        Star1.color = Variables.GetMapColor();
        Star2.color = Variables.GetMapColor();
        Star3.color = Variables.GetMapColor();

        Star1.transform.Find("Text").GetComponent<Text>().color = Variables.GetPlayerColor();
        Star2.transform.Find("Text").GetComponent<Text>().color = Variables.GetPlayerColor();
        Star3.transform.Find("Text").GetComponent<Text>().color = Variables.GetPlayerColor();

        Star1Moves = Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetMoves() + 15;
        Star2Moves = Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetMoves() + 9;
        Star3Moves = Variables.GetMap( PlayerPrefs.GetInt("Progressive_Level", 0) ).GetMoves() + 3;

        Star1.transform.Find("Text").GetComponent<Text>().text = Star1Moves.ToString();
        Star2.transform.Find("Text").GetComponent<Text>().text = Star2Moves.ToString();
        Star3.transform.Find("Text").GetComponent<Text>().text = Star3Moves.ToString();


        if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
            Camera.main.backgroundColor = Color.white;
            Menu.image.color = new Color32( 245, 245, 245, 255 );
            Retry.image.color = new Color32( 245, 245, 245, 255 );
            CurrentLevel.color = new Color32( 23, 23, 23, 255 );
            CurrentCreator.color = new Color32( 245, 245, 245, 255 );
        } else {
            Camera.main.backgroundColor = new Color32( 23, 23, 23, 255 );
            Menu.image.color = new Color32( 40, 40, 40, 255 );
            Retry.image.color = new Color32( 40, 40, 40, 255 );
            CurrentLevel.color = Color.white;
            CurrentCreator.color = new Color32( 40, 40, 40, 255 );
        }

        IEnumerator Wait() {
            yield return new WaitForSeconds( 0.5f );
            Loader.gameObject.GetComponent<Image>().color = Variables.GetMapColor();
        }
        StartCoroutine( Wait() );
    }

    public void UpdateMoves() {
        Star1Moves--;
        Star2Moves--;
        Star3Moves--;

        Star1.transform.Find("Text").GetComponent<Text>().text = Star1Moves.ToString();
        Star2.transform.Find("Text").GetComponent<Text>().text = Star2Moves.ToString();
        Star3.transform.Find("Text").GetComponent<Text>().text = Star3Moves.ToString();

        if ( Star1Moves == 0 ) {
            if ( !AddMovesUsed ) {
                ShowAdMoves();
            } else {
                Loader.Play("ScaleOpen");
                IEnumerator Wait() {
                    yield return new WaitForSeconds( 0.5f );
                    LevelLost.Play("ScaleOpen");
                }
                StartCoroutine( Wait() );
            }
        }

        if ( Star2Moves <= 0 ) {
            Star2.transform.Find("Text").GetComponent<Text>().text = "";
            Star2.sprite = EmptyStar;
        }

        if ( Star3Moves <= 0 ) {
            Star3.transform.Find("Text").GetComponent<Text>().text = "";
            Star3.sprite = EmptyStar;
        }
    }

    bool LevelFinished = false;

    void Update() {
        if ( TilesLeft == 0 ) {
            LevelFinished = true;
        }
        if ( LevelFinished && !OverlayOpen ) {
            if ( Star1Moves >= 0 && TilesLeft == 0 ) {
                Ads.HideBanner();
                OverlayOpen = true;
                if ( PlayerPrefs.GetInt("Sound", 1) == 1 ) {
                    WinSound.Play();
                }
                ShowLevelWin();
            } else 
            if ( !AddMovesUsed && Star1Moves <= 0 && TilesLeft != 0 ) {
                OverlayOpen = true;
                Ads.HideBanner();
                ShowAdMoves();
            } else 
            if ( AddMovesUsed && Star1Moves <= 0 && TilesLeft != 0 ) {
                Ads.HideBanner();
                OverlayOpen = true;
                ShowLevelLost();
            }
        }
    }

    public void ShowLevelWin() {
        Destroy( LevelLost.gameObject );
        Destroy( AddMoves.gameObject );
        OverlayOpen = true;
        Loader.Play("ScaleOpen");
        LevelWon.Play("ScaleOpen");
        int StarsWon = 0;

        if ( Star2.sprite != EmptyStar ) {
            LevelWon.gameObject.transform.Find("Star (1)").GetComponent<Image>().sprite = FullStar;
            StarsWon = 2;
        } else {
            LevelWon.gameObject.transform.Find("Star (1)").GetComponent<Image>().sprite = EmptyStar;
            StarsWon = 1;
        }

        if ( Star3.sprite != EmptyStar ) {
            LevelWon.gameObject.transform.Find("Star (2)").GetComponent<Image>().sprite = FullStar;
            StarsWon = 3;
        } else {
            LevelWon.gameObject.transform.Find("Star (2)").GetComponent<Image>().sprite = EmptyStar;
        }

        if ( PlayerPrefs.GetInt("Level" + (PlayerPrefs.GetInt("Progressive_Level") + 1) + "_Stars", 0) < StarsWon ) {
            PlayerPrefs.SetInt("Level" + (PlayerPrefs.GetInt("Progressive_Level") + 1) + "_Stars", StarsWon );
        }

        PlayerPrefs.SetInt("Progressive_Level", PlayerPrefs.GetInt("Progressive_Level", 0) + 1);
        if ( PlayerPrefs.GetInt("Progressive_Level", 0) > PlayerPrefs.GetInt("Progressive_MaxLevel", 0) ) {
            PlayerPrefs.SetInt("Progressive_MaxLevel", PlayerPrefs.GetInt("Progressive_Level", 0) );
        }


        IEnumerator Wait() {
            Destroy( LevelLost.gameObject );
            Destroy( AddMoves.gameObject );


            if ( PlayerPrefs.GetInt("Progressive_Level", 0) != 99 && PlayerPrefs.GetInt("Progressive_Level", 0) != 199 ) {

                Destroy( LevelPackCompleted.gameObject );
                yield return new WaitForSeconds( 1f );
                LevelWon.Play("ScaleClose");
                yield return new WaitForSeconds( 0.5f );
                if ( PlayerPrefs.GetInt("AdsShown", 0) >= 4 ) {
                    Ads.ShowIntersticial();
                } else {
                    Ads.HideBanner();
                    PlayerPrefs.SetInt("AdsShown", PlayerPrefs.GetInt("AdsShown", 0) + 1 );
                    SceneManager.LoadScene( "Game" );
                }
            } else {
                Destroy( LevelWon.gameObject );
                yield return new WaitForSeconds( 0.5f );
                if ( PlayerPrefs.GetInt("Progressive_Level", 0) == 99 ) {
                    LevelPackCompleted.gameObject.transform.Find("Text").GetComponent<Text>().text = "Level Pack 1 Completed\n\nUnlock Level Pack 2\nTo Continue";
                } else 
                if ( PlayerPrefs.GetInt("Progressive_Level", 0) == 199 ) {
                    LevelPackCompleted.gameObject.transform.Find("Text").GetComponent<Text>().text = "Level Pack 2 Completed\n\nUnlock Level Pack 3\nTo Continue";
                }
                LevelPackCompleted.Play("ScaleClose");
                yield return new WaitForSeconds( 2f );
                Ads.HideBanner();
                SceneManager.LoadScene("Menu");
            }
        }

        StartCoroutine( Wait() );
    }

    public void ShowLevelLost() {
        OverlayOpen = true;
        Loader.Play("ScaleOpen");
        LevelLost.Play("ScaleOpen");
        CountdownSound.Stop();
        WinSound.Stop();
        if ( PlayerPrefs.GetInt("Sound", 1) == 1 ) {
            LoseSound.Play();
        }
        Destroy( LevelPackCompleted );
        Destroy( LevelWon );
        Destroy( AddMoves );
    }


    public bool AddMovesUsed = false;
    public void ShowAdMoves() {
        OverlayOpen = true;

        if ( PlayerPrefs.GetInt("Sound", 1) == 1 ) {
            CountdownSound.Play();
        }

        Loader.Play("ScaleOpen");
        AddMoves.Play("ScaleOpen");

        CountdownAnimator.Play("Countdown");

        IEnumerator Wait() {
            yield return new WaitForSeconds( 5f );
            if ( !AddMovesUsed ) {
                if ( PlayerPrefs.GetInt("Sound", 1) == 1 ) {
                    CountdownSound.Stop();
                }
                AddMoves.Play("ScaleClose");
                yield return new WaitForSeconds( 0.5f );
                LevelLost.Play("ScaleOpen");
                if ( PlayerPrefs.GetInt("Sound", 1) == 1 ) {
                    LoseSound.Play();
                }
            }
        }
        StartCoroutine( Wait() );
    }

    public void ShowRewarded() {
        Ads.ShowRewarded();
        if ( PlayerPrefs.GetInt("Sound", 1) == 1 ) {
            CountdownSound.Stop();
        }
    }

    public void UseAdMoves() {
        AddMovesUsed = true;
        OverlayOpen = false;
        Star1Moves = 15;
        if ( PlayerPrefs.GetInt("Sound", 1) == 1 ) {
            CountdownSound.Stop();
        }
        Star1.transform.Find("Text").GetComponent<Text>().text = Star1Moves.ToString();
        AddMoves.Play("ScaleClose");
        IEnumerator Wait() {
            yield return new WaitForSeconds( 0.5f );
            Loader.Play("ScaleClose");
        }

        StartCoroutine( Wait() );
    }

    public void ReloadScene( ) {
        Loader.Play("ScaleOpen");
        IEnumerator Wait() {
            yield return new WaitForSeconds( 0.5f );

            if ( PlayerPrefs.GetInt("AdsShown", 0) >= 4 ) {
                Ads.ShowIntersticial();
            } else {
                Ads.HideBanner();
                PlayerPrefs.SetInt("AdsShown", PlayerPrefs.GetInt("AdsShown", 0) + 1 );
                SceneManager.LoadScene( "Game" );
            }
        }
        StartCoroutine( Wait() );
    }

    public void HomeScene() {
        Loader.Play("ScaleOpen");
        IEnumerator Wait() {
            yield return new WaitForSeconds( 0.5f );
            Ads.HideBanner();
            SceneManager.LoadScene( "Menu" );
        }
        StartCoroutine( Wait() );
    }

    public void HomeSceneLost() {
        LevelLost.Play("ScaleClose");
        IEnumerator Wait() {
            yield return new WaitForSeconds( 0.5f );
            Ads.HideBanner();
            SceneManager.LoadScene( "Menu" );
        }
        StartCoroutine( Wait() );
    }

    public void ReloadSceneLost() {
        LevelLost.Play("ScaleClose");
        IEnumerator Wait() {
            yield return new WaitForSeconds( 0.5f );
            if ( PlayerPrefs.GetInt("AdsShown", 0) >= 4 ) {
                Ads.ShowIntersticial();
            } else {
                PlayerPrefs.SetInt("AdsShown", PlayerPrefs.GetInt("AdsShown", 0) + 1 );
                Ads.HideBanner();
                SceneManager.LoadScene( "Game" );
            }
        }
        StartCoroutine( Wait() );
    }



    
    
}
