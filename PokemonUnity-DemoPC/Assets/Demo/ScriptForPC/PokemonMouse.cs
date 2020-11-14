using System;
using UnityEngine;
using UnityEngine.UI;

public class PokemonMouse : MonoBehaviour
{
    private Image PokemonSprite { get { return GetComponent<Image>(); } }
    private RectTransform MouseTranform { get { return GetComponent<RectTransform>(); } }
    public bool HasPokemonOnYourMouse { get; private set; }

    public int PartyID;
    public bool IsPartyMouse { get; private set; }
    public int MouseBoxID { get; private set; }
    public int MouseBoxPkmnID { get; private set; }
    public void BoolPokemonMouse(bool active)
    {
        HasPokemonOnYourMouse = active;
    }
    public void BoolPartyMouse(bool active)
    {
        IsPartyMouse = active;
    }
    public void InitSprite(int mode)
    {
        
        switch (mode)
        {
            case 1:
                PokemonSprite.color = Color.clear;
                PokemonSprite.sprite = null;
                break;
            case 2:
                PokemonSprite.color = Color.white;
                PokemonSprite.sprite = PCManager.PokemonSprite[(int)PCManager.player.Party[PartyID].Species];
                break;
            case 3:
                PokemonSprite.color = Color.white;
                PokemonSprite.sprite = PCManager.PokemonSprite[(int)PCManager.player.PC[Convert.ToByte(MouseBoxID)].Pokemons[MouseBoxPkmnID].Species];
                break;
            default:
                break;
        }
    }
    public void SetToMouse(int BoxID, int PkmnID)
    {
        MouseBoxID = BoxID;
        MouseBoxPkmnID = PkmnID;
        HasPokemonOnYourMouse = true;
    }
    /// <summary>
    /// Use World Position
    /// </summary>
    /// <param name="vector3"></param>
    public void FeedToMousePosition(Vector3 vector3)
    {
        MouseTranform.position = vector3;
    }
}
