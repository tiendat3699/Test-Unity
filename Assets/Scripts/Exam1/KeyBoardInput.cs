using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class KeyBoardInput : MonoBehaviour
{   
    public enum Type {
        numberic,
        operation,
        equal,
        function
    }
    public Type type;
    public string value {
        get {
            return content.text;
        }
    }
    private Button button;
    private Text content;
    public static event Action<KeyBoardInput> onClick;

    private void Awake() {
        button = GetComponent<Button>();
        content = GetComponentInChildren<Text>();
    }

    private void OnEnable() {
        button.onClick.AddListener(ClickButton);
    }

    private void ClickButton() {
        onClick?.Invoke(this);
    }
}
