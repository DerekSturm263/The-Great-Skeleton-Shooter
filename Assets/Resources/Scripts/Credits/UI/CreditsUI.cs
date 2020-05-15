using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class CreditsUI : MonoBehaviour
{
    private EventSystem eventSystem;

    [Header("Credits Settings")]
    public RectTransform buttonSelectionHighlight;
    public Animator buttonLayoutAnimator;
    public GameObject[] buttons = new GameObject[6];

    private GameObject lastSelected;
    public float waitTime;

    private GameObject none;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        none = new GameObject();
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

    public void OnBackButton()
    {
        StartCoroutine(BackButton());
    }

    private IEnumerator BackButton()
    {
        buttons[5].GetComponent<CanvasGroup>().interactable = false;
        buttonLayoutAnimator.SetBool("endScene", true);

        yield return new WaitForSeconds(waitTime / 3f);

        SceneManager.LoadScene("Title");
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
}
