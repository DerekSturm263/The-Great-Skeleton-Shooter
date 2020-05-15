using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class TitleUI : MonoBehaviour
{
    private EventSystem eventSystem;
    public GameObject gameController;

    [Header("Title Settings")]
    public RectTransform buttonSelectionHighlight;
    public Animator buttonLayoutAnimator;
    public GameObject[] buttons = new GameObject[5];

    private GameObject lastSelected;
    public float waitTime;

    [Header("Multiplayer Pop-Up")]
    public GameObject multiplayerWarning;
    public GameObject multiplayerButtonYes, multiplayerButtonNo;

    [Header("Options Pop-Up")]
    public GameObject options;
    public GameObject optionsFullscreen, optionsMusic, optionsSoundEffects, optionsParticles, optionsBack;
    public TMPro.TMP_Text optionsFullscreenTMP, optionsMusicTMP, optionsSoundEffectsTMP, optionsParticlesTMP;

    [Header("Quit Pop-Up")]
    public GameObject quitConfirm;
    public GameObject quitButtonYes, quitButtonNo;

    private GameObject none;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        none = new GameObject();

        if (GameObject.FindGameObjectsWithTag("GameController").Length == 0)
        {
            GameObject gC = Instantiate(gameController);
            DontDestroyOnLoad(gC);

            GameController.SetFullscreen(Screen.fullScreen);
            GameController.SetMusic(true);
            GameController.SetSoundEffects(true);
            GameController.SetGraphics(GameController.GraphicsType.Fancy);
        }

        optionsFullscreenTMP.text = (GameController.isFullscreen) ? "On" : "Off";
        optionsMusicTMP.text = (GameController.hasMusic) ? "On" : "Off";
        optionsSoundEffectsTMP.text = (GameController.hasSoundEffects) ? "On" : "Off";
        optionsParticlesTMP.text = (GameController.graphicsType == GameController.GraphicsType.Fancy) ? "High" : "Low";
    }

    private void Start()
    {
        Invoke("EnableAnimators", waitTime);
    }

    private void Update()
    {
        // Make it so you can't deselect UI elements and cause any errors.
        if (eventSystem.currentSelectedGameObject != null)
            lastSelected = eventSystem.currentSelectedGameObject;
        else
            eventSystem.SetSelectedGameObject(lastSelected);

        UpdateSelectionPosition();
    }

    private void UpdateSelectionPosition()
    {
        RectTransform rT = buttonSelectionHighlight.GetComponent<RectTransform>();
        
        if (eventSystem.currentSelectedGameObject == none)
        {
            rT.position = new Vector2(-10f, -10f);
            rT.sizeDelta = new Vector2(0f, 0f);
        }
        else
        {
            RectTransform rT2 = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
            rT.position = rT2.position;
            rT.sizeDelta = rT2.sizeDelta;
        }
    }

    private void LateUpdate()
    {
        CanvasGroup buttonParent = eventSystem.currentSelectedGameObject.transform.parent.transform.parent.GetComponent<CanvasGroup>();

        if (buttonParent != null && buttonParent.GetComponent<CanvasGroup>() != null)
        {
            buttonSelectionHighlight.GetComponent<CanvasGroup>().alpha = buttonParent.alpha;
        }
    }

    private void EnableAnimators()
    {
        foreach (GameObject g in buttons)
        {
            g.GetComponent<Animator>().enabled = true;
        }

        eventSystem.SetSelectedGameObject(buttons[0]);
    }

    #region PopUp Methods

    private IEnumerator EnablePopUp(GameObject g)
    {
        g.GetComponent<Animator>().SetBool("fadeOut", false);
        g.SetActive(true);

        yield return null;
    }

    private IEnumerator DisablePopUp(GameObject g)
    {
        g.GetComponent<Animator>().SetBool("fadeOut", true);

        yield return new WaitForSeconds(0.5f);

        g.SetActive(false);
    }

    #endregion

    #region Title Button Methods

    public void OnSingleplayerButton()
    {
        if (multiplayerWarning.activeSelf || options.activeSelf || quitConfirm.activeSelf)
            return;

        StartCoroutine(SingleplayerButton());
    }

    public void OnMultiplayerButton()
    {
        if (multiplayerWarning.activeSelf || options.activeSelf || quitConfirm.activeSelf)
            return;

        StartCoroutine(MultiplayerButton());
    }

    public void OnOptionsButton()
    {
        if (multiplayerWarning.activeSelf || options.activeSelf || quitConfirm.activeSelf)
            return;

        StartCoroutine(OptionsButton());
    }

    public void OnCreditsButton()
    {
        if (multiplayerWarning.activeSelf || options.activeSelf || quitConfirm.activeSelf)
            return;

        StartCoroutine(CreditsButton());
    }

    public void OnQuitButton()
    {
        if (multiplayerWarning.activeSelf || options.activeSelf || quitConfirm.activeSelf)
            return;

        StartCoroutine(QuitButton());
    }

    private IEnumerator SingleplayerButton()
    {
        buttons[0].GetComponent<CanvasGroup>().interactable = false;

        yield return new WaitForSeconds(waitTime / 3f);

        GameController.SetPlayerCount(1);
        SceneManager.LoadScene("Main");
    }

    private IEnumerator MultiplayerButton()
    {
        StartCoroutine(EnablePopUp(multiplayerWarning));
        eventSystem.SetSelectedGameObject(multiplayerButtonYes);

        yield return new WaitForSeconds(waitTime / 3f);
    }

    private IEnumerator OptionsButton()
    {
        StartCoroutine(EnablePopUp(options));
        eventSystem.SetSelectedGameObject(optionsFullscreen);

        yield return new WaitForSeconds(waitTime / 3f);
    }

    private IEnumerator CreditsButton()
    {
        buttons[3].GetComponent<CanvasGroup>().interactable = false;
        //buttonLayoutAnimator.SetBool("endScene", true);

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene("Credits");
    }

    private IEnumerator QuitButton()
    {
        StartCoroutine(EnablePopUp(quitConfirm));
        eventSystem.SetSelectedGameObject(quitButtonYes);

        yield return new WaitForSeconds(waitTime / 3f);
    }

    #endregion

    #region Pop Up Button Methods

    // Multiplayer warning.
    public void OnMultiplayerYes()
    {
        StartCoroutine(DisablePopUp(multiplayerWarning));
        StartCoroutine(MultiplayerButtonYes());
        buttons[1].GetComponent<CanvasGroup>().interactable = false;
        eventSystem.SetSelectedGameObject(none);
    }

    public void OnMultiplayerNo()
    {
        StartCoroutine(DisablePopUp(multiplayerWarning));
        eventSystem.SetSelectedGameObject(buttons[1]);
    }

    // Options.
    public void OnOptionsFullscreen()
    {
        GameController.SetFullscreen(!GameController.isFullscreen);
        optionsFullscreenTMP.text = (GameController.isFullscreen) ? "On" : "Off";
        Screen.fullScreen = GameController.isFullscreen;
    }

    public void OnOptionsMusic()
    {
        GameController.SetMusic(!GameController.hasMusic);
        optionsMusicTMP.text = (GameController.hasMusic) ? "On" : "Off";
    }

    public void OnOptionsSoundEffects()
    {
        GameController.SetSoundEffects(!GameController.hasSoundEffects);
        optionsSoundEffectsTMP.text = (GameController.hasSoundEffects) ? "On" : "Off";
    }

    public void OnOptionsParticles()
    {
        GameController.SetGraphics((GameController.GraphicsType)((int) GameController.graphicsType * -1));
        optionsParticlesTMP.text = (GameController.graphicsType == GameController.GraphicsType.Fancy) ? "High" : "Low";
    }

    public void OnOptionsBack()
    {
        StartCoroutine(DisablePopUp(options));
        eventSystem.SetSelectedGameObject(buttons[2]);
    }

    // Quit confirm.
    public void OnQuitYes()
    {
        StartCoroutine(DisablePopUp(quitConfirm));
        StartCoroutine(QuitButtonYes());
        buttons[4].GetComponent<CanvasGroup>().interactable = false;
        eventSystem.SetSelectedGameObject(none);
    }

    public void OnQuitNo()
    {
        StartCoroutine(DisablePopUp(quitConfirm));
        eventSystem.SetSelectedGameObject(buttons[4]);
    }

    private IEnumerator MultiplayerButtonYes()
    {
        yield return new WaitForSeconds(waitTime / 3f);
        GameController.SetPlayerCount(2);
        SceneManager.LoadScene("Main");
    }

    private IEnumerator QuitButtonYes()
    {
        yield return new WaitForSeconds(waitTime / 3f);
        Application.Quit();
    }

    #endregion
}
