using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

#region Data Models (Fake API)

[System.Serializable]
public class ProjectInfo
{
    public string studentName;
    public string projectName;
}

[System.Serializable]
public class Player
{
    public int id;
    public string username;
    public int[] deck;
}

[System.Serializable]
public class PlayersWrapper
{
    public Player[] players;
}

#endregion

#region Data Models (PokeAPI)

[System.Serializable]
public class PokemonSprites
{
    public string front_default;
}

[System.Serializable]
public class PokemonTypeInfo
{
    public string name;
}

[System.Serializable]
public class PokemonTypeSlot
{
    public int slot;
    public PokemonTypeInfo type;
}

[System.Serializable]
public class PokemonStatInfo
{
    public string name;
}

[System.Serializable]
public class PokemonStatSlot
{
    public int base_stat;
    public PokemonStatInfo stat;
}

[System.Serializable]
public class PokemonAbilityInfo
{
    public string name;
}

[System.Serializable]
public class PokemonAbilitySlot
{
    public PokemonAbilityInfo ability;
}

[System.Serializable]
public class PokemonData
{
    public int id;
    public string name;
    public PokemonSprites sprites;
    public PokemonTypeSlot[] types;
    public PokemonStatSlot[] stats;
    public PokemonAbilitySlot[] abilities;
}

#endregion

public class ApiClient : MonoBehaviour
{
    [Header("Fake API (My JSON Server)")]
    [SerializeField]
    private string baseUrl =
        "https://my-json-server.typicode.com/truji2506/Jose-Api-Cartas";

    [Header("UI - Card")]
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardTypeText;
    [SerializeField] private TMP_Text cardStatsText;

    [Header("UI - Player")]
    [SerializeField] private TMP_Text userNameText;

    [Header("UI - Student")]
    [SerializeField] private TMP_Text studentNameText;

    [Header("UI - Buttons")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button player1Button;
    [SerializeField] private Button player2Button;

    private Player[] players;
    private Player currentPlayer;
    private int currentCardIndex = 0;

    private void Start()
    {
        HookButtons();
        StartCoroutine(GetProject());
        StartCoroutine(GetPlayers());
    }

    private void HookButtons()
    {
        if (prevButton != null) prevButton.onClick.AddListener(PrevCard);
        if (nextButton != null) nextButton.onClick.AddListener(NextCard);
        if (player1Button != null) player1Button.onClick.AddListener(() => SelectPlayerById(1));
        if (player2Button != null) player2Button.onClick.AddListener(() => SelectPlayerById(2));
    }

    private IEnumerator GetProject()
    {
        string url = $"{baseUrl}/project";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[GetProject] Error: {req.error} | URL: {url}");
                yield break;
            }

            ProjectInfo project = JsonUtility.FromJson<ProjectInfo>(req.downloadHandler.text);

            if (studentNameText != null)
                studentNameText.text = "Estudiante: " + project.studentName;

            Debug.Log($"Proyecto: {project.projectName}");
            Debug.Log($"Estudiante: {project.studentName}");
        }
    }

    private IEnumerator GetPlayers()
    {
        string url = $"{baseUrl}/players";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[GetPlayers] Error: {req.error} | URL: {url}");
                yield break;
            }

            string wrappedJson = "{ \"players\": " + req.downloadHandler.text + " }";
            PlayersWrapper wrapper = JsonUtility.FromJson<PlayersWrapper>(wrappedJson);

            if (wrapper == null || wrapper.players == null || wrapper.players.Length == 0)
            {
                Debug.LogError("[GetPlayers] No se pudo parsear players.");
                yield break;
            }

            players = wrapper.players;

            SelectPlayerById(1); // jugador 1 por defecto
        }
    }

    private void SelectPlayerById(int id)
    {
        if (players == null) return;

        foreach (var p in players)
        {
            if (p.id == id)
            {
                currentPlayer = p;
                currentCardIndex = 0;

                if (userNameText != null)
                    userNameText.text = currentPlayer.username;

                RefreshCurrentCard();
                return;
            }
        }
    }

    private void PrevCard()
    {
        if (!HasDeck()) return;

        currentCardIndex--;
        if (currentCardIndex < 0)
            currentCardIndex = currentPlayer.deck.Length - 1;

        RefreshCurrentCard();
    }

    private void NextCard()
    {
        if (!HasDeck()) return;

        currentCardIndex++;
        if (currentCardIndex >= currentPlayer.deck.Length)
            currentCardIndex = 0;

        RefreshCurrentCard();
    }

    private bool HasDeck()
    {
        return currentPlayer != null && currentPlayer.deck != null && currentPlayer.deck.Length > 0;
    }

    private void RefreshCurrentCard()
    {
        int pokemonId = currentPlayer.deck[currentCardIndex];
        StartCoroutine(GetPokemonById(pokemonId));
    }

    private IEnumerator GetPokemonById(int pokemonId)
    {
        string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonId}";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[GetPokemonById] Error: {req.error} | URL: {url}");
                yield break;
            }

            PokemonData pokemon = JsonUtility.FromJson<PokemonData>(req.downloadHandler.text);

            if (cardName != null)
            {
                string formattedName = string.IsNullOrEmpty(pokemon.name) 
                    ? "" 
                    : char.ToUpper(pokemon.name[0]) + pokemon.name.Substring(1);
                cardName.text = formattedName;
            }

            // Mostrar Tipo y Habilidad
            if (cardTypeText != null)
            {
                string typeStr = (pokemon.types != null && pokemon.types.Length > 0 && pokemon.types[0].type != null) 
                    ? char.ToUpper(pokemon.types[0].type.name[0]) + pokemon.types[0].type.name.Substring(1) 
                    : "Normal";
                string abilityStr = (pokemon.abilities != null && pokemon.abilities.Length > 0 && pokemon.abilities[0].ability != null)
                    ? char.ToUpper(pokemon.abilities[0].ability.name[0]) + pokemon.abilities[0].ability.name.Substring(1)
                    : "Ninguna";
                cardTypeText.text = $"Tipo: {typeStr}  |  Hab: {abilityStr}";
            }

            // Mostrar Estadisticas base (HP, ATK, DEF, SPD)
            if (cardStatsText != null && pokemon.stats != null && pokemon.stats.Length >= 3)
            {
                int hp = pokemon.stats.Length > 0 ? pokemon.stats[0].base_stat : 50;
                int atk = pokemon.stats.Length > 1 ? pokemon.stats[1].base_stat : 50;
                int def = pokemon.stats.Length > 2 ? pokemon.stats[2].base_stat : 50;
                int spd = pokemon.stats.Length > 5 ? pokemon.stats[5].base_stat : 50;
                cardStatsText.text = $"HP: {hp}   ATK: {atk}   DEF: {def}   SPD: {spd}";
            }

            if (cardImage != null)
                StartCoroutine(DownloadSprite(pokemon.sprites.front_default));
        }
    }

    private IEnumerator DownloadSprite(string imageUrl)
    {
        if (string.IsNullOrEmpty(imageUrl)) yield break;

        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[DownloadSprite] Error: {req.error} | URL: {imageUrl}");
                yield break;
            }

            Texture2D tex = DownloadHandlerTexture.GetContent(req);
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );

            cardImage.sprite = sprite;
            cardImage.preserveAspect = true;
        }
    }
}
