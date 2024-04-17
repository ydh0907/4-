using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour{
    public TextMeshProUGUI textCompoment;
    public string[] lines;
    public float textSpeed;

    private int index;

    private void Start() {
        textCompoment.text = string.Empty;
        StartDialogue();
    }
    private void StartDialogue() {
        index = Random.Range(0, lines.Length);
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine() {
        foreach(char c in lines[index].ToCharArray()) {
            textCompoment.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
