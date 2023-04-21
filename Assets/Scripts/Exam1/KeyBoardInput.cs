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
    public bool active {get; private set;}
    public static event Action<KeyBoardInput> onClick;

    private void Awake() {
        button = GetComponent<Button>();
        content = GetComponentInChildren<Text>();
        active = value == "ON";
    }

    private void OnEnable() {
        CalcSimulator.onActive += Active;
        button.onClick.AddListener(ClickButton);
    }

    private void ClickButton() {
        if(active) {
            onClick?.Invoke(this);
        }
    }

    private void Active(bool active) {
        if(value != "ON")
            this.active = active;
    }
}
