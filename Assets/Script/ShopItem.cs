using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using System.Globalization;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private MoneyType _moneyType;
    [FormerlySerializedAs("_text")] [SerializeField] private GameObject _textSoldOut;
    [FormerlySerializedAs("_text")] [SerializeField] private GameObject _textMoney;
    [SerializeField] private TMP_Text _textPrice;
    
    [SerializeField] private UnityEvent _buyEvent;

    void Start()
    {
        _textSoldOut.SetActive(false);
        _textMoney.SetActive(true);
        _textPrice.text = FormatNumber(_price);
    }
    
    public void BuyItem()
    {
        if (_price <= FindAnyObjectByType<GameManager>().Money)
        {
            if (_moneyType == MoneyType.normal)
                FindAnyObjectByType<GameManager>().Money -= _price;
            else
                FindAnyObjectByType<GameManager>().MoneyPrenium -= _price;

            GetComponent<Button>().interactable = false;
            _textSoldOut.SetActive(true);
            _textMoney.SetActive(false);
            _buyEvent?.Invoke();
        }
    }
    
    public string FormatNumber(int number)
    {
        if (number >= 1_000_000)
            return (number / 1_000_000f).ToString("0.#", CultureInfo.GetCultureInfo("fr-FR")) + "M";

        if (number >= 1_000)
            return (number / 1_000f).ToString("0.#", CultureInfo.GetCultureInfo("fr-FR")) + "k";

        return number.ToString();
    }
}

enum MoneyType
{
    normal,
    Prenium
}
