using UnityEngine;
using UnityEngine.Events;
using Vuforia;

[RequireComponent(typeof(ObserverBehaviour))]
public class VuforiaTargetController : MonoBehaviour
{
    [Header("Tracking Events")]
    [Tooltip("Triggered once when the Vuforia target transitions to a tracked state.")]
    [SerializeField] private UnityEvent onTargetDetected;

    [Tooltip("Triggered once when the Vuforia target transitions out of a tracked state.")]
    [SerializeField] private UnityEvent onTargetLost;

    private ObserverBehaviour _observerBehaviour;
    private bool _isCurrentlyTracked;

    private void Awake()
    {
        _observerBehaviour = GetComponent<ObserverBehaviour>();
    }

    private void OnEnable()
    {
        _isCurrentlyTracked = false;

        if (_observerBehaviour != null)
        {
            _observerBehaviour.OnTargetStatusChanged += HandleTargetStatusChanged;
        }
    }

    private void OnDisable()
    {
        if (_observerBehaviour != null)
        {
            _observerBehaviour.OnTargetStatusChanged -= HandleTargetStatusChanged;
        }

        _isCurrentlyTracked = false;
    }

    private void HandleTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED ||
                         status.Status == Status.EXTENDED_TRACKED;

        if (isTracked && !_isCurrentlyTracked)
        {
            _isCurrentlyTracked = true;
            onTargetDetected?.Invoke();
        }
        else if (!isTracked && _isCurrentlyTracked)
        {
            _isCurrentlyTracked = false;
            onTargetLost?.Invoke();
        }
    }
}
