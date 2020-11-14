using PokemonUnity;
using System;
using UnityEngine;
using UnityEngine.UI;
public class MiniWindowData : MonoBehaviour
{
    //SerializeField
    [SerializeField]
    private Text Move_Place_Text;
    [SerializeField]
    private GameObject PokemonSelected;
    [SerializeField]
    private GameObject MiniPartyView;
    [SerializeField]
    private GameObject PokemonStats;
    [SerializeField]
    private PokemonMouse pokemonMouse;
    //End of SerializeField Stuff

    public Image[] MiniPartyImage;
    private PCDataUI DataUI { get { return transform.GetComponentInParent <PCDataUI>(); } }
    public int PCButtonID { get; private set; }
    public bool PartyOrPC { get; private set; }
    public int PartyButtonBehivour { get; private set; }
    
    public void InitAddPartyButton()
    {
        for (int i = 0; i < PCManager.player.Party.Length; i++)
        {
            MiniPartyView.transform.GetChild(i).gameObject.AddComponent<MiniPartyButton>();
            MiniPartyView.transform.GetChild(i).gameObject.GetComponent<MiniPartyButton>().SetData(this, i);
        }
    }
    public void Pass_PCButtonID(int id)
    {
        if (!PCManager.player.PC.Pokemons[id].IsNotNullOrNone() && !pokemonMouse.HasPokemonOnYourMouse) { }
        else
        {
            PartyOrPC = true;
            if (pokemonMouse.HasPokemonOnYourMouse)
            {
                Move_Place_Text.text = "Place";
                ActiveGameobjectMode(1);
                PartyButtonBehivour = 0;
            }
            else if (PCManager.player.Party.GetCount() >= PCManager.player.Party.Length)
            {
                Debug.Log("Full");
                ActiveGameobjectMode(2);
            }
            else
            {
                Move_Place_Text.text = "Move";
                pokemonMouse.BoolPartyMouse(false);
                ActiveGameobjectMode(0);
            }
            PokemonSelected.SetActive(true);
            PCButtonID = id;
            DataUI.DisplayPokemonStatsUI(PCManager.player.PC.Pokemons[id]);
        }
        
    }
    public void Pass_PartyButtonID(int id)
    {
        if (!PCManager.player.Party[id].IsNotNullOrNone() && !pokemonMouse.HasPokemonOnYourMouse) { }
        else
        {
            pokemonMouse.InitSprite(1);
            PartyOrPC = false;
            PartyButtonBehivour = 0;
            Debug.Log(id);
            if (pokemonMouse.HasPokemonOnYourMouse)
            {
                Move_Place_Text.text = "Place";
                PartyButtonBehivour = 1;
                ActiveGameobjectMode(1);
            }
            else
            {
                Move_Place_Text.text = "Move";
                PartyButtonBehivour = 2;
                ActiveGameobjectMode(0);
            }
            PokemonSelected.SetActive(true);
            PCButtonID = id;
        }
    }
    public void PokemonMenu(int button_ID)
    {
        switch (button_ID)
        {
            case 1: // Move/Place Button
                if (PartyButtonBehivour == 1 && pokemonMouse.HasPokemonOnYourMouse)
                {
                    Debug.Log("Mouse should able to place it");
                    PartyOrPC = false;
                }
                else if (PartyButtonBehivour == 2)
                {
                    Debug.Log("Party should able to move");
                    pokemonMouse.BoolPartyMouse(true);
                }
                DataUI.Move_Place_Pkmn(PCButtonID);
                PokemonSelected.SetActive(false);
                break;
            case 2: //Summary Button
                throw new NotImplementedException();
            case 3: //Withdraw Button
                DataUI.WithdrawToParty(PCButtonID);
                PokemonSelected.SetActive(false);
                break;
            case 4: //Item Button
                throw new NotImplementedException();
            case 5: // Mark Button
                throw new NotImplementedException();
            case 6: //Release Button
                DataUI.Release_Pkmn(PCButtonID);
                DataUI.UpdatePCButtonDisplay();
                PokemonSelected.SetActive(false);
                break;
            case 7: //Debug Button
                throw new NotImplementedException();
            case 8: //Cancel Button
                pokemonMouse.InitSprite(1);
                PokemonSelected.SetActive(false);
                break;
        }
        Debug.Log("Switch (button_ID) Done");
    }
    public void ActiveGameobjectMode(int Mode)
    {
        switch (Mode)
        {
            case 0:
                PokemonSelected.transform.GetChild(2).gameObject.SetActive(true);
                PokemonSelected.transform.GetChild(5).gameObject.SetActive(true);
                break;
            case 1:
                PokemonSelected.transform.GetChild(2).gameObject.SetActive(false);
                PokemonSelected.transform.GetChild(5).gameObject.SetActive(false);
                break;
            case 2:
                PokemonSelected.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }
    public void PartyOnClick()
    {
        Debug.Log("initPartyClick");
        if (!MiniPartyView.activeSelf)
        {
            Debug.Log("Active");
            UpdatePartyUI(1);
            MiniPartyView.SetActive(true);
        }
        else
        {
            Debug.Log("Not active");
            UpdatePartyUI(0);
            MiniPartyView.SetActive(false);
        }
    }
    /// <summary>
    /// 1 for Update & 0 for Clear
    /// </summary>
    /// <param name="mode"></param>
    public void UpdatePartyUI(int mode)
    {
        switch (mode)
        {
            case 1:
                for (int i = 0; i < PCManager.player.Party.Length; i++)
                {

                    if (!PCManager.player.Party[i].IsNotNullOrNone())
                    {
                        MiniPartyImage[i].color = UnityEngine.Color.clear;
                    }
                    else
                    {
                        MiniPartyImage[i].color = UnityEngine.Color.white;
                        MiniPartyImage[i].sprite = PCManager.PokemonSprite[(int)PCManager.player.Party[i].Species];
                    }
                }
                break;
            case 0:
                for (int i = 0; i < PCManager.player.Party.Length; i++)
                {
                    MiniPartyImage[i].color = UnityEngine.Color.clear;
                    MiniPartyImage[i].sprite = null;
                }
                break;
        }
    }
    public void BoxNavigateArrow(int arrowvalue)
    {
        Debug.Log(arrowvalue);
        byte PCShouldBeThis;
        int TotalValue;
        switch (arrowvalue)
        {
            case 1:

                if (Core.STORAGEBOXES > PCManager.player.PC.ActiveBox + arrowvalue)
                {
                    TotalValue = PCManager.player.PC.ActiveBox + arrowvalue;
                    PCShouldBeThis = Convert.ToByte(TotalValue);
                    Debug.Log(PCManager.player.PC[PCShouldBeThis]);
                    //PCManager.CurrentPCBox += arrowvalue;
                    Debug.Log("Move to next box!");
                    Debug.Log(PCManager.player.PC.ActiveBox);

                    DataUI.UpdatePCButtonDisplay();
                    DataUI.BoxNum.text = string.Format("Box: {0}", PCManager.player.PC.ActiveBox + 1);
                }
                else
                {
                    Debug.Log("You can't move next box!");
                }
                break;
            case -1:
                if (PCManager.player.PC.ActiveBox + arrowvalue >= 0)
                {
                    TotalValue = PCManager.player.PC.ActiveBox + arrowvalue;
                    PCShouldBeThis = Convert.ToByte(TotalValue);
                    Debug.Log(PCManager.player.PC[PCShouldBeThis]);
                    //PCManager.CurrentPCBox += arrowvalue;
                    Debug.Log("You can move back box!");
                    Debug.Log(PCManager.player.PC.ActiveBox);
                    DataUI.UpdatePCButtonDisplay();
                    DataUI.BoxNum.text = string.Format("Box: {0}", PCManager.player.PC.ActiveBox + 1);
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
}
