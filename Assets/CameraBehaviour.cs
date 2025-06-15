using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{

    public static CameraBehaviour instance = null;

    private float shake;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, -10f), 10f * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        shake = Mathf.Lerp(shake, 0f, 5f * Time.deltaTime);
        transform.localPosition += (Vector3)new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * shake;
    }

    public void Shake(float amount)
    {
        shake += amount;
    }

    public void Nudge(Vector2 dir)
    {
        transform.position += (Vector3)dir;
    }
}
