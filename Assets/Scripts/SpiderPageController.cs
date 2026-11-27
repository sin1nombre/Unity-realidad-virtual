using System.Collections;
using UnityEngine;

[RequireComponent(typeof(VuforiaTargetController))]
public class SpiderPageController : MonoBehaviour
{
    [Header("Species Configuration")]
    [Tooltip("ScriptableObject data for the spider on this page.")]
    [SerializeField] private SpiderSpeciesData speciesData;

    [Tooltip("Root GameObject of the 3D spider model over this ImageTarget.")]
    [SerializeField] private GameObject modelRoot;

    [Header("Animation Settings")]
    [Tooltip("Optional Animator component on the 3D spider model.")]
    [SerializeField] private Animator modelAnimator;

    [Tooltip("Animator trigger fired when the target is detected.")]
    [SerializeField] private string baseAnimationTrigger = "Idle";

    [Tooltip("Animator trigger fired when the user requests the behavior animation.")]
    [SerializeField] private string actionAnimationTrigger = "Action";

    [Header("Fallback Placeholder Animation")]
    [Tooltip("Duration in seconds of the fallback rotation/hop animation.")]
    [SerializeField] private float fallbackActionDuration = 0.75f;

    [Tooltip("Local hop height for the fallback placeholder animation.")]
    [SerializeField] private float fallbackJumpHeight = 0.06f;

    [Header("UI Reference")]
    [Tooltip("Scene StoryUIManager reference. Auto-resolved in Awake if empty.")]
    [SerializeField] private StoryUIManager uiManager;

    private Coroutine _fallbackActionCoroutine;
    private Renderer[] _modelRenderers;
    private Vector3 _initialModelLocalPosition;
    private Quaternion _initialModelLocalRotation;
    private Vector3 _initialModelLocalScale;
    private bool _hasCachedModelTransform;

    public SpiderSpeciesData SpeciesData => speciesData;

    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = Object.FindFirstObjectByType<StoryUIManager>();
        }

        if (modelRoot != null)
        {
            _initialModelLocalPosition = modelRoot.transform.localPosition;
            _initialModelLocalRotation = modelRoot.transform.localRotation;
            _initialModelLocalScale = modelRoot.transform.localScale;
            _hasCachedModelTransform = true;
            _modelRenderers = modelRoot.GetComponentsInChildren<Renderer>(true);

            if (modelAnimator == null)
            {
                modelAnimator = modelRoot.GetComponentInChildren<Animator>(true);
            }
        }
    }

    private void Start()
    {
        if (modelRoot != null)
        {
            modelRoot.SetActive(false);
        }
    }

    public void OnTargetFound()
    {
        if (modelRoot != null)
        {
            modelRoot.SetActive(true);
            SetModelRenderersEnabled(true);
        }

        if (HasValidAnimatorController() && !string.IsNullOrWhiteSpace(baseAnimationTrigger))
        {
            modelAnimator.SetTrigger(baseAnimationTrigger);
        }

        if (uiManager != null)
        {
            uiManager.SetActivePage(this);
        }
    }

    public void OnTargetLost()
    {
        StopFallbackAnimation();

        if (modelRoot != null)
        {
            modelRoot.SetActive(false);
        }

        if (uiManager != null)
        {
            uiManager.ClearActivePage(this);
        }
    }

    public void TriggerBehaviorAction()
    {
        if (modelRoot == null || !modelRoot.activeInHierarchy)
        {
            return;
        }

        if (HasValidAnimatorController() && !string.IsNullOrWhiteSpace(actionAnimationTrigger))
        {
            modelAnimator.SetTrigger(actionAnimationTrigger);
            return;
        }

        StopFallbackAnimation();
        _fallbackActionCoroutine = StartCoroutine(PlayFallbackBehaviorRoutine());
    }

    private void SetModelRenderersEnabled(bool isEnabled)
    {
        if (_modelRenderers == null)
        {
            return;
        }

        for (int i = 0; i < _modelRenderers.Length; i++)
        {
            if (_modelRenderers[i] != null)
            {
                _modelRenderers[i].enabled = isEnabled;
            }
        }
    }

    private bool HasValidAnimatorController()
    {
        return modelAnimator != null && modelAnimator.runtimeAnimatorController != null;
    }

    private void StopFallbackAnimation()
    {
        if (_fallbackActionCoroutine != null)
        {
            StopCoroutine(_fallbackActionCoroutine);
            _fallbackActionCoroutine = null;
        }

        if (modelRoot != null && _hasCachedModelTransform)
        {
            modelRoot.transform.localPosition = _initialModelLocalPosition;
            modelRoot.transform.localRotation = _initialModelLocalRotation;
            modelRoot.transform.localScale = _initialModelLocalScale;
        }
    }

    private IEnumerator PlayFallbackBehaviorRoutine()
    {
        if (modelRoot == null)
        {
            yield break;
        }

        float elapsed = 0f;
        float duration = Mathf.Max(0.2f, fallbackActionDuration);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float arc = Mathf.Sin(t * Mathf.PI);

            Vector3 position = _initialModelLocalPosition;
            position.y += arc * fallbackJumpHeight;
            modelRoot.transform.localPosition = position;
            modelRoot.transform.localRotation = _initialModelLocalRotation * Quaternion.Euler(0f, t * 360f, 0f);
            modelRoot.transform.localScale = _initialModelLocalScale * (1f + arc * 0.2f);

            yield return null;
        }

        modelRoot.transform.localPosition = _initialModelLocalPosition;
        modelRoot.transform.localRotation = _initialModelLocalRotation;
        modelRoot.transform.localScale = _initialModelLocalScale;
        _fallbackActionCoroutine = null;
    }
}
