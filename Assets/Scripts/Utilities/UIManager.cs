using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset mainMenuPanel;
    public VisualTreeAsset skillsMenuPanel;
    public VisualTreeAsset artifactsMenuPanel;
    public VisualTreeAsset optionsMenuPanel;
    public VisualTreeAsset classSelectorPanel;
    public VisualTreeAsset closeButtonTemplate;

    private VisualElement root;

    private VisualElement mainMenu;
    private VisualElement skillsMenu;
    private VisualElement artifactsMenu;
    private VisualElement optionsMenu;
    private VisualElement classSelector;

    public static event Action OnUIReady;
    public static bool IsUIReady { get; private set; }

    private void OnEnable()
    {
        IsUIReady = false;

        UIDocument doc = GetComponent<UIDocument>();
        if (doc == null)
        {
            return;
        }

        root = doc.rootVisualElement;
        if (root == null) return;

        mainMenu = mainMenuPanel?.Instantiate().Q<VisualElement>("MainMenu");
        skillsMenu = skillsMenuPanel?.Instantiate().Q<VisualElement>("SkillsMenu");
        artifactsMenu = artifactsMenuPanel?.Instantiate().Q<VisualElement>("ArtifactsMenu");
        optionsMenu = optionsMenuPanel?.Instantiate().Q<VisualElement>("OptionsMenu");
        classSelector = classSelectorPanel?.Instantiate().Q<VisualElement>("ClassSelector");

        if (mainMenu == null || skillsMenu == null || artifactsMenu == null || optionsMenu == null || classSelector == null)
            return;
        
        Button playButton = mainMenu.Q<Button>("PlayButton");
        Button skillsButton = mainMenu.Q<Button>("SkillsButton");
        Button artifactsButton = mainMenu.Q<Button>("ArtifactsButton");
        Button optionsButton = mainMenu.Q<Button>("OptionsButton");

        if (playButton == null || skillsButton == null || artifactsButton == null || optionsButton == null)
        {
            Debug.LogError("UIManager: Could not find one or more buttons in the main menu");
        }
        else
        {
            playButton.clicked += ShowClassSelector;
            skillsButton.clicked += ShowSkillsMenu;
            artifactsButton.clicked += ShowArtifactsMenu;
            optionsButton.clicked += ShowOptionsMenu;
        }
        
        var rootElement = root.Q<VisualElement>("Root Container");

        if (rootElement == null) return;
        
        rootElement.Add(mainMenu);
        rootElement.Add(skillsMenu);
        rootElement.Add(artifactsMenu);
        rootElement.Add(optionsMenu);
        rootElement.Add(classSelector);
        
        AddCloseButton(skillsMenu);
        AddCloseButton(artifactsMenu);
        AddCloseButton(optionsMenu);
        AddCloseButton(classSelector);

        ShowMainMenu();
        
        IsUIReady = true;
        OnUIReady?.Invoke();
    }

    private void AddCloseButton(VisualElement menu)
    {
        if (closeButtonTemplate == null)
        {
            return;
        }

        var closeButton = closeButtonTemplate.CloneTree().Q<Button>("CloseButton");

        if (closeButton == null)
        {
            return;
        }
        
        closeButton.clicked += () =>
        {
            ShowMainMenu();
        };
        
        menu.Add(closeButton);
    }

    private void ShowOptionsMenu()
    {
        mainMenu.style.display = DisplayStyle.None;
        skillsMenu.style.display = DisplayStyle.None;
        artifactsMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.Flex;
        classSelector.style.display = DisplayStyle.None;
        BringToFront(optionsMenu.parent);
    }

    private void ShowArtifactsMenu()
    {
        mainMenu.style.display = DisplayStyle.None;
        skillsMenu.style.display = DisplayStyle.None;
        artifactsMenu.style.display = DisplayStyle.Flex;
        optionsMenu.style.display = DisplayStyle.None;
        classSelector.style.display = DisplayStyle.None;
        BringToFront(artifactsMenu.parent);
    }

    private void ShowSkillsMenu()
    {
        mainMenu.style.display = DisplayStyle.None;
        skillsMenu.style.display = DisplayStyle.Flex;
        artifactsMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.None;
        classSelector.style.display = DisplayStyle.None;
        BringToFront(skillsMenu.parent);
    }

    private void ShowClassSelector()
    {
        mainMenu.style.display = DisplayStyle.None;
        skillsMenu.style.display = DisplayStyle.None;
        artifactsMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.None;
        classSelector.style.display = DisplayStyle.Flex;
        BringToFront(classSelector.parent);
    }

    private void ShowMainMenu()
    {
        mainMenu.style.display = DisplayStyle.Flex;
        skillsMenu.style.display = DisplayStyle.None;
        artifactsMenu.style.display = DisplayStyle.None;
        optionsMenu.style.display = DisplayStyle.None;
        classSelector.style.display = DisplayStyle.None;
        BringToFront(classSelector.parent);
    }

    private void BringToFront(VisualElement element)
    {
        var parent = element.parent;
        if (parent != null)
        {
            parent.Remove(element);
            parent.Add(element);
        }
    }
}
