using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject GameUI;
    public Button BackToMainButton;

    // MainMenu
    public GameObject MainMenuUI;
    public Button PlayButton;

    // WINMENU
    public GameObject WinMenuUI;
    public Button PlayAgainButton;


    private void Awake()
    {
        MainMenuUI.SetActive(true);
        GameUI.SetActive(false);
        WinMenuUI.SetActive(false);


        PlayButton.onClick.AddListener(OnPlayClicked);
        BackToMainButton.onClick.AddListener(OnBackToMainClicked);
        PlayAgainButton.onClick.AddListener(OnPlayAgainClicked);

        ScanManager.OnPlayerWin += OnPlayerWin;
    }

    private void OnDestroy()
    {
        ScanManager.OnPlayerWin -= OnPlayerWin;
    }


    private void OnPlayClicked()
    {
        MainMenuUI.SetActive(false);
        GameUI.SetActive(true);
        WinMenuUI.SetActive(false);

        ResetPuzzle();
    }

    private void OnPlayerWin()
    {
        GameUI.SetActive(false);
        WinMenuUI.SetActive(true);
    }

    private void OnPlayAgainClicked()
    {
        WinMenuUI.SetActive(false);
        GameUI.SetActive(true);

        ResetPuzzle();
    }

    private void OnBackToMainClicked()
    {
        GameUI.SetActive(false);
        WinMenuUI.SetActive(false);
        MainMenuUI.SetActive(true);

        ResetPuzzle();
    }



    private void ResetPuzzle()
    {
        var scanManager = FindAnyObjectByType<ScanManager>();
        if (scanManager != null)
            scanManager.ResetColorbar();
    }
}
