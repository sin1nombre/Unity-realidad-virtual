using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryUIManager : MonoBehaviour
{
    [Header("Audio Playback")]
    [Tooltip("AudioSource used to play or pause the active page narration AudioClip.")]
    [SerializeField] private AudioSource narrationAudioSource;

    [Header("Header UI")]
    [Tooltip("Text displaying the detected species common and scientific name.")]
    [SerializeField] private TMP_Text activeSpeciesText;

    [Tooltip("Text displaying tracking prompts or user feedback.")]
    [SerializeField] private TMP_Text statusHintText;

    [Header("Danger Traffic-Light Card")]
    [Tooltip("Panel displaying the danger traffic-light card for the active spider.")]
    [SerializeField] private GameObject dangerCardPanel;

    [Tooltip("Header image tinted green, yellow, or red according to danger level.")]
    [SerializeField] private Image dangerTrafficLightImage;

    [Tooltip("Text displaying the danger level classification.")]
    [SerializeField] private TMP_Text dangerLevelBadgeText;

    [Tooltip("Text displaying common and scientific name on the card.")]
    [SerializeField] private TMP_Text dangerSpeciesTitleText;

    [Tooltip("Text displaying the spider habitat summary.")]
    [SerializeField] private TMP_Text habitatText;

    [Tooltip("Text displaying the prevention and coexistence tip.")]
    [SerializeField] private TMP_Text preventionTipText;

    [Tooltip("Text displaying the page story narrative.")]
    [SerializeField] private TMP_Text pageNarrativeText;

    [Header("Interactive Controls")]
    [Tooltip("Label inside the narration button toggled between play and pause.")]
    [SerializeField] private TMP_Text listenButtonLabel;

    [Tooltip("Button that toggles the danger traffic-light card.")]
    [SerializeField] private Button dangerTrafficLightButton;

    [Tooltip("Button that triggers the active spider behavior animation.")]
    [SerializeField] private Button behaviorActionButton;

    [Tooltip("Button that plays or pauses the narration clip.")]
    [SerializeField] private Button listenNarrationButton;

    private SpiderPageController _activePage;
    private bool _isNarrationPaused;

    private void Awake()
    {
        if (narrationAudioSource == null)
        {
            narrationAudioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (dangerCardPanel != null)
        {
            dangerCardPanel.SetActive(false);
        }

        UpdateNoTargetState();
    }

    public void SetActivePage(SpiderPageController pageController)
    {
        if (pageController == null)
        {
            return;
        }

        if (_activePage != pageController)
        {
            StopNarrationAudio();
        }

        _activePage = pageController;
        SpiderSpeciesData data = _activePage.SpeciesData;

        if (activeSpeciesText != null)
        {
            activeSpeciesText.text = data != null
                ? $"{data.CommonName} ({data.ScientificName})"
                : _activePage.name;
        }

        SetStatusMessage("Página detectada: usa los botones para explorar la especie.");
        PopulateDangerCard(data);
        SetControlsInteractable(true);
        UpdateListenButtonLabel(false);
    }

    public void ClearActivePage(SpiderPageController pageController)
    {
        if (_activePage != pageController)
        {
            return;
        }

        StopNarrationAudio();
        _activePage = null;

        if (dangerCardPanel != null)
        {
            dangerCardPanel.SetActive(false);
        }

        UpdateNoTargetState();
    }

    public void ToggleDangerCard()
    {
        if (_activePage == null || _activePage.SpeciesData == null || dangerCardPanel == null)
        {
            SetStatusMessage("Enfoca una página del libro para ver el Semáforo de Peligro.");
            return;
        }

        PopulateDangerCard(_activePage.SpeciesData);
        dangerCardPanel.SetActive(!dangerCardPanel.activeSelf);
    }

    public void TriggerActiveSpiderAction()
    {
        if (_activePage == null)
        {
            SetStatusMessage("Enfoca una página del libro para ver el comportamiento.");
            return;
        }

        _activePage.TriggerBehaviorAction();
        SetStatusMessage($"Mostrando comportamiento: {_activePage.SpeciesData?.CommonName ?? "Araña"}");
    }

    public void ToggleNarrationAudio()
    {
        if (_activePage == null || _activePage.SpeciesData == null)
        {
            SetStatusMessage("Enfoca una página del libro para escuchar la narración.");
            return;
        }

        if (narrationAudioSource == null)
        {
            return;
        }

        AudioClip clip = _activePage.SpeciesData.NarrationClip;
        if (clip == null)
        {
            SetStatusMessage($"Sin AudioClip asignado para {_activePage.SpeciesData.CommonName}.");
            return;
        }

        if (narrationAudioSource.isPlaying)
        {
            narrationAudioSource.Pause();
            _isNarrationPaused = true;
            UpdateListenButtonLabel(false);
            SetStatusMessage("Narración en pausa.");
            return;
        }

        if (_isNarrationPaused && narrationAudioSource.clip == clip)
        {
            narrationAudioSource.UnPause();
            _isNarrationPaused = false;
            UpdateListenButtonLabel(true);
            SetStatusMessage("Reanudando narración...");
            return;
        }

        narrationAudioSource.clip = clip;
        narrationAudioSource.Play();
        _isNarrationPaused = false;
        UpdateListenButtonLabel(true);
        SetStatusMessage($"Escuchando narración: {_activePage.SpeciesData.CommonName}");
    }

    private void PopulateDangerCard(SpiderSpeciesData data)
    {
        if (data == null)
        {
            return;
        }

        if (dangerTrafficLightImage != null)
        {
            dangerTrafficLightImage.color = data.AlertColor;
        }

        if (dangerLevelBadgeText != null)
        {
            dangerLevelBadgeText.text = data.GetDangerLabel();
        }

        if (dangerSpeciesTitleText != null)
        {
            dangerSpeciesTitleText.text = $"{data.CommonName}\n<i>{data.ScientificName}</i>";
        }

        if (habitatText != null)
        {
            habitatText.text = $"<b>Hábitat:</b> {data.HabitatSummary}";
        }

        if (preventionTipText != null)
        {
            preventionTipText.text = $"<b>Prevención:</b> {data.PreventionTip}";
        }

        if (pageNarrativeText != null)
        {
            pageNarrativeText.text = $"<b>Cuento:</b> {data.PageNarrative}";
        }
    }

    private void StopNarrationAudio()
    {
        if (narrationAudioSource != null && (narrationAudioSource.isPlaying || _isNarrationPaused))
        {
            narrationAudioSource.Stop();
        }

        _isNarrationPaused = false;
        UpdateListenButtonLabel(false);
    }

    private void UpdateNoTargetState()
    {
        if (activeSpeciesText != null)
        {
            activeSpeciesText.text = "Enfoca una página del libro";
        }

        SetStatusMessage("Apunta la cámara a cualquiera de las 5 páginas de arañas.");
        SetControlsInteractable(false);
        UpdateListenButtonLabel(false);
    }

    private void SetControlsInteractable(bool interactable)
    {
        if (dangerTrafficLightButton != null)
        {
            dangerTrafficLightButton.interactable = interactable;
        }

        if (behaviorActionButton != null)
        {
            behaviorActionButton.interactable = interactable;
        }

        if (listenNarrationButton != null)
        {
            listenNarrationButton.interactable = interactable;
        }
    }

    private void UpdateListenButtonLabel(bool isPlaying)
    {
        if (listenButtonLabel != null)
        {
            listenButtonLabel.text = isPlaying ? "Pausar Narración" : "Escuchar Narración";
        }
    }

    private void SetStatusMessage(string message)
    {
        if (statusHintText != null)
        {
            statusHintText.text = message;
        }
    }
}
