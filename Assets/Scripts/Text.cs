using UnityEngine;
using TMPro;
using System.Collections;

namespace RPG
{
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
                }
                gameObject.SetActive(false);
                Text.text = string.Empty;
            }
        }
    }
}