using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(1, 3)] //минимально линий 1, макс - 2
        public string text;
        public string speakerName; // Имя говорящего для каждой реплики
    }

    [System.Serializable]
    public class DialogueSequence
    {
        public DialogueLine[] lines; // Массив реплик с именами
    }

    [SerializeField] public DialogueSequence dialogueSequence; //Сам диалог, который состоит из реплик

    // Для ручного вызова из других скриптов
    public void TriggerDialogue() //Используем это, потому что в коде только включаем
    {
        if (!DialogueManager.Instance.isDialogueActive) //Если нет активного диалога - начинаем
        {
           // DialogueManager.Instance.StartDialogue(this); //Начинаем диалог
        }
    }

    // Новый метод для создания диалога из кода
    public void SetDialogueFromCodeMany(string[] texts, string[] speakerNames)
    {
        dialogueSequence = new DialogueSequence();
        dialogueSequence.lines = new DialogueLine[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            dialogueSequence.lines[i] = new DialogueLine
            {
                text = texts[i],
                speakerName = speakerNames[i]
            };
        }
    }

    // Упрощенный метод (если только один говорящий)
    public void SetDialogueFromCodeSolo(string[] texts, string speakerName = "Персонаж")
    {
        dialogueSequence = new DialogueSequence();
        dialogueSequence.lines = new DialogueLine[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            dialogueSequence.lines[i] = new DialogueLine
            {
                text = texts[i],
                speakerName = speakerName
            };
        }
    }
}
