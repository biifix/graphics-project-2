using UnityEngine;

public class Interactable : MonoBehaviour{

    [Header("Interaction Settings")]
    [SerializeField] private string interactionPrompt = "Press [E] to Interact";

    public string getPrompt() {
        return interactionPrompt;
    }
    
    public virtual void Interact(){
        Debug.Log("Interacted with " + gameObject.name);
    }
}
