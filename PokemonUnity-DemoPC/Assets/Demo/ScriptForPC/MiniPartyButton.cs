using UnityEngine;
using UnityEngine.EventSystems;

public class MiniPartyButton : MonoBehaviour, IPointerDownHandler
{
    public int ID { get; private set; }
    MiniWindowData windowData;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked and it should be Party not Pokemon!");
        windowData.Pass_PartyButtonID(ID);
    }
    public void SetData(MiniWindowData data, int ID)
    {
        windowData = data;
        this.ID = ID;
    }
}
