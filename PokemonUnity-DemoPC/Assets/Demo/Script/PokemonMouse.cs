using UnityEngine;
using UnityEngine.UI;
public class PokemonMouse : MonoBehaviour
{
    private Image Mouse_Pkmn_Sprite { get { return GetComponent<Image>(); } }
    private RectTransform MouseTranform { get { return GetComponent<RectTransform>(); } }
    public bool HasPokemonOnMouse { get; private set; }
    /// <summary>
    /// Check which one Party or PC
    /// </summary>
    public bool IsPC_Or_PartyOnMouse { get; private set; }
    public int Party_ID;
    public int PC_BoxID { get; private set; }
    public int PC_BoxPkmnID { get; private set; }
    /// <summary>
    /// For Mode, 0 is Has Pokemon On Your Mouse and 1 is Is PC or Party On Mouse
    /// </summary>
    /// <param name="Mode"></param>
    /// <param name="BoolForMouse"></param>
    public void SetBoolToMouse(int Mode,bool BoolForMouse)
    {
        switch(Mode)
        {
            case 0:
                HasPokemonOnMouse = BoolForMouse;
                break;
            case 1:
                IsPC_Or_PartyOnMouse = BoolForMouse;
                break;
            default:
                Debug.Log("Invalid Mode");
                break;
        }
    }
    /// <summary>
    /// For PC ID
    /// </summary>
    /// <param name="BoxID"></param>
    /// <param name="BoxPkmnID"></param>
    public void SetToMouse(int BoxID, int BoxPkmnID)
    {
        PC_BoxID = BoxID;
        PC_BoxPkmnID = BoxPkmnID;
    }
    /// <summary>
    /// For Party ID
    /// </summary>
    /// <param name="PartyID"></param>
    public void SetToMouse(int PartyID)
    {
        Party_ID = PartyID;
    }
    /// <summary>
    /// Mode = 1 is clear, 2 is Party Sprite, 3 is PC Sprite
    /// </summary>
    /// <param name="mode"></param>
    public void InitSprite(int mode)
    {
        switch (mode)
        {
            case 1:
                Mouse_Pkmn_Sprite.color = Color.clear;
                Mouse_Pkmn_Sprite.sprite = null;
                break;
            case 2:
                Mouse_Pkmn_Sprite.color = Color.white;
                Mouse_Pkmn_Sprite.sprite = PCManager.PokemonSprite[(int)PCManager.DemoPlayer.Party[Party_ID].Species];
                break;
            case 3:
                Mouse_Pkmn_Sprite.color = Color.white;
                Mouse_Pkmn_Sprite.sprite = PCManager.PokemonSprite[(int)PCManager.DemoPlayer.PC[System.Convert.ToByte(PC_BoxID)].Pokemons[PC_BoxPkmnID].Species];
                break;
        }
    }
    private void Update()
    {
        if (HasPokemonOnMouse)
        {
            Vector3 MousePostion = Input.mousePosition;
            MousePostion.z = 65;
            MouseTranform.position = Camera.main.ScreenToWorldPoint(MousePostion);
        }
    }
}
