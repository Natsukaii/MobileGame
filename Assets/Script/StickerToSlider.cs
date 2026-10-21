using UnityEngine;

public class StickerToSlider : MonoBehaviour , ITouch
{
    [SerializeField] private float snapRange = 2f;
    private Vector3 delta;
    private Transform currentSnapPoint;
    private Slider currentSlider;
    
    public void OnTouchContinue(Vector3 position)
    {
        Vector3 newPosition = position + delta;
        newPosition.z = 0;
        transform.position = newPosition;
    }

    public void OnTouchStart(Vector3 position)
    {
        delta = transform.position - position;
        
        if (currentSnapPoint != null)
        {
            if (currentSlider != null)
            {
                currentSlider.ReleaseSnapPoint(gameObject);
            }
            transform.SetParent(null);
            currentSnapPoint = null;
            currentSlider = null;
        }
    }

    public void OnTouchEnd(Vector3 position)
    {
        Slider[] sliders = Object.FindObjectsByType<Slider>(FindObjectsSortMode.None);
        Slider nearestSlider = null;
        float minDist = snapRange;

        foreach (var slider in sliders)
        {
            float dist = Vector3.Distance(transform.position, slider.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestSlider = slider;
            }
        }

        if (nearestSlider != null)
        {
            Transform snap = nearestSlider.GetNearestAvailableSnapPoint(transform.position, gameObject);
            if (snap != null)
            {
                currentSnapPoint = snap;
                currentSlider = nearestSlider;
                transform.SetParent(snap);
                transform.localPosition = Vector3.zero;
            }
        }
    }
}
