using UnityEngine;
using UnityEngine.EventSystems;
public class MiniPartyButton : MonoBehaviour, IPointerDownHandler
{
    public int ID { get; private set; }
    private PCGameObjectAndData ObjectData;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked and it should be Party not Pokemon!");
        ObjectData.PassIDToManager(1, ID);
    }
    public void SetData(int id, PCGameObjectAndData objectData)
    {
        ID = id;
        ObjectData = objectData;
    }
}