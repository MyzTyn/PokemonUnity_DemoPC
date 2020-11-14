using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCButton : MonoBehaviour, IPointerDownHandler
{
    public int ID { get; private set; }
    public Image PokemonSprite { get { return GetComponent<Image>(); } }
    private MiniWindowData Data;
    public void OnPointerDown(PointerEventData eventData)
    {
        Data.Pass_PCButtonID(ID);
    }
    public void SetID(int id, MiniWindowData data)
    {
        ID = id;
        Data = data;
    }
    public void UpdatePCUI()
    {
        PokemonSprite.color = Color.white;
        PokemonSprite.sprite = PCManager.PokemonSprite[PCManager.PokemonSpriteNum(ID)];
    }
    public void DisablePCUI()
    {
        PokemonSprite.sprite = null;
        PokemonSprite.color = Color.clear;
    }
}
