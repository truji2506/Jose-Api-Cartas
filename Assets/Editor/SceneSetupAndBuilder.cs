#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SceneSetupAndBuilder : MonoBehaviour
{
    [MenuItem("Tools/1. Configurar Escena Completa")]
    public static void SetupScene()
    {
        // 1. Crear nueva escena limpia
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Configurar camara principal con fondo oscuro
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.clearFlags = CameraClearFlags.SolidColor;
            mainCam.backgroundColor = new Color(0.05f, 0.07f, 0.12f, 1f);
        }

        // 2. Crear EventSystem
        EventSystem es = Object.FindFirstObjectByType<EventSystem>();
        if (es == null)
        {
            GameObject esGo = new GameObject("EventSystem");
            es = esGo.AddComponent<EventSystem>();
            esGo.AddComponent<StandaloneInputModule>();
        }

        // 3. Crear Canvas Principal
        GameObject canvasGo = new GameObject("Canvas");
        Canvas canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGo.AddComponent<GraphicRaycaster>();

        // 4. Background Principal del Canvas
        GameObject bgGo = new GameObject("Background");
        bgGo.transform.SetParent(canvasGo.transform, false);
        Image bgImg = bgGo.AddComponent<Image>();
        bgImg.color = new Color(0.06f, 0.08f, 0.14f, 1f);
        RectTransform bgRect = bgGo.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;

        // Overlay decorativo con sprite si existe
        Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Background.png");
        if (bgSprite != null)
        {
            GameObject bgArtGo = new GameObject("BackgroundArt");
            bgArtGo.transform.SetParent(bgGo.transform, false);
            Image bgArtImg = bgArtGo.AddComponent<Image>();
            bgArtImg.sprite = bgSprite;
            bgArtImg.color = new Color(1f, 1f, 1f, 0.50f);
            bgArtImg.preserveAspect = true;
            RectTransform bgArtRect = bgArtGo.GetComponent<RectTransform>();
            bgArtRect.anchorMin = Vector2.zero;
            bgArtRect.anchorMax = Vector2.one;
            bgArtRect.sizeDelta = Vector2.zero;
        }

        // 5. Badge del Estudiante (Esquina Superior Izquierda con margen visible)
        GameObject studentBadgeGo = new GameObject("StudentBadge");
        studentBadgeGo.transform.SetParent(canvasGo.transform, false);
        Image studentBadgeImg = studentBadgeGo.AddComponent<Image>();
        studentBadgeImg.color = new Color(0.10f, 0.15f, 0.28f, 0.90f);
        RectTransform studentBadgeRect = studentBadgeGo.GetComponent<RectTransform>();
        studentBadgeRect.anchorMin = new Vector2(0f, 1f);
        studentBadgeRect.anchorMax = new Vector2(0f, 1f);
        studentBadgeRect.pivot = new Vector2(0f, 1f);
        studentBadgeRect.anchoredPosition = new Vector2(40f, -40f);
        studentBadgeRect.sizeDelta = new Vector2(420f, 65f);

        GameObject studentTextGo = new GameObject("StudentNameText");
        studentTextGo.transform.SetParent(studentBadgeGo.transform, false);
        TextMeshProUGUI studentTmp = studentTextGo.AddComponent<TextMeshProUGUI>();
        studentTmp.text = "Estudiante: José Ignacio Trujillo";
        studentTmp.fontSize = 22;
        studentTmp.fontStyle = FontStyles.Bold;
        studentTmp.color = new Color(0.85f, 0.92f, 1f, 1f);
        studentTmp.alignment = TextAlignmentOptions.Center;
        RectTransform studentRect = studentTextGo.GetComponent<RectTransform>();
        studentRect.anchorMin = Vector2.zero;
        studentRect.anchorMax = Vector2.one;
        studentRect.sizeDelta = Vector2.zero;

        // 6. Banner del Jugador Actual (Arriba al Centro)
        GameObject playerBannerGo = new GameObject("PlayerBanner");
        playerBannerGo.transform.SetParent(canvasGo.transform, false);
        Image playerBannerImg = playerBannerGo.AddComponent<Image>();
        playerBannerImg.color = new Color(0.12f, 0.18f, 0.32f, 0.95f);
        RectTransform playerBannerRect = playerBannerGo.GetComponent<RectTransform>();
        playerBannerRect.anchorMin = new Vector2(0.5f, 1f);
        playerBannerRect.anchorMax = new Vector2(0.5f, 1f);
        playerBannerRect.pivot = new Vector2(0.5f, 1f);
        playerBannerRect.anchoredPosition = new Vector2(0f, -40f);
        playerBannerRect.sizeDelta = new Vector2(460f, 75f);

        GameObject playerTextGo = new GameObject("PlayerNameText");
        playerTextGo.transform.SetParent(playerBannerGo.transform, false);
        TextMeshProUGUI playerTmp = playerTextGo.AddComponent<TextMeshProUGUI>();
        playerTmp.text = "Jugador 1 (José)";
        playerTmp.fontSize = 32;
        playerTmp.fontStyle = FontStyles.Bold;
        playerTmp.color = new Color(1f, 0.85f, 0.35f, 1f);
        playerTmp.alignment = TextAlignmentOptions.Center;
        RectTransform playerRect = playerTextGo.GetComponent<RectTransform>();
        playerRect.anchorMin = Vector2.zero;
        playerRect.anchorMax = Vector2.one;
        playerRect.sizeDelta = Vector2.zero;

        // 7. Contenedor de Carta TCG (Centro-Izquierda)
        GameObject cardContainerGo = new GameObject("CardContainer");
        cardContainerGo.transform.SetParent(canvasGo.transform, false);
        RectTransform cardContainerRect = cardContainerGo.AddComponent<RectTransform>();
        cardContainerRect.anchorMin = new Vector2(0.5f, 0.5f);
        cardContainerRect.anchorMax = new Vector2(0.5f, 0.5f);
        cardContainerRect.pivot = new Vector2(0.5f, 0.5f);
        cardContainerRect.anchoredPosition = new Vector2(-140f, -30f);
        cardContainerRect.sizeDelta = new Vector2(400f, 560f);

        // Borde exterior con resplandor suave
        Image cardBorderImg = cardContainerGo.AddComponent<Image>();
        cardBorderImg.color = new Color(0.18f, 0.48f, 0.85f, 1f);

        // Fondo interior de la carta
        GameObject cardBgGo = new GameObject("CardBackground");
        cardBgGo.transform.SetParent(cardContainerGo.transform, false);
        Image cardBgImg = cardBgGo.AddComponent<Image>();
        cardBgImg.color = new Color(0.08f, 0.11f, 0.19f, 0.98f);
        RectTransform cardBgRect = cardBgGo.GetComponent<RectTransform>();
        cardBgRect.anchorMin = Vector2.zero;
        cardBgRect.anchorMax = Vector2.one;
        cardBgRect.sizeDelta = new Vector2(-10f, -10f); // 5px borde

        // A. Cabecera con Nombre del Pokémon
        GameObject cardHeaderGo = new GameObject("CardHeader");
        cardHeaderGo.transform.SetParent(cardBgGo.transform, false);
        Image cardHeaderImg = cardHeaderGo.AddComponent<Image>();
        cardHeaderImg.color = new Color(0.14f, 0.22f, 0.38f, 1f);
        RectTransform cardHeaderRect = cardHeaderGo.GetComponent<RectTransform>();
        cardHeaderRect.anchorMin = new Vector2(0.04f, 0.86f);
        cardHeaderRect.anchorMax = new Vector2(0.96f, 0.97f);
        cardHeaderRect.sizeDelta = Vector2.zero;

        GameObject cardNameGo = new GameObject("CardNameText");
        cardNameGo.transform.SetParent(cardHeaderGo.transform, false);
        TextMeshProUGUI cardNameTmp = cardNameGo.AddComponent<TextMeshProUGUI>();
        cardNameTmp.text = "Cargando...";
        cardNameTmp.fontSize = 24;
        cardNameTmp.fontStyle = FontStyles.Bold;
        cardNameTmp.color = Color.white;
        cardNameTmp.alignment = TextAlignmentOptions.Center;
        RectTransform cardNameRect = cardNameGo.GetComponent<RectTransform>();
        cardNameRect.anchorMin = Vector2.zero;
        cardNameRect.anchorMax = Vector2.one;
        cardNameRect.sizeDelta = Vector2.zero;

        // B. Marco de arte para la foto del Pokémon
        GameObject artFrameGo = new GameObject("ArtFrame");
        artFrameGo.transform.SetParent(cardBgGo.transform, false);
        Image artFrameImg = artFrameGo.AddComponent<Image>();
        artFrameImg.color = new Color(0.04f, 0.06f, 0.10f, 1f);
        RectTransform artFrameRect = artFrameGo.GetComponent<RectTransform>();
        artFrameRect.anchorMin = new Vector2(0.05f, 0.32f);
        artFrameRect.anchorMax = new Vector2(0.95f, 0.84f);
        artFrameRect.sizeDelta = Vector2.zero;

        // Imagen del Sprite descargado de PokéAPI
        GameObject pokeImgGo = new GameObject("PokemonImage");
        pokeImgGo.transform.SetParent(artFrameGo.transform, false);
        Image pokeImg = pokeImgGo.AddComponent<Image>();
        pokeImg.preserveAspect = true;
        pokeImg.color = Color.white;
        RectTransform pokeRect = pokeImgGo.GetComponent<RectTransform>();
        pokeRect.anchorMin = Vector2.zero;
        pokeRect.anchorMax = Vector2.one;
        pokeRect.sizeDelta = new Vector2(-15f, -15f);

        // C. Barra de Tipo y Habilidad
        GameObject typeBoxGo = new GameObject("TypeBox");
        typeBoxGo.transform.SetParent(cardBgGo.transform, false);
        Image typeBoxImg = typeBoxGo.AddComponent<Image>();
        typeBoxImg.color = new Color(0.12f, 0.18f, 0.30f, 0.9f);
        RectTransform typeBoxRect = typeBoxGo.GetComponent<RectTransform>();
        typeBoxRect.anchorMin = new Vector2(0.05f, 0.18f);
        typeBoxRect.anchorMax = new Vector2(0.95f, 0.30f);
        typeBoxRect.sizeDelta = Vector2.zero;

        GameObject cardTypeGo = new GameObject("CardTypeText");
        cardTypeGo.transform.SetParent(typeBoxGo.transform, false);
        TextMeshProUGUI cardTypeTmp = cardTypeGo.AddComponent<TextMeshProUGUI>();
        cardTypeTmp.text = "Tipo: Cargando... | Hab: ...";
        cardTypeTmp.fontSize = 17;
        cardTypeTmp.fontStyle = FontStyles.Bold;
        cardTypeTmp.color = new Color(0.35f, 0.85f, 1f, 1f);
        cardTypeTmp.alignment = TextAlignmentOptions.Center;
        RectTransform cardTypeRect = cardTypeGo.GetComponent<RectTransform>();
        cardTypeRect.anchorMin = Vector2.zero;
        cardTypeRect.anchorMax = Vector2.one;
        cardTypeRect.sizeDelta = Vector2.zero;

        // D. Barra de Estadisticas (HP, ATK, DEF, SPD)
        GameObject statsBoxGo = new GameObject("StatsBox");
        statsBoxGo.transform.SetParent(cardBgGo.transform, false);
        Image statsBoxImg = statsBoxGo.AddComponent<Image>();
        statsBoxImg.color = new Color(0.10f, 0.14f, 0.24f, 0.9f);
        RectTransform statsBoxRect = statsBoxGo.GetComponent<RectTransform>();
        statsBoxRect.anchorMin = new Vector2(0.05f, 0.04f);
        statsBoxRect.anchorMax = new Vector2(0.95f, 0.16f);
        statsBoxRect.sizeDelta = Vector2.zero;

        GameObject cardStatsGo = new GameObject("CardStatsText");
        cardStatsGo.transform.SetParent(statsBoxGo.transform, false);
        TextMeshProUGUI cardStatsTmp = cardStatsGo.AddComponent<TextMeshProUGUI>();
        cardStatsTmp.text = "HP: --   ATK: --   DEF: --   SPD: --";
        cardStatsTmp.fontSize = 16;
        cardStatsTmp.fontStyle = FontStyles.Bold;
        cardStatsTmp.color = new Color(0.9f, 0.95f, 0.7f, 1f);
        cardStatsTmp.alignment = TextAlignmentOptions.Center;
        RectTransform cardStatsRect = cardStatsGo.GetComponent<RectTransform>();
        cardStatsRect.anchorMin = Vector2.zero;
        cardStatsRect.anchorMax = Vector2.one;
        cardStatsRect.sizeDelta = Vector2.zero;

        // 8. Panel de Botones de Control (Derecha)
        GameObject btnPanelGo = new GameObject("ButtonsPanel");
        btnPanelGo.transform.SetParent(canvasGo.transform, false);
        RectTransform btnPanelRect = btnPanelGo.AddComponent<RectTransform>();
        btnPanelRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnPanelRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnPanelRect.pivot = new Vector2(0.5f, 0.5f);
        btnPanelRect.anchoredPosition = new Vector2(250f, -30f);
        btnPanelRect.sizeDelta = new Vector2(260f, 440f);

        VerticalLayoutGroup vlg = btnPanelGo.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 16;
        vlg.childAlignment = TextAnchor.MiddleCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;

        Button prevBtn = CreateStyledButton(btnPanelGo.transform, "PrevButton", "< Anterior", new Color(0.18f, 0.38f, 0.82f));
        Button nextBtn = CreateStyledButton(btnPanelGo.transform, "NextButton", "Siguiente >", new Color(0.18f, 0.38f, 0.82f));
        Button p1Btn = CreateStyledButton(btnPanelGo.transform, "Player1Button", "Jugador 1 (José)", new Color(0.08f, 0.55f, 0.35f));
        Button p2Btn = CreateStyledButton(btnPanelGo.transform, "Player2Button", "Jugador 2 (Rival)", new Color(0.78f, 0.22f, 0.18f));

        // 9. Crear / Configurar GameManager
        GameObject gmGo = GameObject.Find("GameManager");
        if (gmGo == null)
        {
            gmGo = new GameObject("GameManager");
        }
        ApiClient client = gmGo.GetComponent<ApiClient>();
        if (client == null)
        {
            client = gmGo.AddComponent<ApiClient>();
        }

        // Asignar todas las referencias
        SerializedObject so = new SerializedObject(client);
        so.FindProperty("baseUrl").stringValue = "https://my-json-server.typicode.com/truji2506/Jose-Api-Cartas";
        so.FindProperty("cardImage").objectReferenceValue = pokeImg;
        so.FindProperty("cardName").objectReferenceValue = cardNameTmp;
        so.FindProperty("cardTypeText").objectReferenceValue = cardTypeTmp;
        so.FindProperty("cardStatsText").objectReferenceValue = cardStatsTmp;
        so.FindProperty("userNameText").objectReferenceValue = playerTmp;
        so.FindProperty("studentNameText").objectReferenceValue = studentTmp;
        so.FindProperty("prevButton").objectReferenceValue = prevBtn;
        so.FindProperty("nextButton").objectReferenceValue = nextBtn;
        so.FindProperty("player1Button").objectReferenceValue = p1Btn;
        so.FindProperty("player2Button").objectReferenceValue = p2Btn;
        so.ApplyModifiedProperties();

        // 10. Guardar la Escena en Assets/Scenes/MainScene.unity
        if (!Directory.Exists("Assets/Scenes"))
        {
            Directory.CreateDirectory("Assets/Scenes");
        }
        string scenePath = "Assets/Scenes/MainScene.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log("¡Escena con habilidades y estadísticas configurada y guardada en " + scenePath + "!");

        // Actualizar Build Settings
        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[]
        {
            new EditorBuildSettingsScene(scenePath, true)
        };
        EditorBuildSettings.scenes = scenes;
        Debug.Log("Build Settings actualizado.");
    }

    private static Button CreateStyledButton(Transform parent, string name, string label, Color color)
    {
        GameObject btnGo = new GameObject(name);
        btnGo.transform.SetParent(parent, false);

        Image btnImg = btnGo.AddComponent<Image>();
        btnImg.color = color;

        Button btn = btnGo.AddComponent<Button>();
        ColorBlock cb = btn.colors;
        cb.normalColor = color;
        cb.highlightedColor = color * 1.25f;
        cb.pressedColor = color * 0.75f;
        btn.colors = cb;

        LayoutElement le = btnGo.AddComponent<LayoutElement>();
        le.minHeight = 58;
        le.preferredHeight = 58;

        GameObject textGo = new GameObject("Text");
        textGo.transform.SetParent(btnGo.transform, false);
        TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 20;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;

        RectTransform textRect = textGo.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        return btn;
    }

    [MenuItem("Tools/2. Compilar WebGL a docs")]
    public static void BuildWebGLProject()
    {
        SetupScene();

        string buildPath = Path.Combine(Directory.GetCurrentDirectory(), "docs");
        Debug.Log("Iniciando compilacion WebGL hacia: " + buildPath);

        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.decompressionFallback = false;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MainScene.unity" },
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(options);
        Debug.Log("Resultado de compilacion WebGL: " + report.summary.result);
    }
}
#endif
