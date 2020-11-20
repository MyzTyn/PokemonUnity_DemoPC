using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PokemonUnity.Monster;
using PokemonUnity;
using System;

public class PCGameObjectAndData : MonoBehaviour
{
    private PCManager Manager { get { return GetComponent<PCManager>(); } }
    public int PartyButtonbehavior { get; private set; }
    public bool PartyOrPC { get; private set; }
    public int ButtonIDHolder { get; private set; }
    public PokemonMouse PkmnMouse;
    #region SerializeField, Gameoject (UI Releted)
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
    public GameObject PokemonStats;
    public Text PCMenu_Move_Place_Text;
    public GameObject PokemonMenuClicked;
    public GameObject MiniPartyUI;
    public Image[] MiniPartyImage;
    #endregion
    #endregion
    #region UI Script
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
    public void UpdatePartyUINum()
    {
        PartyNum.text = "Party: " + PCManager.DemoPlayer.Party.GetCount();
    }
    public void UpdatePCBoxText()
    {
        BoxNum.text = string.Format("Box: {0}", PCManager.DemoPlayer.PC.ActiveBox + 1);
    }
    #region Button Script Related
    public void InitPCBoxButton()
    {
        //Create new List and Add info to list
        ID = new List<int>();
        for (int i = 0; i < 30; i++)
        {
            ID.Add(i);
        }
        //SetButtonInfo and Grid Setting
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
            GameObject ButtonClone = UnityEngine.Object.Instantiate(ButtonTemplate);
            ButtonClone.AddComponent<PCButton>();
            ButtonClone.GetComponent<PCButton>().SetPCButtonData(Id, this);
            Manager.PCButtonScriptData.Add(Id, ButtonClone.GetComponent<PCButton>());
            ButtonClone.SetActive(true);
            ButtonClone.transform.SetParent(ButtonTemplate.transform.parent, false);
        }
    }
    public void UpdatePCButtonUI_Info()
    {
        for (int i = 0; i < 30; i++)
        {
            if (PkmnMouse.HasPokemonOnMouse && PkmnMouse.PC_BoxID == PCManager.DemoPlayer.PC.ActiveBox && i == PkmnMouse.PC_BoxPkmnID && !PkmnMouse.IsPC_Or_PartyOnMouse)
            {
                Manager.PCButtonScriptData[i].DisablePCButonUI();
            }
            else
            {
                Manager.PCButtonScriptData[i].UpdatePCButtonUI();
            }
        }
    }
    public void InitAddPartyScript()
    {
        for (int i = 0; i < PCManager.DemoPlayer.Party.Length; i++)
        {
            //Add script to Party
            MiniPartyImage[i].gameObject.AddComponent<MiniPartyButton>();
            MiniPartyImage[i].gameObject.GetComponent<MiniPartyButton>().SetData(i, this);
        }
    } //To DO: Need add script  
    /// <summary>
    /// Mode = 1 is Party ID, Mode = 2 is PC ID
    /// </summary>
    /// <param name="Mode"></param>
    /// <param name="ButtonID"></param>
    public void PassIDToManager(int Mode, int ButtonID)
    {
        switch (Mode)
        {
            case 1: // Party
                if (!PCManager.DemoPlayer.Party[ButtonID].IsNotNullOrNone() && !PkmnMouse.HasPokemonOnMouse) { }
                else
                {
                    PartyOrPC = false;
                    PartyButtonbehavior = 0;
                    Debug.Log(ButtonID);
                    if (PkmnMouse.HasPokemonOnMouse)
                    {
                        PCMenu_Move_Place_Text.text = "Place";
                        PartyButtonbehavior = 1;
                        ActiveGameobjectMode(1);
                    }
                    else
                    {
                        PCMenu_Move_Place_Text.text = "Move";
                        PartyButtonbehavior = 2;
                        ActiveGameobjectMode(0);
                        ActiveGameobjectMode(2); //Disable Widthdraw
                    }
                    PokemonMenuClicked.SetActive(true);
                    ButtonIDHolder = ButtonID;
                }
                break;
            case 2: // PC
                if (!PCManager.DemoPlayer.PC.Pokemons[ButtonID].IsNotNullOrNone() && !PkmnMouse.HasPokemonOnMouse) { }
                else
                {
                    PartyOrPC = true;
                    if (PkmnMouse.HasPokemonOnMouse)
                    {
                        PCMenu_Move_Place_Text.text = "Place";
                        ActiveGameobjectMode(1);
                        PartyButtonbehavior = 0;
                    }
                    else if (PCManager.DemoPlayer.Party.GetCount() >= PCManager.DemoPlayer.Party.Length)
                    {
                        Debug.Log("Full");
                        ActiveGameobjectMode(2);
                    }
                    else
                    {
                        PCMenu_Move_Place_Text.text = "Move";
                        PkmnMouse.SetBoolToMouse(1, false);
                        ActiveGameobjectMode(0);
                    }
                    PokemonMenuClicked.SetActive(true);
                    ButtonIDHolder = ButtonID;
                    DisplayPokemonStatsUI(PCManager.DemoPlayer.PC.Pokemons[ButtonID]);
                }
                break;
        }
    }
    public void PokemonMenu(int button_ID)
    {
        switch (button_ID)
        {
            case 1: // Move/Place Button
                if (PartyButtonbehavior == 1 && PkmnMouse.HasPokemonOnMouse)
                {
                    Debug.Log("Mouse should able to place it");
                    PartyOrPC = false;
                }
                else if (PartyButtonbehavior == 2)
                {
                    Debug.Log("Party should able to move");
                    PkmnMouse.SetBoolToMouse(1, true);
                }
                Move_Place_Pkmn();
                UpdatePartyUI(1);
                UpdatePCButtonUI_Info();
                PokemonMenuClicked.SetActive(false);
                break;
            case 2: //Summary Button
                throw new NotImplementedException();
            case 3: //Withdraw Button
                WithdrawToParty();
                UpdatePCButtonUI_Info();
                PokemonMenuClicked.SetActive(false);
                break;
            case 4: //Item Button
                throw new NotImplementedException();
            case 5: // Mark Button
                throw new NotImplementedException();
            case 6: //Release Button
                ReleasePokemon();
                PokemonMenuClicked.SetActive(false);
                break;
            case 7: //Debug Button
                throw new NotImplementedException();
            case 8: //Cancel Button
                PkmnMouse.InitSprite(1);
                PkmnMouse.SetBoolToMouse(0, false);
                UpdatePCButtonUI_Info();
                UpdatePartyUI(1);
                PokemonMenuClicked.SetActive(false);
                break;
        }
        Debug.Log("Switch (button_ID) Done");
    }
    /// <summary>
    /// Mode = 0 is Disable Party Image, and Mode = 1 is Enabled and disable if there no Pokemon in Party
    /// </summary>
    /// <param name="mode"></param>
    public void UpdatePartyUI(int mode)
    {
        switch (mode)
        {
            case 1:
                for (int i = 0; i < PCManager.DemoPlayer.Party.Length; i++)
                {

                    if (!PCManager.DemoPlayer.Party[i].IsNotNullOrNone())
                    {
                        MiniPartyImage[i].color = UnityEngine.Color.clear;
                    }
                    else
                    {
                        if (PkmnMouse.HasPokemonOnMouse && PkmnMouse.IsPC_Or_PartyOnMouse && i == PkmnMouse.Party_ID)
                        {
                            MiniPartyImage[i].color = UnityEngine.Color.clear;
                            Debug.Log(i + "Is Disabled");
                        }
                        else
                        {
                            MiniPartyImage[i].color = UnityEngine.Color.white;
                            MiniPartyImage[i].sprite = PCManager.PokemonSprite[(int)PCManager.DemoPlayer.Party[i].Species];
                        }
                    }
                }
                break;
            case 0:
                for (int i = 0; i < PCManager.DemoPlayer.Party.Length; i++)
                {
                    MiniPartyImage[i].color = UnityEngine.Color.clear;
                    MiniPartyImage[i].sprite = null;
                }
                break;
        }
    }
    public void ExitOnClick()
    {
        Debug.Log("ExitPCScene");
        throw new NotImplementedException();
    }
    public void OnPartyClick()
    {
        Debug.Log("InitPartyClicked");
        if (!MiniPartyUI.activeSelf)
        {
            Debug.Log("Party is Active");
            UpdatePartyUI(1);
            MiniPartyUI.SetActive(true);
        }
        else
        {
            Debug.Log("Party is off");
            UpdatePartyUI(0);
            MiniPartyUI.SetActive(false);
        }
    }
    #endregion
    #endregion
    public void Move_Place_Pkmn()
    {

        if (PkmnMouse.HasPokemonOnMouse)
        {
            if (PartyOrPC && !PkmnMouse.IsPC_Or_PartyOnMouse) //true(PC) and False(PC)
            {
                //PC swap
                Debug.Log("From Mouse(False) to PC");
                PCManager.DemoPlayer.PC.swapPokemon(PkmnMouse.PC_BoxID, PkmnMouse.PC_BoxPkmnID, PCManager.DemoPlayer.PC.ActiveBox, ButtonIDHolder);
            }
            else if (!PartyOrPC && !PkmnMouse.IsPC_Or_PartyOnMouse) //false(Party) and false(PC)
            {
                //Party and PC Swap
                Debug.Log("From mouse(False) to Party");
                PCManager.DemoPlayer.PC.Switch_PC_And_Party_Pokemon(PCManager.DemoPlayer, ButtonIDHolder, PkmnMouse.PC_BoxPkmnID);
            }
            else if (PartyOrPC && PkmnMouse.IsPC_Or_PartyOnMouse) //true(PC) and true(Party)
            {
                Debug.Log("From mouse(true) to PC");
                PCManager.DemoPlayer.PC.Switch_PC_And_Party_Pokemon(PCManager.DemoPlayer, PkmnMouse.Party_ID, ButtonIDHolder);
            }
            else
            {
                Debug.Log("Party to Party");
                Pokemon temp = PCManager.DemoPlayer.Party[ButtonIDHolder];
                PCManager.DemoPlayer.Party[ButtonIDHolder] = PCManager.DemoPlayer.Party[PkmnMouse.Party_ID];
                PCManager.DemoPlayer.Party[PkmnMouse.Party_ID] = temp;
            }
            PkmnMouse.SetBoolToMouse(0, false);
            PkmnMouse.InitSprite(1);
        }
        else
        {
            if (PartyOrPC)
            {
                //Add PC to Mouse
                PkmnMouse.SetToMouse(PCManager.DemoPlayer.PC.ActiveBox, ButtonIDHolder);
                PkmnMouse.SetBoolToMouse(1, false);
                PkmnMouse.InitSprite(3);
            }
            else
            {
                //Add Party to Mouse
                PkmnMouse.Party_ID = ButtonIDHolder;
                PkmnMouse.SetBoolToMouse(1, true);
                PkmnMouse.InitSprite(2);
            }
            PkmnMouse.SetBoolToMouse(0, true);
        }
    }
    public void WithdrawToParty()
    {
        Debug.Log("Widthdrawing");
        PCManager.DemoPlayer.addPokemon(PCManager.DemoPlayer.PC.Pokemons[ButtonIDHolder]);
        Debug.Log("Added to Pokemon");
        PCManager.DemoPlayer.PC.removePokemon(PCManager.DemoPlayer.PC.ActiveBox, ButtonIDHolder);
        Debug.Log("Removed PC Pokemon");
        Debug.Log("Add to Party(Pkmn Name: " + PCManager.DemoPlayer.Party[1].Name);
        UpdatePartyUI(1);
        Debug.Log("Updated Party UI (1)");
        PkmnMouse.SetBoolToMouse(0, false);
        Debug.Log("Disabled PokemonMouse");
        UpdatePartyUINum();
        Debug.Log("Updated PartyNum Text and Method is Done!");
    }
    public void ReleasePokemon()
    {
        Debug.Log("InitRemovePokemon");
        Debug.Log("Is PC: "+ PartyOrPC +" ID: " + ButtonIDHolder);
        if (PartyOrPC)
        {
            Debug.Log(PCManager.DemoPlayer.PC.removePokemon(PCManager.DemoPlayer.PC.ActiveBox, ButtonIDHolder));
            Debug.Log("Removed PC");
            UpdatePCButtonUI_Info();
        }
        else
        {
            PCManager.DemoPlayer.Party[ButtonIDHolder] = new Pokemon();
            UpdatePartyUI(1);
            UpdatePartyUINum();
            Debug.Log("Removed Party");
        }
        Debug.Log("End of Release");
    }
    public void BoxNavigateArrow(int arrowvalue)
    {
        Debug.Log("Box Navigate Clicked and value is: "+ arrowvalue);
        byte PCShouldBeThis;
        int TotalValue;
        switch (arrowvalue)
        {
            case 1:

                if (Core.STORAGEBOXES > PCManager.DemoPlayer.PC.ActiveBox + arrowvalue)
                {
                    TotalValue = PCManager.DemoPlayer.PC.ActiveBox + arrowvalue;
                    PCShouldBeThis = Convert.ToByte(TotalValue);
                    Debug.Log("PC is Switching. " + PCManager.DemoPlayer.PC[PCShouldBeThis] + " Active Box is: " + PCManager.DemoPlayer.PC.ActiveBox);
                    Debug.Log("Move to next box!");
                    UpdatePCButtonUI_Info();
                    UpdatePCBoxText();
                }
                else
                {
                    Debug.Log("You can't move next box!");
                }
                break;
            case -1:
                if (PCManager.DemoPlayer.PC.ActiveBox + arrowvalue >= 0)
                {
                    TotalValue = PCManager.DemoPlayer.PC.ActiveBox + arrowvalue;
                    PCShouldBeThis = Convert.ToByte(TotalValue);
                    Debug.Log("PC is Switching. " + PCManager.DemoPlayer.PC[PCShouldBeThis] + " Active Box is: " + PCManager.DemoPlayer.PC.ActiveBox);
                    Debug.Log("You can move back box!");
                    UpdatePCButtonUI_Info();
                    UpdatePCBoxText();
                }
                else
                {
                    Debug.Log("You can't move back box!");
                }
                break;
            default:
                Debug.Log("Nothing Happen");
                break;
        }
    }
    public void ActiveGameobjectMode(int Mode)
    {
        switch (Mode)
        {
            case 0:
                PokemonMenuClicked.transform.GetChild(2).gameObject.SetActive(true);
                PokemonMenuClicked.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 1:
                PokemonMenuClicked.transform.GetChild(2).gameObject.SetActive(false);
                PokemonMenuClicked.transform.GetChild(5).gameObject.SetActive(false);
                break;
            case 2:
                PokemonMenuClicked.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }
}