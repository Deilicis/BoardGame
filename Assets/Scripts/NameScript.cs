using UnityEngine;
using TMPro;
public class NameScript : MonoBehaviour
{
    public string playerName;
    TextMeshPro textMeshPro; 

    void Awake()
    {
        textMeshPro = transform.Find("NameField").gameObject.GetComponent<TextMeshPro>();
    }

    public void SetName(string name)
    {
        playerName = name; // <-- Store the name for later access
        textMeshPro.text = name;
        textMeshPro.color = new Color32(
            (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255),
            255);
    }
}
