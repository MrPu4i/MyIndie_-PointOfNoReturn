using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(3, 5)]
        public string text;
        public string speakerName; // Имя говорящего для каждой реплики
    }

    [System.Serializable]
    public class DialogueSequence
    {
        public DialogueLine[] lines; // Массив реплик с именами
        public bool isItTimelineDialogue = false;
        //public PlayableDirector timeline;
        public bool isItFinalScene = false;
    }

    [SerializeField] public DialogueSequence dialogueSequence;
    [SerializeField] public Collider2D triggerCollider;
    [SerializeField] public Animator animator = null;
    //[SerializeField] public FinalScene finaleScene = null;
    private bool isItTimelineDialogue;



    private void Start()
    {
        if (triggerCollider == null)
            triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //не проверяем, если был д
            if (dialogueSequence.isItFinalScene) //зашли в триггер финальной сцены
            {
                if (DialogueManager.Instance.isDialogueActive) //если активен диалог
                {
                    DialogueManager.Instance.EndDialogue(); //заканчиваем предыдущий диалог
                }
                //начинается скрипт FinalScene
              //  finaleScene.StartScene();
            }
            else if (!DialogueManager.Instance.isDialogueActive)
            {
                if (dialogueSequence.isItTimelineDialogue /*&& dialogueSequence.timeline != null*/)
                {
                    //Debug.Log("Зашли в диалог если тут есть напарник");
                    DialogueManager.Instance.StartDialogue(this, true, animator);
                    //Debug.Log("Строчка после диалога");
                }
                else
                {
                    //Debug.Log("Начали обычный диалог, без напарника");
                    DialogueManager.Instance.StartDialogue(this);
                }
                if (triggerCollider != null)
                {
                    triggerCollider.enabled = false;
                }
            }
        }
    }

    // Для ручного вызова из других скриптов
    public void TriggerDialogue()
    {
        /*if (!DialogueManager.Instance.isDialogueActive)
        {
            Debug.Log("До StartDialogue");
            DialogueManager.Instance.StartDialogue(this);
        }*/
        if (!DialogueManager.Instance.isDialogueActive)
        {
            if (dialogueSequence.isItTimelineDialogue /*&& dialogueSequence.timeline != null*/)
            {
                Debug.Log("Зашли в диалог если тут есть напарник и мы вызываем не через коллайдер");
                DialogueManager.Instance.StartDialogue(this, true);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(this);
            }
        }
    }
}
