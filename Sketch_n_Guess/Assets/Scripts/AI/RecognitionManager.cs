using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecognitionManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recognitionResult; // UI element to display recognition result
    [SerializeField] private Button _templateModeButton; // Button to switch to template mode
    [SerializeField] private Button _recognitionModeButton; // Button to switch to recognition mode
    [SerializeField] private Button _reviewTemplates; // Button to switch to template review mode
    [SerializeField] private TMP_InputField _templateName; // Input field for template name
    [SerializeField] private TemplateReviewPanel _templateReviewPanel; // Panel for reviewing templates
    [SerializeField] private RecognitionPanel _recognitionPanel; // Panel for recognition mode

    private GestureTemplates _templates => GestureTemplates.Get(); // Getter for gesture templates
    private static readonly DollarOneRecognizer _dollarOneRecognizer = new DollarOneRecognizer(); // DollarOne recognizer instance
    private static readonly DollarPRecognizer _dollarPRecognizer = new DollarPRecognizer(); // DollarP recognizer instance
    private IRecognizer _currentRecognizer = _dollarOneRecognizer; // Currently used recognizer
    private RecognizerState _state = RecognizerState.RECOGNITION; // Current state of the recognizer

    public enum RecognizerState
    {
        TEMPLATE,
        RECOGNITION,
        TEMPLATE_REVIEW
    }

    [Serializable]
    public struct GestureTemplate
    {
        public string Name;
        public DollarPoint[] Points;

        // Constructor for GestureTemplate
        public GestureTemplate(string templateName, DollarPoint[] preparePoints)
        {
            Name = templateName;
            Points = preparePoints;
        }
    }

    private string TemplateName => _templateName.text; // Getter for template name input field text

    // Called when the script instance is being loaded
    private void Start()
    {
        _drawable.OnDrawFinished += OnDrawFinished; // Subscribe to the draw finished event
        _templateModeButton.onClick.AddListener(() => SetupState(RecognizerState.TEMPLATE)); // Add listener for template mode button
        _recognitionModeButton.onClick.AddListener(() => SetupState(RecognizerState.RECOGNITION)); // Add listener for recognition mode button
        _reviewTemplates.onClick.AddListener(() => SetupState(RecognizerState.TEMPLATE_REVIEW)); // Add listener for review templates button
        _recognitionPanel.Initialize(SwitchRecognitionAlgorithm); // Initialize the recognition panel

        SetupState(_state); // Setup initial state
    }

    // Switch between different recognition algorithms
    private void SwitchRecognitionAlgorithm(int algorithm)
    {
        if (algorithm == 0)
        {
            _currentRecognizer = _dollarOneRecognizer; // Set current recognizer to DollarOneRecognizer
        }

        if (algorithm == 1)
        {
            _currentRecognizer = _dollarPRecognizer; // Set current recognizer to DollarPRecognizer
        }
    }

    // Set up the current state of the recognizer
    private void SetupState(RecognizerState state)
    {
        _state = state;
        _templateModeButton.image.color = _state == RecognizerState.TEMPLATE ? Color.green : Color.white; // Change button color based on state
        _recognitionModeButton.image.color = _state == RecognizerState.RECOGNITION ? Color.green : Color.white; // Change button color based on state
        _reviewTemplates.image.color = _state == RecognizerState.TEMPLATE_REVIEW ? Color.green : Color.white; // Change button color based on state
        _templateName.gameObject.SetActive(_state == RecognizerState.TEMPLATE); // Show template name input field in template mode
        _recognitionResult.gameObject.SetActive(_state == RecognizerState.RECOGNITION); // Show recognition result in recognition mode

        _drawable.gameObject.SetActive(state != RecognizerState.TEMPLATE_REVIEW); // Show drawable area except in template review mode
        _templateReviewPanel.SetVisibility(state == RecognizerState.TEMPLATE_REVIEW); // Show template review panel in template review mode
        _recognitionPanel.SetVisibility(state == RecognizerState.RECOGNITION); // Show recognition panel in recognition mode
    }

    // Event handler for when drawing is finished
    private void OnDrawFinished(DollarPoint[] points)
    {
        if (_state == RecognizerState.TEMPLATE)
        {
            // Create a new template and add to the templates list
            GestureTemplate preparedTemplate = new GestureTemplate(TemplateName, _currentRecognizer.Normalize(points, 64));
            _templates.RawTemplates.Add(new GestureTemplate(TemplateName, points));
            _templates.ProceedTemplates.Add(preparedTemplate);
        }
        else
        {
            // Perform recognition on the drawn points
            (string, float) result = _currentRecognizer.DoRecognition(points, 64, _templates.RawTemplates);
            string resultText = "";
            if (_currentRecognizer is DollarOneRecognizer)
            {
                resultText = $"Recognized: {result.Item1}, Score: {result.Item2}";
            }
            else if (_currentRecognizer is DollarPRecognizer)
            {
                resultText = $"Recognized: {result.Item1}, Distance: {result.Item2}";
            }

            _recognitionResult.text = resultText; // Display recognition result
            Debug.Log(resultText); // Log recognition result
        }
    }

    // Save templates when the application quits
    private void OnApplicationQuit()
    {
        _templates.Save();
    }
}
