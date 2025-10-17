using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
//using Unity.Cinemachine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    [SerializeField] public GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] TextMeshProUGUI speakerNameText;
    [SerializeField] public Button nextButton;

    [Header("Ссылки")]
    [HideInInspector] public DialogueTrigger.DialogueLine[] currentSentence;
    [HideInInspector] public int currentSentenceIndex;
    [HideInInspector] public bool isTyping;
    [HideInInspector] public bool isDialogueActive; // Флаг активного диалога
    [SerializeField] private AudioSource typeSourse;
    [SerializeField] private AudioClip typeClip;
    [SerializeField] private Shop shop;

    private float minPitch = 0.9f;
    private float maxPitch = 1.1f;
    private bool isThisMasterDialogue;
    private bool isThisAfterTabletka;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        nextButton.onClick.AddListener(NextSentence);
    }

    // Новый метод для запуска диалога напрямую из кода
    public void StartDialogueFromCode(string[] texts, string[] speakerNames, bool master = false)
    {
        isThisMasterDialogue = master;
        
        if (isDialogueActive)
        {
            Debug.Log("Закончили диалог");
            EndDialogue();
        }


        // Создаем массив DialogueLine
        DialogueTrigger.DialogueLine[] dialogueLines = new DialogueTrigger.DialogueLine[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            dialogueLines[i] = new DialogueTrigger.DialogueLine
            {
                text = texts[i],
                speakerName = speakerNames[i]
            };
        }

        //Вместо StartDialogue

        // Запускаем диалог
        currentSentence = dialogueLines;
        currentSentenceIndex = 0;
        isDialogueActive = true;

        //Debug.Log("Устанавливаем панель");
        dialoguePanel.SetActive(true);
        DisplayCurrentLine();
    }

    // Упрощенная версия с одним говорящим
    public void StartDialogueFromCodeSolo(string[] texts, string speakerName = "Персонаж", bool tabletka = false)
    {
        isThisAfterTabletka = tabletka;
        Debug.Log(isThisAfterTabletka);
        string[] speakerNames = new string[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            speakerNames[i] = speakerName;
        }

        StartDialogueFromCode(texts, speakerNames);
    }

/*    public virtual void StartDialogue(DialogueTrigger trigger, Animator _animator = null)
    {

        if (isDialogueActive)
            EndDialogue();

        currentSentence = trigger.dialogueSequence.lines; //Передаём предложения и уже воспроизводим сам диалог
        currentSentenceIndex = 0;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);
        DisplayCurrentLine();
    }*/
    public void ShowCurrentLine()
    {
        if (!isDialogueActive) return;
        dialoguePanel.SetActive(true);
        DisplayCurrentLine();
    }
    public void DisplayCurrentLine()
    {
        speakerNameText.text = currentSentence[currentSentenceIndex].speakerName;

        StopCoroutine("TypeSentence");
        StartCoroutine(TypeSentence(currentSentence[currentSentenceIndex].text));
    }
    private IEnumerator TypeSentence(string sentence)
    {
/*        Debug.Log(sentence);
        Debug.Log("Начили корутин с тайм сентенс");*/
        isTyping = true;
        dialogueText.text = "";
        /*Debug.Log(sentence.ToCharArray());*/
        /*foreach (char letter in sentence.ToCharArray())
        {
            Debug.Log($"{letter}");
        }*/
            foreach (char letter in sentence.ToCharArray())
        {
            /*Debug.Log($"{letter}");*/
            dialogueText.text += letter;
            /*Debug.Log(dialogueText.text);*/
            //звук буквыыыы
            typeSourse.pitch = Random.Range(minPitch, maxPitch);
            typeSourse.PlayOneShot(typeClip);
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    public void NextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence[currentSentenceIndex].text;
            isTyping = false;
            return;
        }

        currentSentenceIndex++;
        if (currentSentenceIndex < currentSentence.Length)
        {
                DisplayCurrentLine();
        }
        else
        {
            //Debug.Log("СУКА ЗАКОНЧИЛИСЬ СЛОВА");

            //Если это был разговор с мастером - нужно переходить к меню 
            Debug.Log(isThisAfterTabletka);
            if (isThisMasterDialogue == true)
                shop.MasterDialogEnd();
            if (isThisAfterTabletka == true)
                SceneManager.LoadScene("Room");
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false; // Сбрасываем флаг при завершении
    }
    private IEnumerator Damping()
    {
        yield return null;
    }
}
