# Project Context & Stack

- **Engine & Pipeline:** Unity 6 (6000.3.21f1) using Universal Render Pipeline (URP).
- **Target Platform:** Android mobile Augmented Reality using **Vuforia Engine AR (11.4.4)** and the **Unity Input System** (`UnityEngine.InputSystem`).
- **Project Concept:** Educational **Interactive Augmented Reality Storybook** titled **"Leo y las Guardianas de Ocho Patas"** (*Arañas de Chile*), teaching children and families to distinguish harmless/beneficial Chilean spiders from medically important venomous species.
- **Physical Book Structure:** Physical cover + 6 pages, with 5 interactive AR spider pages inside a single continuous AR scene (`StoryAR`).
- **Project Architecture:** 2-scene flow registered in Build Settings:
  1. `menu` (index 0): Main hub scene titled *"Leo y las Guardianas de Ocho Patas"* with Canvas UI buttons (`Leer Cuento` → `StoryAR` and `Salir`).
  2. `StoryAR` (index 1): Continuous Vuforia AR storybook scene with `ARCamera`, 5 `ImageTarget` pages, `VuforiaTargetController`, `SpiderPageController`, `StoryUIManager`, and 4 interactive buttons (*Semáforo de Peligro*, *Ver Comportamiento*, *Escuchar Narración*, *Volver al Menú*).

# Chilean Spider Species Catalog (5 AR Pages)

1. **Page 1 — Araña saltarina (*Salticidae*):** `Harmless` (Green) — Harmless garden ally; lives on sunny leaves and window frames. Target: `Page1_JumpingSpider_Target`.
2. **Page 2 — Araña tigre (*Scytodes globula*):** `Harmless` (Green) — Harmless home guardian; hunts recluse spiders; lives in corners and ceilings. Target: `Page2_TigerSpider_Target`.
3. **Page 3 — Araña de rincón (*Loxosceles laeta*):** `Venomous` (Red) — Medically important venomous spider; lives in dark places behind pictures, closets, and clothing. Target: `Page3_RecluseSpider_Target`.
4. **Page 4 — Viuda negra (*Latrodectus thoracicus*):** `Venomous` (Red) — Medically important venomous spider; glossy black body with red hourglass/spot; lives under stones, firewood, and dry grass. Target: `Page4_BlackWidow_Target`.
5. **Page 5 — Araña pollito chilena (*Grammostola rosea*):** `MildCaution` (Yellow) — Venom not dangerous to humans; mild caution due to urticating hairs; lives in earth burrows and under rocks. Target: `Page5_ChileanRoseTarantula_Target`.

# Language Rules

- **Explanations & Communication:** Always respond in clear, concise **Spanish**.
- **Code & Identifiers:** Write all C# scripts, class names, variables, methods, namespaces, and `[Tooltip]` attributes strictly in **English**, without unnecessary comments.

# Unity 6, Vuforia & C# Coding Standards

1. **Modern Unity 6 APIs:**
   - Use `Object.FindFirstObjectByType<T>()` or `Object.FindAnyObjectByType<T>()` instead of deprecated `FindObjectOfType`.
   - Use `Rigidbody.linearVelocity` instead of `Rigidbody.velocity`.
   - Use `UnityEngine.SceneManagement` (`SceneManager.LoadScene`) for transitions between `menu` and `StoryAR`.
2. **Vuforia Engine Best Practices:**
   - Use `ARCamera` instead of standard `Main Camera` in AR scenes.
   - Hook into `ObserverBehaviour.OnTargetStatusChanged` or `DefaultObserverEventHandler` (`OnTargetFound` / `OnTargetLost`) rather than polling tracking status inside `Update()`.
3. **Inspector & Performance:**
   - Keep fields `private` and expose them with `[SerializeField]`.
   - Cache component references in `Awake()` or `Start()`—never inside `Update()`.
   - Use `CompareTag("TagName")` instead of `gameObject.tag == "TagName"`.
   - Never modify `.meta` files manually unless explicitly requested.
