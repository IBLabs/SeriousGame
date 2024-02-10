using TMPro;
using UnityEngine;

namespace Common.Scripts
{
    public class CounterController : MonoBehaviour
    {
        public int value = 15; // The value to display
        public int leadingZeros = 3; // Number of leading zeros
        public TextMeshProUGUI textDisplay; // Reference to the TextMeshProUGUI component

        void Start()
        {
            UpdateDisplay(); // Update the display on start
        }

        // Method to update the displayed value
        void UpdateDisplay()
        {
            if (textDisplay != null)
            {
                // Format the value with leading zeros based on the leadingZeros variable
                string formattedValue = value.ToString($"D{leadingZeros}");
                textDisplay.text = formattedValue;
            }
            else
            {
                Debug.LogError("TextMeshProUGUI reference not set.");
            }
        }

        // Example method to increment the value and update the display
        public void IncrementValue()
        {
            value++;
            UpdateDisplay();
        }

        // Example method to decrement the value and update the display
        public void DecrementValue()
        {
            value--;
            UpdateDisplay();
        }

        // Example method to set the value directly
        public void SetValue(int newValue)
        {
            value = newValue;
            UpdateDisplay();
        }
    }
}