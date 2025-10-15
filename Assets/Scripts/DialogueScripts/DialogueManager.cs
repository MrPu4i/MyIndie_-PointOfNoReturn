using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
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
    //[SerializeField] private CinemachineCamera cineCam;
    [HideInInspector] public DialogueTrigger.DialogueLine[] currentSentence;
    [HideInInspector] public int currentSentenceIndex;
    [HideInInspector] public bool isTyping;
    [HideInInspector] public bool isDialogueActive; // Флаг активного диалога
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource typeSound;

    private float minPitch = 0.9f;
    private float maxPitch = 1.1f;
    private Vector3 cordPlayer;


    private Animator animator;
    private PlayerInput playerInput;
    //private CinemachinePositionComposer positionComposer;


    private void Awake()
    {
        playerInput = player.GetComponent<PlayerInput>();
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        nextButton.onClick.AddListener(NextSentence);
    }

    public virtual void StartDialogue(DialogueTrigger trigger, bool timelineBased = false, Animator _animator = null)
    {
        animator = _animator;
        if (isDialogueActive)
            EndDialogue();

        currentSentence = trigger.dialogueSequence.lines;
        currentSentenceIndex = 0;
        isDialogueActive = true;

        if (animator != null) //передали аниматор
        {
            animator.SetTrigger("Next"); //триггерим, что бы перешёл в анимацию поднятия головы и далее смотрения на нас
            //блокируем движение до окончания диалога
            playerInput.enabled = false; //отключаем управление
            //передвинуть камеру красиво
    //        cineCam.Lens.OrthographicSize = 6f;
            StartCoroutine(DialogueCamPartner());
        }
        dialoguePanel.SetActive(true);
        DisplayCurrentLine();
    }
    private IEnumerator DialogueCamPartner()
    {
        yield return null;
        //cordPlayer = cineCam.transform.position;
        //Debug.Log(cineCam.transform.position);
        //cineCam.transform.position = new Vector3(3, -138, -10);
        //Debug.Log(cineCam.transform.position);
       // cineCam.Follow = null;
        //cineCam.LookAt = null;
        //cineCam.GetComponent<CinemachineConfiner2D>().InvalidateBoundingShapeCache();
    }
    public void ShowCurrentLine()
    {
        if (!isDialogueActive) return;
        dialoguePanel.SetActive(true);
        DisplayCurrentLine();
    }
    public void DisplayCurrentLine()
    {
        speakerNameText.text = currentSentence[currentSentenceIndex].speakerName;
        StartCoroutine(TypeSentence(currentSentence[currentSentenceIndex].text));
    }
    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            //звук буквыыыы
            typeSound.pitch = Random.Range(minPitch, maxPitch);
            typeSound.Play();
            yield return new WaitForSeconds(0.05f);
        }

        isTyping = false;
    }

    public void NextSentence()
    {
        //Debug.Log("NextSentence");
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence[currentSentenceIndex].text;
            isTyping = false;
            return;
        }

        currentSentenceIndex++;
        //Debug.Log($"{currentSentenceIndex}, {currentSentence.Length}");
        if (currentSentenceIndex < currentSentence.Length)
        {
            /*if (isItTimelineDialogue)
            {
                timelineDirector.Resume();
                //если мы говорим с напарником - можем поймать конкретный момент и что-то сделать. Например поменять анимацию
            }
            else
            {*/
                DisplayCurrentLine();
            /*}*/
        }
        else
        {
            //Debug.Log("СУКА ЗАКОНЧИЛИСЬ СЛОВА");
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        //Debug.Log("Зашли в коНЕЦ ДИАЛОГА");
        if (animator != null)
        {
            animator.SetTrigger("Next"); //ещё раз триггерим, что бы он опустил голову
            playerInput.enabled = true; //возвращаем управление
            //Debug.Log("Включили управление в DialogueManager");

           // cineCam.transform.position = cordPlayer;
           // cineCam.Lens.OrthographicSize = 9.70f;
            StartCoroutine(Damping());
        }
        dialoguePanel.SetActive(false);
        isDialogueActive = false; // Сбрасываем флаг при завершении
    }
    private IEnumerator Damping()
    {
        yield return null;
       // cineCam.Follow = player.transform;
       // cineCam.LookAt = player.transform;
        //cineCam.GetComponent<CinemachineConfiner2D>().InvalidateBoundingShapeCache();
    }
}
