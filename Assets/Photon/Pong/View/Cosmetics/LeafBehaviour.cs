using UnityEngine;

public class LeafBehaviour : MonoBehaviour
{
    private float _progress = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _progress += Time.deltaTime;

        var transform = GetComponent<Transform>();
        transform.eulerAngles = new Vector3(0, 0, Mathf.Sin(_progress) * 20);
    }
}
