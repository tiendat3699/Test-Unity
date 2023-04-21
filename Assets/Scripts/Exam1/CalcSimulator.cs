using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CalcSimulator : MonoBehaviour
{
    [SerializeField] private Text displayResult;
    [SerializeField] private Text displayCalc;
    [SerializeField] private Image screen;
    [SerializeField] private AudioClip clickSound;
    private string input;
    private AudioSource audioSource;
    private string operators;
    private Queue<float> numbers;
    private string storedOera;
    private Regex isNumber = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
    private bool flag;
    private float result;
    public static event Action<bool> onActive;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        screen.color = Color.black;
        numbers = new Queue<float>(2);
    }

    private void OnEnable()
    {
        KeyBoardInput.onClick += keyBoardClick;
    }

    private void OnDisable()
    {
        KeyBoardInput.onClick -= keyBoardClick;
    }

    private void keyBoardClick(KeyBoardInput keyBoard)
    {   
        if(keyBoard.active) {
            audioSource.PlayOneShot(clickSound);
        }
        switch (keyBoard.type)
        {
            //khi bấm nút "="
            case KeyBoardInput.Type.equal:
                numbers.Enqueue(GetFloat(input));
                input = "";
                if(numbers.Count == 2) {
                    float number1 = numbers.Dequeue();
                    float number2 = numbers.Dequeue();
                    result = Calc(storedOera, number1, number2);
                    numbers.Enqueue(result);
                    displayResult.text = result.ToString();
                    flag = true;
                }
                break;
            //khi bấm nút phép tính
            case KeyBoardInput.Type.operation:
                numbers.Enqueue(GetFloat(input));
                displayResult.text = "";
                input = "";
                if(numbers.Count == 2) {
                    float number1 = numbers.Dequeue();
                    float number2 = numbers.Dequeue();
                    result = Calc(storedOera, number1, number2);
                    numbers.Enqueue(result);
                    displayResult.text = result.ToString();
                    flag = true;
                }
                storedOera = keyBoard.value;
                displayCalc.text += keyBoard.value;
                break;
            //bấm nút chức năng
            case KeyBoardInput.Type.function:
                //thực hiện các chức năng xóa, bật/tắt
                if (keyBoard.value == "C")
                {
                    if (!String.IsNullOrEmpty(input))
                    {
                        Clear();
                    }
                }
                else if (keyBoard.value == "ON")
                {
                    ActiveCalculator(true);
                }
                else
                {
                    ActiveCalculator(false);
                }
                break;
            //bấm phím số
            default:
                if(displayResult.text == "0" || flag) {
                    flag = false;
                    displayResult.text = "";
                }
                displayCalc.text += keyBoard.value;
                displayResult.text += keyBoard.value;
                input+=keyBoard.value;
                break;
        }
    }

    private float GetFloat(string value)
    {
        if(String.IsNullOrEmpty(value)) value = "0";
        return float.Parse(value, CultureInfo.GetCultureInfo("en-US"));
    }


    private float Calc(string opera, float number1, float number2)
    {
        switch (opera)
        {
            case "/":
                return number1 / number2;
            case "x":
                return number1 * number2;
            case "+":
                return number1 += number2;
            default:
                return number1 -= number2;
        }
    }

    private void Clear()
    {
        input = input.Remove(input.Length -1);
        displayCalc.text = displayCalc.text.Remove(displayCalc.text.Length - 1);
        displayResult.text = displayResult.text.Remove(displayResult.text.Length - 1);
    }

    private void ActiveCalculator(bool active)
    {
        if (active)
        {
            screen.color = Color.white;
        }
        else
        {
            displayResult.text = "";
            displayCalc.text = "";
            screen.color = Color.black;
        }

        onActive?.Invoke(active);
    }
}
