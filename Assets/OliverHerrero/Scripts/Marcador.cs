using TMPro;
using UnityEngine;

public class Marcador : MonoBehaviour
{
    public static Marcador Instance;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Start()
    {
        scoreText.text = score.ToString();
    }
}
