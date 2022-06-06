using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonReset : MonoBehaviour
{
    public void onClick(){
        EventSystem.current.SetSelectedGameObject(null);
    }
}
