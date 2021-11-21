using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimationControl : MonoBehaviour
{
    public Animator LoaderAnimator;
    public GlobalVariables Variables;

    public Button Play;
    public Button Levels;
    public Button Creator;
    public Image TopMenu;
    public Image BottomMenu;
    public Image Sound;

    public Sprite SoundOn;
    public Sprite SoundOff;

    void Awake() {
        LoaderAnimator.gameObject.GetComponent<Image>().color = Variables.GetLastColor();
        LoaderAnimator.Play("ScaleClose");

        Level.text = "Level " + ( PlayerPrefs.GetInt("Progressive_MaxLevel", 0) + 1 ).ToString();

        Play.image.color = Variables.GetLastColor();
        Levels.image.color = Variables.GetLastColor();
        Creator.image.color = Variables.GetLastColor();
        TopMenu.color = Variables.GetLastColor();
        BottomMenu.color = Variables.GetLastColor();

        if ( PlayerPrefs.GetInt("Sound", 1) == 0 ) {
            Sound.sprite = SoundOff;
        } else {
            Sound.sprite = SoundOn;
        }

        if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {

            Camera.main.backgroundColor = Color.white;
            Title.color = new Color32( 23, 23, 23, 255 );
            Level.color = new Color32( 23, 23, 23, 255 );
            Sound.color = new Color32( 240, 240, 240, 255 );

            DarkModeText.color = new Color32( 23, 23, 23, 255 );
            DarkMode.image.sprite = DarkModeOff;
            DarkMode.image.color = new Color32( 23, 23, 23, 255 );

            for ( int i = 0; i < MenuFiller.LevelPacks.Length; i++ ) {
                MenuFiller.LevelPacks[i].GetComponent<Image>().color = Color.white;

                if ( MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock") != null ) {
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").GetComponent<Image>().color = Color.white;
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Text").GetComponent<Text>().color = new Color32( 23, 23, 23, 255 );
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").Find("Text").GetComponent<Text>().color = new Color32( 23, 23, 23, 255 );
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").GetComponent<Button>().image.color = Variables.GetLastColor();
                }
            }

        } else {

            Camera.main.backgroundColor = new Color32( 23, 23, 23, 255 );
            Title.color = Color.white;
            Level.color = Color.white;
            Sound.color = new Color32( 40, 40, 40, 255 );

            DarkModeText.color = Color.white;
            DarkMode.image.sprite = DarkModeOn;
            DarkMode.image.color = Variables.GetLastColor();

            for ( int i = 0; i < MenuFiller.LevelPacks.Length; i++ ) {
                MenuFiller.LevelPacks[i].GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                if ( MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock") != null ) {
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Text").GetComponent<Text>().color = Color.white;
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").Find("Text").GetComponent<Text>().color = Color.white;
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").GetComponent<Button>().image.color = Variables.GetLastColor();
                }
            }

        }

        if ( MenuFiller.StarsCollected >= 200 ) {
            MenuFiller.LevelPacks[1].viewport.Find("Overlay Lock").Find("Lock").GetComponent<Button>().image.sprite = OpenLock;
            MenuFiller.LevelPacks[1].viewport.Find("Overlay Lock").Find("Lock").GetComponent<Button>().onClick.AddListener( delegate { RemoveOverlay2(); } );
        }

    }

    public void ToggleSound() {
        if ( PlayerPrefs.GetInt("Sound", 1 ) == 0 ) {
            PlayerPrefs.SetInt("Sound", 1 );
            Sound.sprite = SoundOn;
        } else {
            PlayerPrefs.SetInt("Sound", 0 );
            Sound.sprite = SoundOff;
        }
    }

    public void ChangeScene( string SceneName ) {
        LoaderAnimator.Play("ScaleOpen");
        IEnumerator WaitForLoader() {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Creator");
        }
        StartCoroutine( WaitForLoader() );
    }

    public void ContinueLevels() {
        PlayerPrefs.SetInt("Progressive_Level", PlayerPrefs.GetInt("Progressive_MaxLevel", 0) );
        LoaderAnimator.Play("ScaleOpen");
        IEnumerator WaitLoader() {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Game");
        }
        StartCoroutine( WaitLoader() );
    }

    public void StartLevel( int LevelIndex ) {
        PlayerPrefs.SetInt("Progressive_Level", LevelIndex);
        LoaderAnimator.Play("ScaleOpen");
        Debug.Log( "Level " + LevelIndex.ToString() + " Started!" );
        IEnumerator WaitLoader() {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Game");
        }
        StartCoroutine( WaitLoader() );
    }

    public Animator LevelsAnimator;
    public Animator TopMenuAnimator;
    public Animator BottomMenuAnimator;

    public void OpenLevels() {
        LevelsAnimator.Play("SlideDownOpen");
        TopMenuAnimator.Play("TopMenuOpen");
        BottomMenuAnimator.Play("BottomMenuOpen");
    }

    public void CloseLevels() {
        LevelsAnimator.Play("SlideDownClose");
        TopMenuAnimator.Play("TopMenuClose");
        BottomMenuAnimator.Play("BottomMenuClose");
    }

    public Text Title;
    public Text Level;
    public LevelsMenuFiller MenuFiller;

    public Button DarkMode;
    public Sprite DarkModeOn;
    public Sprite DarkModeOff;
    public Text DarkModeText;

    public Sprite OpenLock;

    public void ToggleDarkMode() {
        if ( PlayerPrefs.GetInt("DarkMode", 0) == 1 ) {

            PlayerPrefs.SetInt( "DarkMode", 0 );

            Camera.main.backgroundColor = Color.white;
            Title.color = new Color32( 23, 23, 23, 255 );
            Level.color = new Color32( 23, 23, 23, 255 );
            Sound.color = new Color32( 240, 240, 240, 255 );

            DarkModeText.color = new Color32( 23, 23, 23, 255 );
            DarkMode.image.sprite = DarkModeOff;
            DarkMode.image.color = new Color32( 23, 23, 23, 255 );

            for ( int i = 0; i < MenuFiller.LevelPacks.Length; i++ ) {
                MenuFiller.LevelPacks[i].GetComponent<Image>().color = Color.white;

                if ( MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock") != null ) {
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").GetComponent<Image>().color = Color.white;
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Text").GetComponent<Text>().color = new Color32( 23, 23, 23, 255 );
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").Find("Text").GetComponent<Text>().color = new Color32( 23, 23, 23, 255 );
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").GetComponent<Button>().image.color = Variables.GetLastColor();
                }

                for ( int j = 0; j < 100; j++ ) {
                    if ( i * 100 + j > PlayerPrefs.GetInt("Progressive_MaxLevel", 0) ) {
                        MenuFiller.InstantiatedButtons[i * 100 + j].image.color = new Color32( 245, 245, 245, 255 );
                        MenuFiller.InstantiatedButtons[i * 100 + j].transform.Find("Text").GetComponent<Text>().color = Color.white;
                        MenuFiller.InstantiatedButtons[i * 100 + j].transform.Find("Star 1").GetComponent<Image>().color = Color.white;
                        MenuFiller.InstantiatedButtons[i * 100 + j].transform.Find("Star 2").GetComponent<Image>().color = Color.white;
                        MenuFiller.InstantiatedButtons[i * 100 + j].transform.Find("Star 3").GetComponent<Image>().color = Color.white;
                    }
                }
            }

        } else {

            PlayerPrefs.SetInt( "DarkMode", 1 );

            Camera.main.backgroundColor = new Color32( 23, 23, 23, 255 );
            Title.color = Color.white;
            Level.color = Color.white;
            Sound.color = new Color32( 40, 40, 40, 255 );

            DarkModeText.color = Color.white;
            DarkMode.image.sprite = DarkModeOn;
            DarkMode.image.color = Variables.GetLastColor();

            for ( int i = 0; i < MenuFiller.LevelPacks.Length; i++ ) {
                MenuFiller.LevelPacks[i].GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                if ( MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock") != null ) {
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Text").GetComponent<Text>().color = Color.white;
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").Find("Text").GetComponent<Text>().color = Color.white;
                    MenuFiller.LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").GetComponent<Button>().image.color = Variables.GetLastColor();
                }

                for ( int j = 0; j < 100; j++ ) {
                    if ( i * 100 + j > PlayerPrefs.GetInt("Progressive_MaxLevel", 0) ) {
                        MenuFiller.InstantiatedButtons[ i * 100 + j ].image.color = new Color32( 40, 40, 40, 255 );
                        MenuFiller.InstantiatedButtons[i * 100 + j].transform.Find("Text").GetComponent<Text>().color = new Color32( 23, 23, 23, 255 );
                        MenuFiller.InstantiatedButtons[i * 100 + j].transform.Find("Star 1").GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                        MenuFiller.InstantiatedButtons[i * 100 + j].transform.Find("Star 2").GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                        MenuFiller.InstantiatedButtons[i * 100 + j].transform.Find("Star 3").GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                    }
                }
            }

        }
    }

    public int CurrentPack = 0;

    public void NextPack() {
        if ( CurrentPack == 0 ) {
            MenuFiller.LevelPacks[1].GetComponent<Animator>().Play("SlideDownOpen");
            BottomMenu.transform.Find("Text").GetComponent<Text>().text = "Level Pack " + 2;
            CurrentPack++;
        } else 
        if ( CurrentPack == 1 ) {
            MenuFiller.LevelPacks[2].GetComponent<Animator>().Play("SlideDownOpen");
            BottomMenu.transform.Find("Text").GetComponent<Text>().text = "Level Pack " + 3;
            CurrentPack++;
        }
    }

    public void LastPack() {
        if ( CurrentPack == 1 ) {
            MenuFiller.LevelPacks[1].GetComponent<Animator>().Play("SlideDownClose");
            BottomMenu.transform.Find("Text").GetComponent<Text>().text = "Level Pack " + 1;
            CurrentPack--;
        } else 
        if ( CurrentPack == 2 ) {
            MenuFiller.LevelPacks[2].GetComponent<Animator>().Play("SlideDownClose");
            BottomMenu.transform.Find("Text").GetComponent<Text>().text = "Level Pack " + 2;
            CurrentPack--;
        }
    }

    public void RemoveOverlay2() {
        PlayerPrefs.SetInt("LevelPack2Unlocked", 1);
        MenuFiller.LevelPacks[1].viewport.Find("Overlay Lock").gameObject.transform.localPosition += Vector3.down * 5000f;
    }

}
