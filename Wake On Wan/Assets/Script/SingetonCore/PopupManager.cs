using System.Collections.Generic;
using UnityEngine;

public class PopupManager : SingletonObject<PopupManager>
{
    private int _totalPopupCount = 0;
    private Dictionary<PopupSortOrder, PopupDictionary> _dictionary;
    
    public int TotalPopupCount
    {
        get => _totalPopupCount;
    }

    public override void Awake()
    {
        base.Awake();
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        _dictionary = new Dictionary<PopupSortOrder, PopupDictionary>();
        foreach (PopupSortOrder order in System.Enum.GetValues(typeof(PopupSortOrder)))
        {
            _dictionary[order] = new PopupDictionary();
        }
    }
    
    public void AddPopup(SingletonPopup<Component> popup)
    {
        // Setup
        popup.canvas.sortingOrder = GetNewSortOrder(popup);
        popup.canvas.pixelPerfect = false;

        bool success = _dictionary[popup.popupOrder].AddPopup(popup);
        _totalPopupCount = success ? _totalPopupCount + 1 : _totalPopupCount;
    }

    public void RemovePopup(SingletonPopup<Component> popup)
    {
        bool success = _dictionary[popup.popupOrder].RemovePopup(popup);
        _totalPopupCount = success ? _totalPopupCount - 1 : _totalPopupCount;
        if (_totalPopupCount < 0) _totalPopupCount = 0;
    }

    public int GetPopupCountBySortOrder(PopupSortOrder sortOrder)
    {
        return _dictionary[sortOrder].GetPopupCount();
    }

    private int GetNewSortOrder(SingletonPopup<Component> popup)
    {
        return _dictionary[popup.popupOrder].GetNewSortOrder(popup);
    }
    
}

public class PopupDictionary
{
    private List<SingletonPopup<Component>> _listPopup = new List<SingletonPopup<Component>>();
    private int nextOrder = 0;
    
    public int GetPopupCount()
    {
        return _listPopup.Count;
    }

    public int GetNewSortOrder<T>(SingletonPopup<T> popup) where T : Component
    {
        return popup.canvas.sortingOrder + nextOrder;
    }
    
    public bool AddPopup<T>(SingletonPopup<T> popup) where T : Component
    {
        bool success = false;
        if (!_listPopup.Contains(popup as SingletonPopup<Component>))
        {
            _listPopup.Add(popup as SingletonPopup<Component>);
            nextOrder++;
            success = true;
        }

        return success;
    }
    
    public bool RemovePopup<T>(SingletonPopup<T> popup) where T : Component
    {
        bool success = false;
        if (!_listPopup.Contains(popup as SingletonPopup<Component>))
        {
            _listPopup.Remove(popup as SingletonPopup<Component>);
            if (_listPopup.Count == 0) nextOrder = 0;
            success = true;
        }

        return success;
    }
}