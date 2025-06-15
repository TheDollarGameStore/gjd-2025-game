using System.Collections;
using UnityEngine;

public enum PieceColor
{
    BLUE,
    GREEN,
    RED,
    YELLOW
}

public class Piece : MonoBehaviour
{
    [SerializeField] public PieceColor color;

    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private GameObject flasher;

    [SerializeField] private GameObject trailObject;

    public void UpdateYPos(int y)
    {
        sr.sortingOrder = y;
    }

    public IEnumerator FlashEffect()
    {
        CameraBehaviour.instance.Nudge(Vector2.up * 2f);
        flasher.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        flasher.SetActive(false);
    }

    public void TrailEffect(Vector2 startPoint, Vector2 endPoint)
    {
        Instantiate(trailObject).GetComponent<TrailEffect>().Init(startPoint, endPoint);
    }
}
