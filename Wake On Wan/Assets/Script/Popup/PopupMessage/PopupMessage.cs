using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupMessage : SingletonPopup<PopupMessage>
{
    [SerializeField] private TextMeshProUGUI txtTitle;
    [SerializeField] private TextMeshProUGUI txtMessage;

    private Action _onClickOk;
    private Action _onClickCancel;

    public void ShowMessage(string msg, Action onClickOk = null, Action onClickCancel = null)
    {
        ShowMessage("Notice", msg);
    }
    
    public void ShowMessage(string title, string msg, Action onClickOk = null, Action onClickCancel = null)
    {
        Show();
        _onClickOk = onClickOk;
        _onClickCancel = onClickCancel;
        txtTitle.text = title;
        txtMessage.text = msg;
    }

    public void OnClickOk()
    {
        _onClickOk?.Invoke();
        Hide();
    }

    public void OnClickCancel()
    {
        _onClickCancel?.Invoke();
        Hide();
    }
}
