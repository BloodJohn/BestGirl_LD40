using UnityEngine;

public class GirlController : MonoBehaviour
{
    [SerializeField] private float velocity;

    private Plane[] planes;
    private Collider2D collider;

    void Start()
    {
        var cam = FindObjectOfType<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.localPosition;
        pos.x += velocity * Time.deltaTime;
        transform.localPosition = pos;

        
        if (!GeometryUtility.TestPlanesAABB(planes, collider.bounds))
        {
            Debug.LogFormat("hide");
            velocity *= -1;
            pos.x += velocity * Time.deltaTime;
            transform.localPosition = pos;

            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
