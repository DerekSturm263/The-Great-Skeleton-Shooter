using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    private EventSystem eventSystem;

    [Header("Victory Settings")]
    public RectTransform buttonSelectionHighlight;
    public GameObject victoryPopUp;
    public GameObject[] buttons = new GameObject[2];

    private GameObject lastSelected;
    public float waitTime;

    private GameObject none;

    public bool hasDisplayedMessage = false;

    private Transform freezeOnPause;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        none = new GameObject();
        freezeOnPause = GameObject.FindGameObjectWithTag("FreezeOnPause").GetComponent<Transform>();
    }

    private void Update()
    {
        #region Selection Handler

        // Make it so you can't deselect UI elements and cause any errors.
        if (victoryPopUp.activeSelf)
        {
            if (eventSystem.currentSelectedGameObject != null)
                lastSelected = eventSystem.currentSelectedGameObject;
            else
                eventSystem.SetSelectedGameObject(lastSelected);

            UpdateSelectionPosition();
        }

        #endregion

        if (ZoneCaptureScript.numCaptured == 8 && !hasDisplayedMessage)
        {
            hasDisplayedMessage = true;
            DisplayMessage();
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

    private void DisplayMessage()
    {
        StartCoroutine(EnablePopUp(victoryPopUp));
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

    #region Victory Button Methods

    public void OnCreditsButton()
    {
        StartCoroutine(DisablePopUp(victoryPopUp));
        StartCoroutine(CreditsButton());
    }

    public void OnTitleButton()
    {
        StartCoroutine(DisablePopUp(victoryPopUp));
        StartCoroutine(TitleButton());
    }



    private IEnumerator CreditsButton()
    {
        yield return new WaitForSeconds(waitTime / 3f);
        SceneManager.LoadScene("Credits");
    }

    private IEnumerator TitleButton()
    {
        yield return new WaitForSeconds(waitTime / 3f);
        SceneManager.LoadScene("Title");
    }

    #endregion
}
