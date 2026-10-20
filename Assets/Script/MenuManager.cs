using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Tooltip("Positions monde de la camera, dans l'ordre de gauche a droite.")]
    [SerializeField] private Vector3[] _cameraPositions;
    [SerializeField] private int _startPositionIndex = 1;
    [SerializeField, Min(0.01f)] private float _slideDuration = 0.3f;
    [Tooltip("Distance minimale du swipe, en proportion de la largeur de l'ecran.")]
    [SerializeField, Range(0.01f, 1f)] private float _swipeThreshold = 0.1f;

    private Camera mainCamera;
    private TouchManager _touchManager;
    private int _currentPositionIndex;
    private Vector2 _touchStartPosition;
    private bool _isTouching;
    private Vector3 _slideStartPosition;
    private float _slideElapsed;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (mainCamera == null || _cameraPositions == null || _cameraPositions.Length == 0)
        {
            Debug.LogWarning("MenuManager : une camera et au moins une position sont necessaires.", this);
            enabled = false;
            return;
        }

        _currentPositionIndex = Mathf.Clamp(_startPositionIndex, 0, _cameraPositions.Length - 1);
        mainCamera.transform.position = _cameraPositions[_currentPositionIndex];
        _slideStartPosition = mainCamera.transform.position;
        _slideElapsed = _slideDuration;
    }

    private void OnEnable()
    {
        _touchManager = FindAnyObjectByType<TouchManager>();
        if (_touchManager == null)
        {
            Debug.LogWarning("MenuManager : TouchManager introuvable. Lance le jeu depuis Main.", this);
            return;
        }

        _touchManager.OnTouchStart.AddListener(OnTouchStart);
        _touchManager.OnTouchEnd.AddListener(OnTouchEnd);
    }

    private void OnDisable()
    {
        _isTouching = false;
        if (_touchManager == null)
            return;

        _touchManager.OnTouchStart.RemoveListener(OnTouchStart);
        _touchManager.OnTouchEnd.RemoveListener(OnTouchEnd);
    }

    private void LateUpdate()
    {
        if (mainCamera == null || _slideElapsed >= _slideDuration)
            return;

        _slideElapsed += Time.unscaledDeltaTime;
        float progress = Mathf.SmoothStep(0f, 1f, _slideElapsed / _slideDuration);
        mainCamera.transform.position = Vector3.Lerp(
            _slideStartPosition, _cameraPositions[_currentPositionIndex], progress);
    }

    private void OnTouchStart(Vector3 position)
    {
        if (mainCamera == null)
            return;

        _touchStartPosition = mainCamera.WorldToScreenPoint(position);
        _isTouching = true;
    }

    private void OnTouchEnd(Vector3 position)
    {
        if (!_isTouching || mainCamera == null)
            return;

        _isTouching = false;
        Vector2 touchEndPosition = mainCamera.WorldToScreenPoint(position);
        Vector2 delta = touchEndPosition - _touchStartPosition;

        if (Mathf.Abs(delta.x) < Screen.width * _swipeThreshold ||
            Mathf.Abs(delta.x) <= Mathf.Abs(delta.y))
            return;

        GoToPosition(_currentPositionIndex + (delta.x < 0f ? 1 : -1));
    }

    public void GoToPosition(int index)
    {
        if (mainCamera == null || _cameraPositions == null || _cameraPositions.Length == 0)
            return;

        index = Mathf.Clamp(index, 0, _cameraPositions.Length - 1);
        if (index == _currentPositionIndex)
            return;

        _currentPositionIndex = index;
        _slideStartPosition = mainCamera.transform.position;
        _slideElapsed = 0f;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            _isTouching = false;
    }
}
