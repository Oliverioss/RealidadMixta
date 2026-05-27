using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CubeManager : MonoBehaviour
{
    public GameObject[] cubes;
    public TextMeshProUGUI scoreText;


    private float topoTime = 0f;
    private float timeLimit = 3f;

    private bool touched = false;
    private bool waiting = false;
    private bool gameFinished = false;

    private int score = 0;
    public int choice = 0;
    public GameObject exitButton;

    void Start()
    {
        exitButton.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            timeLimit = 3f;
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            timeLimit = 1f;
        }

        foreach (var cube in cubes)
        {
            cube.GetComponent<Renderer>().material.color = Color.grey;

            XRSimpleInteractable interactable = cube.GetComponent<XRSimpleInteractable>();
            interactable.hoverEntered.AddListener(OnHoverEntered);
        }

        scoreText.text = "Score: " + score;
        ChangeColour();
    }

    void Update()
    {
        if (gameFinished || waiting)
        {
            return;
        }

        topoTime += Time.deltaTime;

        if (topoTime >= timeLimit)
        {
            if (!touched)
            {
                StartCoroutine(ChangeColourBlack());
            }
            else
            {
                touched = false;
                topoTime = 0f;
                ChangeColour();
            }
        }
    }

    private void ChangeColour()
    {
        foreach (var cube in cubes)
        {
            cube.GetComponent<Renderer>().material.color = Color.grey;
        }

        int maxOptions;

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            maxOptions = 3;
        }
        else
        {
            maxOptions = 5;
        }
        choice = Random.Range(0, maxOptions);

        cubes[choice].GetComponent<Renderer>().material.color = Color.red;
    }

    private IEnumerator ChangeColourBlack()
    {
        waiting = true;

        cubes[choice].GetComponent<Renderer>().material.color = Color.black;

        yield return new WaitForSeconds(0.5f);

        cubes[choice].GetComponent<Renderer>().material.color = Color.grey;

        touched = false;
        topoTime = 0f;

        ChangeColour();

        waiting = false;
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (gameFinished || touched)
            return;

        if (args.interactableObject.transform.gameObject == cubes[choice])
        {
            touched = true;

            cubes[choice].GetComponent<Renderer>().material.color = Color.green;

            score += 10;
            scoreText.text = "Score: " + score;

            if (score >= 100)
            {
                GameWon();
            }
        }
    }

    private void GameWon()
    {
        gameFinished = true;

        StopAllCoroutines();

        foreach (var cube in cubes)
        {
            cube.GetComponent<Renderer>().material.color = Color.yellow;
        }

        exitButton.SetActive(true);
        scoreText.text = "¡HAS GANADO!";

    }

    public void ExitGame()  
    {
        SceneManager.LoadScene("Menu");
    }
}