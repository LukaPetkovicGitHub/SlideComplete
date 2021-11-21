using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using GoogleMobileAds.Api;
using System;

public class Ads : MonoBehaviour
{
    public Manager Manager;


    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    void Start() {
        MobileAds.Initialize(initStatus => { });

        this.RequestBanner();
        this.RequestIntersticial();
        this.RequestRewarded();

        this.bannerView.Show();

    }

    private void RequestBanner() {
        string  adUnitId = "ca-app-pub-2326925561615149/2113458296";
        // Test string adUnitId = "ca-app-pub-3940256099942544/6300978111";

        this.bannerView = new BannerView( adUnitId, AdSize.Banner, AdPosition.Bottom );
        this.bannerView.LoadAd( CreateAdRequest() );
    }

    public void HideBanner() {
        this.bannerView.Hide();
        this.bannerView.Destroy();
    }

    string Destination = "";
    private void RequestIntersticial() {
        string  adUnitId = "ca-app-pub-2326925561615149/5565618146";
        // Test string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        this.interstitial = new InterstitialAd( adUnitId );
        this.interstitial.LoadAd( CreateAdRequest() );

        this.interstitial.OnAdClosed += HandleOnAdClosed;
    }

    public void HandleOnAdClosed( object sender, EventArgs args ) {
        PlayerPrefs.SetInt("AdsShown", 0 );
        SceneManager.LoadScene("Game");
    }

    public void ShowIntersticial( ) {
        if ( interstitial.IsLoaded() ) {
            interstitial.Show();
        } else {
            HideBanner();
            PlayerPrefs.SetInt("AdsShown", 0 );
            SceneManager.LoadScene("Game");
        }
    }

    private void RequestRewarded() {
        string  adUnitId = "ca-app-pub-2326925561615149/9071322696";
        // Test string adUnitId = "ca-app-pub-3940256099942544/1033173712";

        this.rewardedAd = new RewardedAd( adUnitId );
        this.rewardedAd.LoadAd( CreateAdRequest() );

        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    public void ShowRewarded() {
        if ( rewardedAd.IsLoaded() ) {
            rewardedAd.Show();
        } else {
            Manager.UseAdMoves();
        }
    }

    public void HandleUserEarnedReward( object sender, EventArgs args ) {
        Manager.UseAdMoves();
    }

    public void HandleRewardedAdClosed ( object sender, EventArgs args ) {
        Manager.UseAdMoves();
    }

    private AdRequest CreateAdRequest() {
        return new AdRequest.Builder().Build();
    }
}
