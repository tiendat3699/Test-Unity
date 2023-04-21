using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CalcSimulator : MonoBehaviour
{
    [SerializeField] private Text displayText;
    private string operators;
    private List<string> numbers;

    private void Awake() {
        numbers = new List<string>();
    }

    private void OnEnable() {   
        KeyBoardInput.onClick += keyBoardClick;
    }

    private void keyBoardClick(KeyBoardInput keyBoard) {
        if(keyBoard.type == KeyBoardInput.Type.equal) {
            Regex digitsOnly = new Regex(@"[0-9]");
            operators = digitsOnly.Replace(displayText.text, "");
            numbers = displayText.text.Split(new char[] {'/','x','+','-'}).ToList();
            Operation("/");
            Operation("x");
            Operation("+");
            Operation("-");

            displayText.text = numbers[0];
        } else if(keyBoard.type == KeyBoardInput.Type.operation){
            if(!String.IsNullOrEmpty(displayText.text)) {
                displayText.text += keyBoard.value;
            }
        } else {
            displayText.text += keyBoard.value;
        }
    }

    private float GetFloat(string value) {
        return float.Parse(value, CultureInfo.CurrentCulture.NumberFormat);
    }

    private void Operation(string opera) {
        int indexOpera = operators.IndexOf(opera);
        while(indexOpera != -1) {
                var removedList = numbers.GetRange(indexOpera, 2);
                removedList[indexOpera] = Calc(opera, indexOpera);
                numbers = removedList;
                operators = operators.Remove(indexOpera, 1);
                indexOpera = operators.IndexOf(opera);
        }
    }

    private string Calc(string opera, int index) {
        switch(opera) {
            case "/":
                return (GetFloat(numbers[index]) / GetFloat(numbers[index + 1])).ToString();
            case "x":
                return (GetFloat(numbers[index]) * GetFloat(numbers[index + 1])).ToString();
            case "+":
                return (GetFloat(numbers[index]) + GetFloat(numbers[index + 1])).ToString();
            default:
                return (GetFloat(numbers[index]) - GetFloat(numbers[index + 1])).ToString();
        }
    }
}
