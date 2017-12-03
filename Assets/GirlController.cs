using UnityEngine;

public class GirlController : MonoBehaviour
{
    public float velocity;

    private Plane[] planes;
    private Collider2D collider;

    void Start()
    {
        var mainCamera = FindObjectOfType<Camera>();
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        collider = GetComponent<Collider2D>();

		var pos = transform.localPosition;
		pos.z = pos.y +10f;
		transform.localPosition = pos;
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
            TurnRound();
            pos.x += velocity * Time.deltaTime;
            if (Random.value < 0.5f) pos.z += Random.Range(-0.2f, 0.2f);

            transform.localPosition = pos;
        }
    }

    public void TurnRound()
    {
        velocity *= -1;

        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
