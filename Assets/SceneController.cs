using UnityEngine;

public class SceneController : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
        void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            if (hit.transform != null)
            {
                Debug.LogFormat("click {0}", hit.transform.name);
            }
        }
    }
}
