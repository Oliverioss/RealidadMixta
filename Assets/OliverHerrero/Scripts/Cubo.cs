using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Cubo : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    public Vector3 spawnPoint;
    private float time = 3.0f;
    [SerializeField] private Transform camara;
    [SerializeField] float distance = 10f;
    private Scene currentScene;
    private string sceneName;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        StartCoroutine(spawnTime(time));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator spawnTime(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            Vector3 direction = camara.forward;
            direction.y = 0f;
            Vector3 position = camara.position + direction * distance;
            GameObject cubePrefab = Instantiate(cube, position, Quaternion.identity);
            Destroy(cubePrefab, 3.5f);
            
        }
    }
}
