﻿using System.Collections;
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

    public float waitTime;

    public static bool hasDisplayedMessage = false;

    private Transform freezeOnPause;

    private void Awake()
    {
        eventSystem = EventSystem.current;
        freezeOnPause = GameObject.FindGameObjectWithTag("FreezeOnPause").GetComponent<Transform>();
    }

    private void DisplayMessage()
    {
        hasDisplayedMessage = true;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) DisplayMessage();

        if (ZoneCaptureScript.numCaptured == 8 && !hasDisplayedMessage)
        {
            DisplayMessage();
        }
    }

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
