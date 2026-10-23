using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using System.Globalization;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private float _price;
    [SerializeField] private MoneyType _moneyType;
    [SerializeField] private MoneyType _moneyToBuy;
    [SerializeField] private int _getPrice;
    [SerializeField] private GameObject _textSoldOut;
    [SerializeField] private GameObject _textMoney;
    [SerializeField] private TMP_Text _textPrice;
    
    [SerializeField] private UnityEvent _buyEvent;

    void Start()
    {
        _textSoldOut.SetActive(false);
        _textMoney.SetActive(true);
        if (_moneyType == MoneyType.real)
        {
            _textPrice.text = _price + " €";
        }
        else
        {
            _textPrice.text = FormatNumber((int)_price);
        }
    }
    
    public void BuyItem()
    {
        switch (_moneyType)
        {
            case MoneyType.normal:
                if (_price <= FindAnyObjectByType<GameManager>().Money)
                    FindAnyObjectByType<GameManager>().Money -= (int)_price;
                else
                {
                    FindAnyObjectByType<MenuManager>()?.GoToPosition(4);
                    return;
                }
                break;
            case MoneyType.Prenium:
                if (_price <= FindAnyObjectByType<GameManager>().MoneyPrenium)
                    FindAnyObjectByType<GameManager>().MoneyPrenium -= (int)_price;
                else
                {
                    FindAnyObjectByType<MenuManager>()?.GoToPosition(4);
                    return;
                }
                break;
            case MoneyType.real:
                if (_moneyToBuy != MoneyType.normal)
                {
                    int m = FindAnyObjectByType<GameManager>().Money + _getPrice;
                    if (m != null)
                        FindAnyObjectByType<GameManager>().Money = m;
                }
                else if (_moneyToBuy != MoneyType.Prenium)
                {
                    int m = FindAnyObjectByType<GameManager>().MoneyPrenium + _getPrice;
                    if (m != null)
                        FindAnyObjectByType<GameManager>().MoneyPrenium = m;
                }
                
                break;
        }
        
        GetComponent<Button>().interactable = false;
        _textSoldOut.SetActive(true);
        _textMoney.SetActive(false);
        _buyEvent?.Invoke();
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
    Prenium,
    real
}
