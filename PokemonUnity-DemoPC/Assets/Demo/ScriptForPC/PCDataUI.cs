using PokemonUnity;
using PokemonUnity.Monster;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCDataUI : MonoBehaviour
{
    private PCManager Manager { get { return GetComponent<PCManager>(); } }
    [SerializeField]
    public GameObject test { get; private set; }
    //PC Button
    #region PC Box
    public GameObject ButtonTemplate;
    private List<int> ID;
    public GridLayoutGroup GridLayout;
    #endregion
    [Space(5)]
    #region Pokemon Stats
    #region Name/Gender
    public Text PkmnName = null;
    public Text Gender = null;
    #endregion
    public Image PokemonSprite;
    [Space(5)]
    #region BackgroundOfStats
    public Text Level = null;
    public Image Type1 = null;
    public Image Type2 = null;
    public Text AbilityName = null;
    public Text ItemName = null;
    #endregion
    [Space(5)]
    #endregion
    #region BoxUI
    public Text BoxNum = null;
    #endregion
    [Space(5)]
    #region PC Menu
    public Button PartyButton = null;
    public Text PartyNum = null;
    public Button Exit = null;
    #endregion
    [Space(5)]
    #region MiniWindow/Mouse
    public PokemonMouse mouseUI1;
    public GameObject PokemonStats;
    public MiniWindowData windowData;
    #endregion
    public void DisplayPokemonStatsUI(Pokemon pokemon)
    {
        Debug.Log("InitDisplayPokemonStats");
        if (pokemon.IsNotNullOrNone())
        {
            PkmnName.text = pokemon.Name;
            //Use sprite?
            if (Convert.ToBoolean(pokemon.Gender))
            {
                Gender.text = "♂";
                Gender.color = UnityEngine.Color.blue;
            }
            else
            {
                Gender.text = "♀";
                Gender.color = UnityEngine.Color.magenta;
            }
            PokemonSprite.sprite = PCManager.PokemonSprite[(int)pokemon.Species];
            Level.text = "Lv." + pokemon.Level;
            Type1.sprite = PCManager.PokemonType[(int)pokemon.Type1];
            Type2.sprite = PCManager.PokemonType[(int)pokemon.Type2];
            AbilityName.text = pokemon.Ability.ToString().Replace("_", " ");
            ItemName.text = pokemon.Item.ToString();
        }
        Debug.Log("End of Display Pokemon Stats");
    }
    public void ClearPokemonStatsUI()
    {
        PkmnName.text = null;
        //Use sprite?
        Gender.text = null;
        PokemonSprite.sprite = null;
        Level.text = null;
        Type1.sprite = null;
        Type2.sprite = null;
        AbilityName.text = null;
        ItemName.text = null;
    }
    //Init Button for PC BOX
    public void InitPCBoxButton()
    {
        ID = new List<int>();
        for (int i = 0; i < 30; i++)
        {
            ID.Add(i);
        }
        GetPCButton();
        windowData.InitAddPartyButton();
    }
    private void GetPCButton()
    {
        if (ID.Count <= 6)
        {
            GridLayout.constraintCount = ID.Count;
        }
        else
        {
            GridLayout.constraintCount = 6;
        }
        foreach (int Id in ID)
        {
            GameObject ButtonClone = Instantiate(ButtonTemplate);
            ButtonClone.AddComponent<PCButton>();
            ButtonClone.GetComponent<PCButton>().SetID(Id, windowData);
            ButtonClone.GetComponent<PCButton>().UpdatePCUI();
            Manager.PCButtonData.Add(Id, ButtonClone.GetComponent<PCButton>());
            ButtonClone.SetActive(true);
            ButtonClone.transform.SetParent(ButtonTemplate.transform.parent, false);
        }
    }
    public void UpdatePCButtonDisplay()
    {
        for (int i = 0; i < 30; i++)
        {
            if (mouseUI1.HasPokemonOnYourMouse && mouseUI1.MouseBoxID == PCManager.player.PC.ActiveBox && i == mouseUI1.MouseBoxPkmnID && !mouseUI1.IsPartyMouse)
            {
                Manager.PCButtonData[i].DisablePCUI();
            }
            else
            {
                Manager.PCButtonData[i].UpdatePCUI();
            }
        }
    }
    //End of Button
    //MiniWindow
    
    //Pokemon Selected MENU
    public void Move_Place_Pkmn(int ID)
    {
        
        if (mouseUI1.HasPokemonOnYourMouse)
        {
            if (windowData.PartyOrPC && !mouseUI1.IsPartyMouse) //true(PC) and False(PC)
            {
                //PC swap
                Debug.Log("From Mouse(False) to PC");
                PCManager.player.PC.swapPokemon(mouseUI1.MouseBoxID, mouseUI1.MouseBoxPkmnID, PCManager.player.PC.ActiveBox, ID);
            }
            else if (!windowData.PartyOrPC && !mouseUI1.IsPartyMouse) //false(Party) and false(PC)
            {
                //Party and PC Swap
                Debug.Log("From mouse(False) to Party");
                PCManager.player.PC.Switch_PC_And_Party_Pokemon(PCManager.player, ID, mouseUI1.MouseBoxPkmnID);
            }
            else if (windowData.PartyOrPC && mouseUI1.IsPartyMouse) //true(PC) and true(Party)
            {
                Debug.Log("From mouse(true) to PC");
                PCManager.player.PC.Switch_PC_And_Party_Pokemon(PCManager.player, mouseUI1.PartyID, ID);
            }
            else
            {
                Debug.Log("Party to Party");
                Pokemon temp = PCManager.player.Party[ID];
                PCManager.player.Party[ID] = PCManager.player.Party[mouseUI1.PartyID];
                PCManager.player.Party[mouseUI1.PartyID] = temp;
            }
            mouseUI1.BoolPokemonMouse(false);
            mouseUI1.InitSprite(1);
            UpdatePCButtonDisplay();
            windowData.UpdatePartyUI(1);
        }
        else
        {
            if (windowData.PartyOrPC)
            {
                mouseUI1.SetToMouse(PCManager.player.PC.ActiveBox, ID);
                mouseUI1.BoolPartyMouse(false);
                mouseUI1.InitSprite(3);
            }
            else
            {
                //Add to Party
                mouseUI1.PartyID = ID;
                mouseUI1.BoolPartyMouse(true);
                mouseUI1.InitSprite(2);
            }
            mouseUI1.BoolPokemonMouse(true);
            
            UpdatePCButtonDisplay();
        }
    }
    public void Release_Pkmn(int ID)
    {
        //You should add confirm Release UI here
        Manager.ReleasePC(ID, windowData.PartyOrPC);
        //Change to int mode
        ClearPokemonStatsUI();
        UpdatePCButtonDisplay();
        windowData.UpdatePartyUI(1);
        mouseUI1.BoolPokemonMouse(false);
        mouseUI1.InitSprite(0);
        UpdatePartyUINum();
    }
    public void WithdrawToParty(int ID)
    {
        Debug.Log("Widthdrawing");
        PCManager.player.addPokemon(PCManager.player.PC.Pokemons[ID]);
        Debug.Log("Added to Pokemon");
        PCManager.player.PC.removePokemon(PCManager.player.PC.ActiveBox, ID);
        Debug.Log("Removed PC Pokemon");
        Debug.Log("Add to Party(Pkmn Name: "+PCManager.player.Party[1].Name);
        UpdatePCButtonDisplay();
        Debug.Log("Updated PC Button");
        windowData.UpdatePartyUI(1);
        Debug.Log("Updated Party UI (1)");
        mouseUI1.BoolPokemonMouse(false);
        Debug.Log("Disabled PokemonMouse");
        PartyNum.text = "Party: " + PCManager.player.Party.GetCount();
        Debug.Log("Updated PartyNum Text and Method is Done!");
    }
    //End of Pokemon Selected Menu
    public void ExitOnClick()
    {
        Debug.Log("ExitPCScene");
        throw new NotImplementedException();
    }
    public void UpdatePartyUINum()
    {
        PartyNum.text = "Party: " + PCManager.player.Party.GetCount();
    }
    //End of MiniWindow
    
    private void Update()
    {
        if (mouseUI1.HasPokemonOnYourMouse)
        {
            Vector3 MousePostion = UnityEngine.Input.mousePosition;
            MousePostion.z = 65;
            mouseUI1.FeedToMousePosition(Camera.main.ScreenToWorldPoint(MousePostion));
            if (UnityEngine.Input.GetMouseButtonDown(1))
            {
                mouseUI1.BoolPokemonMouse(false);
                UpdatePCButtonDisplay();
            }
        }
    }
}
