using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreatorAnimations : MonoBehaviour
{
    public GlobalVariables Variables;

    [Space(20)]

    public Animator LoaderAnimator;

    [Space(20)]

    public Animator MapSizeTopMenu;
    public Animator MapSizeBottomMenu;
    public Animator MapSizeDrop;

    public void HideMapSizeMenu() {
        MapSizeTopMenu.Play("TopMenuClose");
        MapSizeBottomMenu.Play("BottomMenuClose");
        MapSizeDrop.Play("ScaleClose");

        IEnumerator ShowCreatorIEnumerator() {
            yield return new WaitForSeconds(0.5f);
            ShowCreatorMenu();
        }
        StartCoroutine( ShowCreatorIEnumerator() );
    }

    [Space(20)]
    public Animator CreatorTopMenu;
    public Animator CreatorBottomMenu;

    public void ShowCreatorMenu() {
        CreatorTopMenu.Play("TopMenuOpen");
        CreatorBottomMenu.Play("BottomMenuOpen");
    }

    void Awake() {
        LoaderAnimator.Play("ScaleClose");
    }

    [Space(20)]
    public Image InitialMenuTop;
    public Image InitialMenuBottom;
    public Image CreatorMenuTop;
    public Image CreatorMenuBottom;
    public Dropdown Dropdown;
    public Image Item;

    public Text TermsText;
    public Image TermsMenu;
    public Image TermsButton;
    public Image RejectButton;
    public Image AcceptButton;

    [Space(20)]

    public Image Tile;
    public Image Wall;
    public Image Player;

    void Start() {
        if ( PlayerPrefs.GetInt("TermsAccepted", 0) == 1 ) {
            TermsMenu.gameObject.transform.position += Vector3.down * 5000f;
        }

        LoaderAnimator.gameObject.GetComponent<Image>().color = Variables.GetLastColor();
        InitialMenuTop.color = Variables.GetLastColor();
        InitialMenuBottom.color = Variables.GetLastColor();
        CreatorMenuTop.color = Variables.GetLastColor();
        CreatorMenuBottom.color = Variables.GetLastColor();
        Dropdown.image.color = Variables.GetLastColor();
        Item.color = Variables.GetLastColor();
        TermsButton.color = Variables.GetLastColor();
        RejectButton.color = Variables.GetLastColor();
        AcceptButton.color = Variables.GetLastColor();

        Player.color = Variables.GetLastPlayerColor();

        if ( PlayerPrefs.GetInt("DarkMode", 0) == 0 ) {
            Camera.main.backgroundColor = Color.white;
            Tile.color = new Color32( 23, 23, 23, 255 );
            Wall.color = Color.white;
            TermsMenu.color = Color.white;
            TermsText.color = new Color32( 23, 23, 23, 255 );
        } else {
            Camera.main.backgroundColor = new Color32( 23, 23, 23, 255 );
            Tile.color = Color.white;
            Wall.color = new Color32( 23, 23, 23, 255 );
            TermsMenu.color = new Color32( 23, 23, 23, 255 );
            TermsText.color = Color.white;
        }

        SendMenuImage.color = Variables.GetLastColor();
        InstaBack.color = Variables.GetLastPlayerColor();
        TiktokBack.color = Variables.GetLastPlayerColor();
        CopyBack.color = Variables.GetLastPlayerColor();
    }

    public void ChangeScene( string SceneName ) {
        LoaderAnimator.Play("ScaleOpen");
        IEnumerator WaitForAnimation() {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene( SceneName );
        }
        StartCoroutine( WaitForAnimation() );
    }

    public void OpenTerms() {
        Application.OpenURL("https://vkorp.com/terms-of-service-project-penguin/");
    }

    public void AcceptTerms() {
        PlayerPrefs.SetInt("TermsAccepted", 1);
        TermsMenu.gameObject.transform.position += Vector3.down * 5000f;
    }

    public Animator SendMenuAnimator;

    public void OpenSendMenu() {
        SendMenuAnimator.Play("SlideDownOpen");
    }

    public Image SendMenuImage;
    public Image InstaBack;
    public Image TiktokBack;
    public Image CopyBack;

    public void CloseSendMenu() {
        SendMenuAnimator.Play("SlideDownClose");
    }

    public void SendToInsta() {
        Application.OpenURL("https://instagram.com/penguin_apps");
    }

    public void SendToTiktok() {
        Application.OpenURL("https://vm.tiktok.com/ZM8VkA2Qe/");
    }

}
