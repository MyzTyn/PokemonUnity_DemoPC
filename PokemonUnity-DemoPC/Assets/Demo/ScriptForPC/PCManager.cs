using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using System.Collections.Generic;
using UnityEngine;

public class PCManager : MonoBehaviour
{
    public PCDataUI DataUI { get { return GetComponent<PCDataUI>(); } }
    //
    public static Sprite[] PokemonSprite;
    public static Sprite[] PokemonType;
    public Dictionary<int, PCButton> PCButtonData { get; private set; }
    //
    public static Player player { get; private set; }
    private void Awake()
    {
        Debug.Log("Awaking");
        Debug.Log("Init load all two Resources");
        PokemonSprite = Resources.LoadAll<Sprite>("PokemonIcon");
        PokemonType = Resources.LoadAll<Sprite>("PokemonType");
        Debug.Log("Init PCButtonData Dictionary");
        PCButtonData = new Dictionary<int, PCButton>();
        Debug.Log("Init new Player()");
        player = new Player();
        Debug.Log("End Awake()");
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting");
        Debug.Log("Add pokemon to Party");
        player.Party[0] = new Pokemon(Pokemons.BULBASAUR, 100, false);
        Debug.Log("Random Add Pokemon to PC Scene");
        RandomAddPokemon();
        Debug.Log("Display Stats");
        DataUI.DisplayPokemonStatsUI(player.Party[0]);
        Debug.Log("Display Party Count");
        DataUI.UpdatePartyUINum();
        Debug.Log("Set Default Active Box to 0");
        Debug.Log(player.PC[0]);
        Debug.Log("Init PCButton");
        DataUI.InitPCBoxButton();
        Debug.Log("Update Box Text");
        DataUI.BoxNum.text = string.Format("Box: {0}", player.PC.ActiveBox + 1); //Because I don't want to show 0
        Debug.Log("End Start()");
    }
    private void RandomAddPokemon()
    {
        Debug.Log("InitRandomPokemon");
        for (int i = 0; i <= 100; i++)
        {
            int P = Random.Range(1, 151);
            player.PC.addPokemon(new Pokemon((Pokemons)P, 100, false));
        }
        Debug.Log("End of Random");
    }
    public void ReleasePC(int ID, bool PartyOrPC)
    {
        Debug.Log("InitRemovePokemon");
        Debug.Log("ID: " + ID);
        if (PartyOrPC)
        {
            Debug.Log(player.PC.removePokemon(player.PC.ActiveBox, ID));
            Debug.Log("Removed PC");
        }
        else
        {
            player.Party[ID] = new Pokemon();
            Debug.Log("Removed Party");
        }
        Debug.Log("End of Release");
    }
    public static int PokemonSpriteNum(int ID)
    {
        return (int)player.PC.Pokemons[ID].Species;
    }
}
