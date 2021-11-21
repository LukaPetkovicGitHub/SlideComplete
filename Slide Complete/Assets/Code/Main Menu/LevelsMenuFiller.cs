using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsMenuFiller : MonoBehaviour
{
    public GlobalVariables Variables;

    public AnimationControl Animation;

    public Text TotalStars;

    public ScrollRect[] LevelPacks;
    public GameObject[] Overlays;

    public Button OriginalButton;

    public List<Button> InstantiatedButtons = new List<Button>();

    private int LevelsPerPack = 100;

    private float OffsetX = 0;
    private float OffsetY = 0;

    public Sprite FullStar;

    public int StarsCollected = 0;

    public Sprite OpenLock;

    void Start() {
        for ( int i = 0; i < LevelPacks.Length; i++ ) {
            OffsetX = -OriginalButton.GetComponent<RectTransform>().rect.width - OriginalButton.GetComponent<RectTransform>().rect.width / 4;
            OffsetY = -OriginalButton.GetComponent<RectTransform>().rect.height * 2;

            float OffsetXMiddle = LevelPacks[i].viewport.Find("Content").GetComponent<RectTransform>().rect.width / 2;

            if ( PlayerPrefs.GetInt("LevelPack2Unlocked", 0) == 1 ) {
                LevelPacks[1].viewport.Find("Overlay Lock").gameObject.transform.localPosition += Vector3.down * 10000;
            }

            for ( int j = 0; j < LevelsPerPack; j++ ) {
                Button ButtonInstance = Instantiate<Button>( OriginalButton );

                ButtonInstance.transform.SetParent( LevelPacks[i].viewport.Find("Content").transform, false );
                ButtonInstance.GetComponent<RectTransform>().anchorMin = new Vector2( 0.5f, 1 );
                ButtonInstance.GetComponent<RectTransform>().anchorMax = new Vector2( 0.5f, 1 );

                if ( j % 3 == 0 ) {
                    LevelPacks[i].viewport.Find("Content").GetComponent<RectTransform>().sizeDelta += 
                        new Vector2( 
                            0,
                            OriginalButton.GetComponent<RectTransform>().rect.height + OriginalButton.GetComponent<RectTransform>().rect.height / 4
                        );
                    OffsetY -= OriginalButton.GetComponent<RectTransform>().rect.height + OriginalButton.GetComponent<RectTransform>().rect.height / 4;
                    OffsetX = OffsetXMiddle - OriginalButton.GetComponent<RectTransform>().rect.width - OriginalButton.GetComponent<RectTransform>().rect.width / 4;
                } else 
                if ( j % 3 == 1 ) {
                    OffsetX = OffsetXMiddle;
                } else {
                    OffsetX = OffsetXMiddle + OriginalButton.GetComponent<RectTransform>().rect.width + OriginalButton.GetComponent<RectTransform>().rect.width / 4;
                }

                ButtonInstance.transform.localPosition = new Vector3( OffsetX, OffsetY, 0f );
                ButtonInstance.transform.Find("Text").GetComponent<Text>().text = ( i * 100 + j + 1 ).ToString();

                if ( j + i * 100 > PlayerPrefs.GetInt("Progressive_MaxLevel", 0) ) {
                    if ( PlayerPrefs.GetInt("DarkMode", 0) == 1 ) {
                        ButtonInstance.image.color = new Color32( 40, 40, 40, 255 );
                        ButtonInstance.transform.Find("Text").GetComponent<Text>().color = new Color32( 23, 23, 23, 255 );
                        ButtonInstance.transform.Find("Star 1").GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                        ButtonInstance.transform.Find("Star 2").GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                        ButtonInstance.transform.Find("Star 3").GetComponent<Image>().color = new Color32( 23, 23, 23, 255 );
                    } else {
                        ButtonInstance.image.color = new Color32( 245, 245, 245, 255 );
                        ButtonInstance.transform.Find("Text").GetComponent<Text>().color = Color.white;
                        ButtonInstance.transform.Find("Star 1").GetComponent<Image>().color = Color.white;
                        ButtonInstance.transform.Find("Star 2").GetComponent<Image>().color = Color.white;
                        ButtonInstance.transform.Find("Star 3").GetComponent<Image>().color = Color.white;
                    }
                } else {
                    if ( PlayerPrefs.GetInt("Level" + ( i * 100 + j + 1 ).ToString() + "_Stars") == 1 ) {
                        ButtonInstance.transform.Find("Star 1").GetComponent<Image>().sprite = FullStar;
                    } else 
                    if ( PlayerPrefs.GetInt("Level" + ( i * 100 + j + 1 ).ToString() + "_Stars") == 2 ) {
                        ButtonInstance.transform.Find("Star 1").GetComponent<Image>().sprite = FullStar;
                        ButtonInstance.transform.Find("Star 2").GetComponent<Image>().sprite = FullStar;
                    } else 
                    if ( PlayerPrefs.GetInt("Level" + ( i * 100 + j + 1 ).ToString() + "_Stars") == 3 ) {
                        ButtonInstance.transform.Find("Star 1").GetComponent<Image>().sprite = FullStar;
                        ButtonInstance.transform.Find("Star 2").GetComponent<Image>().sprite = FullStar;
                        ButtonInstance.transform.Find("Star 3").GetComponent<Image>().sprite = FullStar;
                    } 
                    ButtonInstance.image.color = Variables.GetLastColor();
                    StarsCollected += PlayerPrefs.GetInt("Level" + ( i * 100 + j + 1).ToString() + "_Stars");
                    ButtonInstance.onClick.AddListener( delegate{ Animation.StartLevel( int.Parse( ButtonInstance.transform.Find("Text").GetComponent<Text>().text ) - 1 ); } );

                }
                LevelPacks[i].viewport.Find("Overlay Lock").Find("Lock").Find("Text").GetComponent<Text>().text = StarsCollected + " / 200 ";

                InstantiatedButtons.Add(ButtonInstance);
            }

            TotalStars.text = StarsCollected.ToString() + " / 600";
            LevelPacks[1].viewport.Find("Overlay Lock").Find("Lock").Find("Text").GetComponent<Text>().text = StarsCollected + " / 200 ";

            if ( i == 0 ) {
                Overlays[0].transform.localPosition += Vector3.right * 5000f;
            } 
        }

        if ( PlayerPrefs.GetInt("LevelPack2Unlocked", 0) == 1 ) {
            LevelPacks[1].viewport.Find("Overlay Lock").transform.localPosition += Vector3.down * 5000f;
        }

        if ( StarsCollected >= 200 ) {
            LevelPacks[1].viewport.Find("Overlay Lock").Find("Lock").GetComponent<Button>().image.sprite = OpenLock;
            LevelPacks[1].viewport.Find("Overlay Lock").Find("Lock").GetComponent<Button>().onClick.AddListener( delegate { RemoveOverlay2(); } );
        }
    }

    public void RemoveOverlay2() {
        PlayerPrefs.SetInt("LevelPack2Unlocked", 1);
        LevelPacks[1].viewport.Find("Overlay Lock").gameObject.transform.localPosition += Vector3.down * 5000f;
    }
}
