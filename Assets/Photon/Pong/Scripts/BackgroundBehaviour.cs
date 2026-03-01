using UnityEngine;

public class BackgroundBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var transform = GetComponent<Transform>();
        transform.Rotate(Vector3.forward, Random.Range(0, Mathf.PI * 2));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * 0.01f);
    }
}
