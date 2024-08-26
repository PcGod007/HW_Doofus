using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text gameOverText;
    public GameObject[] elementsToHide;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }

        ShowUIElements(true);
    }

    public void GameOver()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }

        ShowUIElements(false);
        Time.timeScale = 0;
    }

    private void ShowUIElements(bool show)
    {
        foreach (GameObject element in elementsToHide)
        {
            if (element != null)
            {
                element.SetActive(show);
            }
        }
    }
}
