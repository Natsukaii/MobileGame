using UnityEngine;

public class Slider : MonoBehaviour, ITouch
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private Transform[] snapPoints;
    [SerializeField] private LayerMask obstacleLayer;
    
    private float delta;
    private GameObject[] occupiedBy;
    private Collider2D[] stickerColliders;

    void Awake()
    {
        occupiedBy = new GameObject[snapPoints.Length];
        stickerColliders = new Collider2D[snapPoints.Length];
    }

    public void ReleaseSnapPoint(GameObject sticker)
    {
        for (int i = 0; i < occupiedBy.Length; i++)
        {
            if (occupiedBy[i] == sticker)
            {
                occupiedBy[i] = null;
                stickerColliders[i] = null;
            }
        }
    }

    public Transform GetNearestAvailableSnapPoint(Vector3 position, GameObject sticker)
    {
        Transform nearest = null;
        float minDistance = float.MaxValue;
        int index = -1;

        for (int i = 0; i < snapPoints.Length; i++)
        {
            if (occupiedBy[i] != null && occupiedBy[i] != sticker) continue;

            float dist = Vector3.Distance(position, snapPoints[i].position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = snapPoints[i];
                index = i;
            }
        }

        if (nearest != null)
        {
            Slider[] allSliders = Object.FindObjectsByType<Slider>(FindObjectsSortMode.None);
            foreach (var s in allSliders)
            {
                s.ReleaseSnapPoint(sticker);
            }
            
            occupiedBy[index] = sticker;
            stickerColliders[index] = sticker.GetComponent<Collider2D>();
        }

        return nearest;
    }
    
    public void OnTouchStart(Vector3 position)
    {
        delta = transform.position.x - position.x;
    }

    public void OnTouchEnd(Vector3 position)
    {
    }

    public void OnTouchContinue(Vector3 position)
    {
        float targetX = position.x + delta;
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(targetX, minX, maxX);
        transform.position = newPos;
    }
}
