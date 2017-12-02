using UnityEngine;

public class GirlController : MonoBehaviour
{
    [SerializeField] private float velocity;

    private Plane[] planes;
    private Collider2D collider;

    void Start()
    {
        var mainCamera = FindObjectOfType<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //двигаем даму вправо
        var pos = transform.localPosition;
        pos.x += velocity * Time.deltaTime;
        transform.localPosition = pos;

        
        //если ушла за экран - поворачиваем обратно
        if (!GeometryUtility.TestPlanesAABB(planes, collider.bounds))
        {
            velocity *= -1;
            pos.x += velocity * Time.deltaTime;
            transform.localPosition = pos;

            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}
