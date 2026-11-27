using UnityEngine;

public enum DangerLevel
{
    Harmless,
    MildCaution,
    Venomous
}

[CreateAssetMenu(fileName = "NewSpiderSpeciesData", menuName = "Chilean Spiders AR/Spider Species Data")]
public class SpiderSpeciesData : ScriptableObject
{
    [Header("Species Identity")]
    [Tooltip("Common name of the spider species.")]
    [SerializeField] private string commonName;

    [Tooltip("Scientific binomial or family name of the spider species.")]
    [SerializeField] private string scientificName;

    [Header("Safety Classification")]
    [Tooltip("Danger level used for the educational traffic-light indicator.")]
    [SerializeField] private DangerLevel dangerLevel = DangerLevel.Harmless;

    [Tooltip("Alert color displayed on the danger traffic-light card.")]
    [SerializeField] private Color alertColor = new Color(0.18f, 0.8f, 0.44f);

    [Header("Educational Content")]
    [Tooltip("Short summary of where this spider lives.")]
    [TextArea(2, 4)]
    [SerializeField] private string habitatSummary;

    [Tooltip("Practical safety or coexistence prevention tip.")]
    [TextArea(2, 4)]
    [SerializeField] private string preventionTip;

    [Tooltip("Storybook narrative text for this page.")]
    [TextArea(3, 6)]
    [SerializeField] private string pageNarrative;

    [Tooltip("Voiceover narration AudioClip for this page.")]
    [SerializeField] private AudioClip narrationClip;

    public string CommonName => commonName;
    public string ScientificName => scientificName;
    public DangerLevel DangerLevel => dangerLevel;
    public Color AlertColor => alertColor;
    public string HabitatSummary => habitatSummary;
    public string PreventionTip => preventionTip;
    public string PageNarrative => pageNarrative;
    public AudioClip NarrationClip => narrationClip;

    public string GetDangerLabel()
    {
        switch (dangerLevel)
        {
            case DangerLevel.Harmless:
                return "VERDE — Inofensiva / Aliada";
            case DangerLevel.MildCaution:
                return "AMARILLO — Precaución leve";
            case DangerLevel.Venomous:
                return "ROJO — Venenosa / Peligro";
            default:
                return "Sin clasificación";
        }
    }
}
