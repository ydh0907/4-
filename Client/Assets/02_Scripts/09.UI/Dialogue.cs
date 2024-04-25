using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour{
    public TextMeshProUGUI textCompoment;
    public string[] lines;
    public float waitTextSpeed;

    private int index;

    private void OnEnable()
    {
        textCompoment.text = string.Empty;
        StartDialogue();
    }

    private void StartDialogue() {
        index = Random.Range(0, lines.Length);
        textCompoment.text = lines[index].ToString();
    }
}
