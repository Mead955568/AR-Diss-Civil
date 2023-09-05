using TMPro;
using UnityEngine;

public class SetUiText : MonoBehaviour {

    [SerializeField]
    private TMP_Text _textField;
    [SerializeField]
    private string _fixedText;

    public void OnSliderValueChanged(float numericValue) {
        _textField.text = $"{_fixedText}: {numericValue}";
    }
}
