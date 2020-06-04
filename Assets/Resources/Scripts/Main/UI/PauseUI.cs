using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private EventSystem eventSystem;

    [Header("Pause Settings")]
    public RectTransform buttonSelectionHighlight;
    public GameObject pauseMenu;
    public GameObject[] buttons = new GameObject[4];

    [SerializeField] private static bool isPaused;
    private bool isPausing;
    private bool isFading;

    private GameObject lastSelected;
    public float waitTime;

    [Header("Options Pop-Up")]
    public GameObject options;
    public GameObject optionsFullscreen, optionsMusic, optionsSoundEffects, optionsParticles, optionsBack;
    public TMPro.TMP_Text optionsFullscreenTMP, optionsMusicTMP, optionsSoundEffectsTMP, optionsParticlesTMP;

    [Header("Credits Pop-Up")]
    public GameObject creditsConfirm;
    public GameObject creditsButtonYes, creditsButtonNo;

    [Header("Title Pop-Up")]
    public GameObject titleConfirm;
    public GameObject titleButtonYes, titleButtonNo;

    private GameObject none;

    [Space(10f)]
    public GameObject fancyGraphics;
    public GameObject fastGraphics;

    private Transform freezeOnPause;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        none = new GameObject();
        freezeOnPause = GameObject.FindGameObjectWithTag("FreezeOnPause").GetComponent<Transform>();

        optionsFullscreenTMP.text = (GameController.isFullscreen) ? "On" : "Off";
        optionsMusicTMP.text = (GameController.hasMusic) ? "On" : "Off";
        optionsSoundEffectsTMP.text = (GameController.hasSoundEffects) ? "On" : "Off";
        optionsParticlesTMP.text = (GameController.graphicsType == GameController.GraphicsType.Fancy) ? "High" : "Low";
    }

    public void Pause()
    {
        isPaused = true;
        isPausing = true;

        StartCoroutine(EnablePopUp(pauseMenu));
        eventSystem.SetSelectedGameObject(buttons[0]);

        foreach (Transform gameObject in freezeOnPause)
        {
            if (gameObject.GetComponent<Rigidbody2D>() != null) gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            if (gameObject.GetComponent<EntityData>() != null) gameObject.GetComponent<EntityData>().enabled = false;
            if (gameObject.GetComponent<EntityMove>() != null) gameObject.GetComponent<EntityMove>().enabled = false;
            if (gameObject.GetComponent<EntityActions>() != null) gameObject.GetComponent<EntityActions>().enabled = false;
            if (gameObject.GetComponent<ZoneCaptureScript>() != null) gameObject.GetComponent<ZoneCaptureScript>().enabled = false;
            if (gameObject.GetComponent<EnemySpawning>() != null) gameObject.GetComponent<EnemySpawning>().enabled = false;
        }

        isPausing = false;
    }

    private void Update()
    {
        #region Selection Handler

        // Make it so you can't deselect UI elements and cause any errors.
        if (eventSystem.currentSelectedGameObject != null)
            lastSelected = eventSystem.currentSelectedGameObject;
        else
            eventSystem.SetSelectedGameObject(lastSelected);

        UpdateSelectionPosition();

        #endregion

        if (Input.GetButtonDown("Pause") && !isPausing && !isFading && !VictoryUI.hasDisplayedMessage)
        {
            if (!isPaused) Pause();
            else  OnResumeButton();
        }
    }

    private void UpdateSelectionPosition()
    {
        try
        {
            RectTransform rT = buttonSelectionHighlight.GetComponent<RectTransform>();

            if (eventSystem.currentSelectedGameObject == none)
            {
                rT.position = new Vector2(-100f, -100f);
                rT.sizeDelta = new Vector2(0f, 0f);
            }
            else
            {
                RectTransform rT2 = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
                rT.position = rT2.position;
                rT.sizeDelta = rT2.sizeDelta;
            }
        }
        catch { }
    }

    private void LateUpdate()
    {
        try
        {
            CanvasGroup buttonParent = eventSystem.currentSelectedGameObject.transform.parent.transform.parent.GetComponent<CanvasGroup>();

            if (buttonParent != null && buttonParent.GetComponent<CanvasGroup>() != null)
            {
                buttonSelectionHighlight.GetComponent<CanvasGroup>().alpha = buttonParent.alpha;
            }
        }
        catch { }
    }

    #region PopUp Methods

    private IEnumerator EnablePopUp(GameObject g)
    {
        g.SetActive(true);
        g.GetComponent<Animator>().SetBool("fadeOut", false);

        yield return null;
    }

    private IEnumerator DisablePopUp(GameObject g)
    {
        g.GetComponent<Animator>().SetBool("fadeOut", true);

        yield return new WaitForSeconds(0.5f);

        g.SetActive(false);
    }

    #endregion

    #region Pause Button Methods
    
    public void OnResumeButton()
    {
        StartCoroutine(ResumeButton());
    }

    public void OnOptionsButton()
    {
        StartCoroutine(OptionsButton());
    }

    public void OnCreditsButton()
    {
        StartCoroutine(CreditsButton());
    }

    public void OnTitleButton()
    {
        StartCoroutine(TitleButton());
    }

    private IEnumerator ResumeButton()
    {
        isPaused = false;
        isPausing = true;

        if (pauseMenu.activeSelf) StartCoroutine(DisablePopUp(pauseMenu));
        else if (options.activeSelf) StartCoroutine(DisablePopUp(options));
        else if (creditsConfirm.activeSelf) StartCoroutine(DisablePopUp(creditsConfirm));
        else if (titleConfirm.activeSelf) StartCoroutine(DisablePopUp(titleConfirm));

        foreach (Transform gameObject in freezeOnPause)
        {
            if (gameObject.GetComponent<Rigidbody2D>() != null) gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            if (gameObject.GetComponent<EntityData>() != null) gameObject.GetComponent<EntityData>().enabled = true;
            if (gameObject.GetComponent<EntityMove>() != null) gameObject.GetComponent<EntityMove>().enabled = true;
            if (gameObject.GetComponent<EntityActions>() != null) gameObject.GetComponent<EntityActions>().enabled = true;
            if (gameObject.GetComponent<ZoneCaptureScript>() != null) gameObject.GetComponent<ZoneCaptureScript>().enabled = true;
            if (gameObject.GetComponent<EnemySpawning>() != null) gameObject.GetComponent<EnemySpawning>().enabled = true;
        }

        eventSystem.SetSelectedGameObject(none);
        yield return new WaitForSeconds(waitTime / 3f);
        isPausing = false;
    }

    private IEnumerator OptionsButton()
    {
        isFading = true;

        StartCoroutine(EnablePopUp(options));
        StartCoroutine(DisablePopUp(pauseMenu));
        eventSystem.SetSelectedGameObject(optionsFullscreen);

        yield return new WaitForSeconds(waitTime / 3f);

        isFading = false;
    }

    private IEnumerator CreditsButton()
    {
        isFading = true;

        StartCoroutine(EnablePopUp(creditsConfirm));
        StartCoroutine(DisablePopUp(pauseMenu));
        eventSystem.SetSelectedGameObject(creditsButtonYes);

        yield return new WaitForSeconds(waitTime / 3f);

        isFading = false;
    }

    private IEnumerator TitleButton()
    {
        isFading = true;

        StartCoroutine(EnablePopUp(titleConfirm));
        StartCoroutine(DisablePopUp(pauseMenu));
        eventSystem.SetSelectedGameObject(titleButtonYes);

        yield return new WaitForSeconds(waitTime / 3f);

        isFading = false;
    }

    #endregion

    #region Pop Up Button Methods

    // Options.
    public void OnOptionsFullscreen()
    {
        GameController.SetFullscreen(!GameController.isFullscreen);
        optionsFullscreenTMP.text = (GameController.isFullscreen) ? "On" : "Off";
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

        if (GameController.graphicsType == GameController.GraphicsType.Fast)
        {
            fancyGraphics.SetActive(false);
            fastGraphics.SetActive(true);
        }
        else
        {
            fancyGraphics.SetActive(true);
            fastGraphics.SetActive(false);
        }
    }

    public void OnOptionsBack()
    {
        StartCoroutine(DisablePopUp(options));
        StartCoroutine(EnablePopUp(pauseMenu));
        eventSystem.SetSelectedGameObject(buttons[1]);
    }

    // Credits confirm.
    public void OnCreditsYes()
    {
        StartCoroutine(DisablePopUp(creditsConfirm));
        StartCoroutine(CreditsButtonYes());
    }

    public void OnCreditsNo()
    {
        StartCoroutine(DisablePopUp(creditsConfirm));
        StartCoroutine(EnablePopUp(pauseMenu));
        eventSystem.SetSelectedGameObject(buttons[2]);
    }

    // Title confirm.
    public void OnTitleYes()
    {
        StartCoroutine(DisablePopUp(titleConfirm));
        StartCoroutine(TitleButtonYes());
    }

    public void OnTitleNo()
    {
        StartCoroutine(DisablePopUp(titleConfirm));
        StartCoroutine(EnablePopUp(pauseMenu));
        eventSystem.SetSelectedGameObject(buttons[3]);
    }



    private IEnumerator TitleButtonYes()
    {
        yield return new WaitForSeconds(waitTime / 3f);
        SceneManager.LoadScene("Title");
    }

    private IEnumerator CreditsButtonYes()
    {
        yield return new WaitForSeconds(waitTime / 3f);
        SceneManager.LoadScene("Credits");
    }

    #endregion
}
