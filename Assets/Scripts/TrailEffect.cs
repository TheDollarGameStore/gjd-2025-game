using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    public float fadeDuration;

    private SpriteRenderer spriteRenderer;
    private float fadeTimer = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2 startPoint, Vector2 endPoint)
    {
        Vector2 direction = endPoint - startPoint;
        Vector2 midPoint = startPoint + direction * 0.5f;
        transform.position = midPoint;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        float distance = direction.magnitude;
        float spriteHeightUnits = 256f / spriteRenderer.sprite.pixelsPerUnit;
        Vector3 newScale = transform.localScale;
        newScale.y = distance / spriteHeightUnits;
        transform.localScale = newScale;
    }

    void Update()
    {
        fadeTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);

        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;

        if (fadeTimer >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }
}
