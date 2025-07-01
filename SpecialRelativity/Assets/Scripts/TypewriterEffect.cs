using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [Header("Typewriter Settings")]
    [SerializeField, Range(1f, 100f)]
    private float charactersPerSecond = 20f;

    [SerializeField, Range(0f, 2f)]
    private float interpunctuationDelay = 0.5f;

    [Header("Skip Settings")]
    [SerializeField]
    private bool allowQuickSkip = true;

    [SerializeField, Min(1)]
    private int skipSpeedup = 5;

    [Header("Completion Settings")]
    [SerializeField, Range(0.1f, 1f)]
    private float completionDelay = 0.25f;

    // Events
    public static event Action CompleteTextRevealed;
    public static event Action<char> CharacterRevealed;

    // Properties
    public bool IsTyping { get; private set; }
    public bool IsSkipping { get; private set; }
    public bool IsComplete { get; private set; }

    // Private fields
    private TMP_Text _textBox;
    private int _currentVisibleCharacterIndex;
    private Coroutine _typewriterCoroutine;
    private bool _readyForNewText = true;

    // Cached delays for performance
    private WaitForSeconds _normalDelay;
    private WaitForSeconds _interpunctuationWait;
    private WaitForSeconds _skipDelay;
    private WaitForSeconds _completionWait;

    // Punctuation characters that trigger longer pauses
    private static readonly char[] PunctuationChars = { '?', '.', ',', ':', ';', '!', '-' };

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();
        CacheDelays();
    }

    private void Start()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    private void OnDestroy()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
        StopTypewriter();
    }

    private void CacheDelays()
    {
        _normalDelay = new WaitForSeconds(1f / Mathf.Max(charactersPerSecond, 0.1f));
        _interpunctuationWait = new WaitForSeconds(interpunctuationDelay);
        _skipDelay = new WaitForSeconds(1f / Mathf.Max(charactersPerSecond * skipSpeedup, 0.1f));
        _completionWait = new WaitForSeconds(completionDelay);
    }

    private void OnTextChanged(UnityEngine.Object obj)
    {
        if (obj != _textBox || !_readyForNewText)
            return;

        PrepareForNewText();
    }

    private void PrepareForNewText()
    {
        IsSkipping = false;
        _readyForNewText = false;

        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);

        _textBox.maxVisibleCharacters = 0;
        _currentVisibleCharacterIndex = 0;
        IsTyping = false;
        IsComplete = false;

        _typewriterCoroutine = StartCoroutine(TypewriterCoroutine());
    }

    public void StartTypewriter()
    {
        if (_textBox == null || string.IsNullOrEmpty(_textBox.text))
            return;

        PrepareForNewText();
    }

    public void StartTypewriter(string newText)
    {
        if (_textBox == null)
            return;

        _textBox.text = newText;
        StartTypewriter();
    }

    public void StopTypewriter()
    {
        if (_typewriterCoroutine != null)
        {
            StopCoroutine(_typewriterCoroutine);
            _typewriterCoroutine = null;
        }
        IsTyping = false;
        IsSkipping = false;
    }

    public void Skip()
    {
        if (!IsTyping || IsComplete)
            return;

        if (allowQuickSkip && !IsSkipping)
            IsSkipping = true;
        else
            CompleteImmediately();
    }

    public void CompleteImmediately()
    {
        if (_textBox == null)
            return;

        StopTypewriter();

        _textBox.ForceMeshUpdate();

        _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
        _currentVisibleCharacterIndex = _textBox.textInfo.characterCount;
        IsComplete = true;
        _readyForNewText = true;

        CompleteTextRevealed?.Invoke();
    }

    private IEnumerator TypewriterCoroutine()
    {
        IsTyping = true;
        IsComplete = false;

        // Force text info update
        _textBox.ForceMeshUpdate();
        TMP_TextInfo textInfo = _textBox.textInfo;

        if (textInfo.characterCount == 0)
        {
            CompleteTypewriter();
            yield break;
        }

        while (_currentVisibleCharacterIndex < textInfo.characterCount)
        {
            // Update visible characters
            _textBox.maxVisibleCharacters = _currentVisibleCharacterIndex + 1;

            // Get current character safely
            if (_currentVisibleCharacterIndex < textInfo.characterInfo.Length)
            {
                char currentChar = textInfo.characterInfo[_currentVisibleCharacterIndex].character;
                CharacterRevealed?.Invoke(currentChar);

                // Determine delay based on character and skip state
                WaitForSeconds delay = GetDelayForCharacter(currentChar);
                yield return delay;
            }

            _currentVisibleCharacterIndex++;
        }

        // Ensure all characters are visible
        _textBox.maxVisibleCharacters = textInfo.characterCount;

        // Wait before completing
        yield return _completionWait;

        CompleteTypewriter();
    }

    private WaitForSeconds GetDelayForCharacter(char character)
    {
        if (IsSkipping)
            return _skipDelay;

        // Check if character is punctuation that should have longer delay
        for (int i = 0; i < PunctuationChars.Length; i++)
        {
            if (PunctuationChars[i] == character)
                return _interpunctuationWait;
        }

        return _normalDelay;
    }

    private void CompleteTypewriter()
    {
        IsTyping = false;
        IsSkipping = false;
        IsComplete = true;
        _readyForNewText = true;
        CompleteTextRevealed?.Invoke();
    }
}
