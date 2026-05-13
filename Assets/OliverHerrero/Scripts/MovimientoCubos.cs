using UnityEngine;

public class MovimientoCubos : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cube;
    public Vector3 spawnPoint;
    [SerializeField] private float speed = 3.0f;
    private float destinationOffsetRange = 0.6f;
    private float offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = cube.transform.position;
        offset = Random.Range(-destinationOffsetRange, destinationOffsetRange);
    }
    void Update()
    {
        Vector3 direction = new Vector3(Camera.main.transform.position.x + offset, Camera.main.transform.position.y, Camera.main.transform.position.z);
        float velocity = speed * Time.deltaTime;
        cube.transform.position = Vector3.MoveTowards(cube.transform.position, direction, velocity);
    }
}
