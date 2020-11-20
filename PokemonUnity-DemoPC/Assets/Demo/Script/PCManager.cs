using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using System.Collections.Generic;
using UnityEngine;

public class PCManager : MonoBehaviour
{
    //
    public PCGameObjectAndData ObjectData { get { return GetComponent<PCGameObjectAndData>(); } }
    //
    public static Sprite[] PokemonSprite;
    public static Sprite[] PokemonType;
    //
    public static Player DemoPlayer;
    //
    public Dictionary<int, PCButton> PCButtonScriptData { get; private set; }
    //
    private void Awake()
    {
        Debug.Log("Awaking");
        Debug.Log("Init load all two Resources");
        PokemonSprite = Resources.LoadAll<Sprite>("PokemonIcon");
        PokemonType = Resources.LoadAll<Sprite>("PokemonType");
        Debug.Log("Init New Dictionary");
        PCButtonScriptData = new Dictionary<int, PCButton>();
        Debug.Log("Init new Player()");
        DemoPlayer = new Player();
        Debug.Log("End Awake()");
    }
    void Start()
    {
        Debug.Log("Starting");
        Debug.Log("Add pokemon to Party");
        DemoPlayer.Party[0] = new Pokemon(Pokemons.BULBASAUR, 100, false);
        Debug.Log("Random Add Pokemon to PC Scene");
        RandomAddPokemon();
        Debug.Log("Display Stats");
        ObjectData.DisplayPokemonStatsUI(DemoPlayer.Party[0]);
        Debug.Log("Display Party Count");
        ObjectData.UpdatePartyUINum();
        Debug.Log("Set Default Active Box to 0");
        Debug.Log(DemoPlayer.PC[0]);
        Debug.Log("Init PCButton");
        ObjectData.InitPCBoxButton();
        Debug.Log("Init PartyButton");
        ObjectData.InitAddPartyScript();
        Debug.Log("Update Box Text");
        ObjectData.UpdatePCBoxText();
        Debug.Log("Update PCBox");
        ObjectData.UpdatePCButtonUI_Info();
        Debug.Log("End Start()");
    }
    private void RandomAddPokemon()
    {
        Debug.Log("InitRandomPokemon");
        for (int NumberLimit = 0; NumberLimit <= 100; NumberLimit++) //Will go looping till 100
        {
            int PokemonRandom = Random.Range(1, 151); //Roll dice from 1 to 151
            DemoPlayer.PC.addPokemon(new Pokemon((Pokemons)PokemonRandom, 100, false)); //Add Pokemon
        }
        Debug.Log("End of Random");
    }
    /// <summary>
    /// Mode = 1 is PC for Sprite num, 2 is Party for Sprite Num. This is just shortcut.
    /// </summary>
    /// <param name="Mode"></param>
    /// <param name="ID"></param>
    /// <returns></returns>
    public static int PokemonSpriteNum(int Mode, int ID)
    {
        switch (Mode)
        {
            case 1:
                return (int)DemoPlayer.PC.Pokemons[ID].Species;
            case 2:
                return (int)DemoPlayer.Party[ID].Species;
            default:
                return 0;
        }
    }
}
