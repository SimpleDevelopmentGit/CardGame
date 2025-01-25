using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardsManager cardsManager;
    public bool available;
    public bool completed;
    public bool hasToHaveSameNumberOrColor;
    public int maxAmount;
    public int amountToComplete;

    public HolderType holderType;

    //All states our card can be in
    public enum HolderType
    {
        Play, Discard, CardTrader, MainHolder
    }

    private void Update()
    {
        HandleCardHolderFunctinallity();

        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            if (cardsManager.Cards.Contains(child.gameObject))
            {
                cardsManager.Cards.Remove(child.gameObject);
            }

            if (child.GetComponent<Card>())
            {
                child.GetComponent<Card>().CanDrag = false;
                child.GetComponent<Card>()._CardState = Card.CardState.Played;
            }
        }
    }

    public void HandleCardHolderFunctinallity()
    {
        if (holderType == HolderType.Play)
        {
            if (hasToHaveSameNumberOrColor)
            {
                if (cardsManager.SelectedCard != null && transform.childCount > 3)
                {
                    Debug.Log(cardsManager.SelectedCard.GetComponent<Card>().cardNumber);
                    Debug.Log(transform.GetChild(transform.childCount - 1).GetComponent<Card>().cardNumber);

                    if (cardsManager.SelectedCard.GetComponent<Card>().cardNumber == transform.GetChild(transform.childCount - 1).GetComponent<Card>().cardNumber ||
                        cardsManager.SelectedCard.GetComponent<Card>().cardType.CardIcon == transform.GetChild(transform.childCount - 1).GetComponent<Card>().cardType.CardIcon)
                    {
                        available = transform.childCount - 3 < maxAmount;
                    }
                    else
                    {
                        available = false;
                    }
                }
                else
                {
                    available = true;
                }
            }
            else
            {
                available = transform.childCount - 3 < maxAmount;
            }

            completed = transform.childCount - 3 == amountToComplete;
        }

        if (holderType == HolderType.CardTrader)
        {
            available = true;
        }

        if (holderType == HolderType.MainHolder)
        {
            available = true;
            completed = transform.childCount == amountToComplete;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(available)
            cardsManager.HoveringMenu = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (available)
            cardsManager.HoveringMenu = null;
    }
}
