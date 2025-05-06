using UnityEngine;
using TMPro;
using System.Collections;
using System.Xml.Linq;
using Unity.VisualScripting;

public class DText : MonoBehaviour
{
    [Header("Add Text Component")]
    public TextMeshProUGUI Text;

    [Header("Enter the Desired Text (Enter for next Line)")]
    [TextArea(3, 10)]
    public string InputString;

    [Header("Enter Speed rate of the dialog")]
    public float DialogueSpeed;

    string[] Lines;
    int index;
    bool done;

    public bool Test1 = false;
    private void Awake()
    {
        GameManagerTS.OnDirectiveChanged += CheckDirective;
    }
    void CheckDirective(Directive Dir)
    {
        switch (Dir)
        {
            case Directive._directive1:
                InputString = "Greetings, Architect! Colony Zeta-9 is offline.\r\nWe must restore the Core Habitat to begin operations.";
                break;
            case Directive._directive2:
                InputString = "Architect, the colony’s systems are stabilized, but we lack a functional habitat.\r\nDeploying a Core Habitat will provide shelter and a foundation for Zeta-9’s growth.\r\nAccess the blueprint menu from the bottom right Inventroy.";
                break;
        }
        Lines = InputString.Split(new[] { '\n' }, System.StringSplitOptions.None);
    }
    void Start()
    {
        Text.text = string.Empty;
        Lines = InputString.Split(new[] { '\n' }, System.StringSplitOptions.None);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && done)
        {
            if (Text.text == Lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                Text.text = Lines[index];
                done = true;
            }
        }
    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        done = false;
        Text.text = string.Empty;
        foreach (char c in Lines[index])
        {
            Text.text += c;
            yield return new WaitForSeconds(DialogueSpeed);
        }
        done = true;
    }

    void NextLine()
    {
        if (index < Lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (Test1)
            {
                GameManagerTS._checks[0] = true;
                Test1 = false;
            }
            gameObject.SetActive(false);
            Text.text = string.Empty;
        }
    }
}