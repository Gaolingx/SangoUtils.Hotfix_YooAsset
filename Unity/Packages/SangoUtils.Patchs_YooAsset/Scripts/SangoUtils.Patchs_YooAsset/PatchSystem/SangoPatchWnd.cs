using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SangoUtils.Patchs_YooAsset
{
    public class SangoPatchWnd : MonoBehaviour
    {
        private SangoPatchRoot _sangoHotFixRoot;
        private Transform _messageBoxTrans;
        private TMP_Text _tips;

        private Action _clickMessageBoxOkCB;
        private Button _messageBoxOkBtn;
        private TMP_Text _messageBoxContent;

        internal void Initialize()
        {
            _messageBoxTrans = transform.Find("MessageBox");
            _tips = transform.Find("tips").GetComponent<TMP_Text>();

            _messageBoxOkBtn = _messageBoxTrans.Find("messageBoxOkBtn").GetComponent<Button>();
            _messageBoxContent = _messageBoxTrans.Find("messageBoxContent").GetComponent<TMP_Text>();

            _messageBoxTrans.gameObject.SetActive(false);
            _tips.SetText("欢迎使用热更新系统");
        }

        public void SetRoot(SangoPatchRoot root)
        {
            _sangoHotFixRoot = root;
        }

        public void ShowMessageBox(string content, Action onMessageBoxOKBtnClickedCB)
        {
            _messageBoxOkBtn.onClick.RemoveAllListeners();
            _messageBoxContent.SetText(content);
            _clickMessageBoxOkCB = onMessageBoxOKBtnClickedCB;
            _messageBoxOkBtn.onClick.AddListener(OnMessageBoxOKBtnClicked);
            _messageBoxTrans.gameObject.SetActive(true);
            _messageBoxTrans.SetAsLastSibling();
        }

        public void UpdateTips(string content)
        {
            _tips.SetText(content);
        }

        public void UpdateSliderValue(float value)
        {

        }

        private void OnMessageBoxOKBtnClicked()
        {
            _clickMessageBoxOkCB?.Invoke();
            _messageBoxTrans.gameObject.SetActive(false);
        }
    }
}