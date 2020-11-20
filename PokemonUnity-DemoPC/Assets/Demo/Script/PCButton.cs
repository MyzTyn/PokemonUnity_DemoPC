using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PCButton : MonoBehaviour, IPointerDownHandler
{
    public int ID { get; private set; }
    public Image PokemonSprite { get { return GetComponent<Image>(); } }
    private PCGameObjectAndData Data;
    public void OnPointerDown(PointerEventData eventData)
    {
        Data.PassIDToManager(2, ID);
    }
    public void SetPCButtonData(int id, PCGameObjectAndData data)
    {
        ID = id;
        Data = data;
    }
    public void UpdatePCButtonUI()
    {
        PokemonSprite.color = Color.white;
        PokemonSprite.sprite = PCManager.PokemonSprite[PCManager.PokemonSpriteNum(1, ID)];
    }
    public void DisablePCButonUI()
    {
        PokemonSprite.sprite = null;
        PokemonSprite.color = Color.clear;
    }
}
