using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongPathController : MonoBehaviour
{

    public Vector2[] SetPaths;
    public int currentPathIndex = 0;
    public float speed = 4;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, SetPaths[currentPathIndex], speed * Time.deltaTime);
        if (transform.position.x == SetPaths[currentPathIndex].x && transform.position.y == SetPaths[currentPathIndex].y)
        {
            currentPathIndex++;
            // Check for last location in list, then go back to first location.
            if (currentPathIndex >= SetPaths.Length)
            {
                currentPathIndex = 0;
            }
        }
    }
}
