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
        private Transform _hotFixProgressPanel;
        private TMP_Text _tips;
        private TMP_Text _messageBoxContent;
        private Image _loadingProgressFG;
        private Image _loadingProgressPoint;
        private TMP_Text _loadingProgressText;
        private Action _clickMessageBoxOkCB;
        private Button _messageBoxOkBtn;
        private Button _messageBoxCancelBtn;

        private float _loadingProgressFGWidth;
        private float _loadingProgressPointYPos;

        internal void Initialize()
        {
            _messageBoxTrans = transform.Find("MessageBox");
            _hotFixProgressPanel = transform.Find("HotFixProgressPanel");
            _tips = _hotFixProgressPanel.Find("Tips").GetComponent<TMP_Text>();

            _messageBoxOkBtn = _messageBoxTrans.Find("messageBoxOkBtn").GetComponent<Button>();
            _messageBoxCancelBtn = _messageBoxTrans.Find("messageBoxCancelBtn").GetComponent<Button>();
            _messageBoxContent = _messageBoxTrans.Find("messageBoxContent").GetComponent<TMP_Text>();

            _loadingProgressFG = _hotFixProgressPanel.Find("LoadingProgressFG").GetComponent<Image>();
            _loadingProgressPoint = _hotFixProgressPanel.Find("LoadingProgressPoint").GetComponent<Image>();
            _loadingProgressText = _hotFixProgressPanel.Find("LoadingProgressText").GetComponent<TMP_Text>();

            _loadingProgressFGWidth = _loadingProgressFG.GetComponent<RectTransform>().sizeDelta.x;
            _loadingProgressPointYPos = _loadingProgressPoint.GetComponent<RectTransform>().sizeDelta.y;
            _loadingProgressText.text = "0%";
            _loadingProgressFG.fillAmount = 0;
            _loadingProgressPoint.transform.localPosition = new Vector3(-_loadingProgressFGWidth / 2, _loadingProgressPointYPos, 0);
            
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
            _messageBoxCancelBtn.onClick.AddListener(OnMessageBoxCancelBtnClicked);
            _hotFixProgressPanel.gameObject.SetActive(true);
            _messageBoxTrans.gameObject.SetActive(true);
            _messageBoxTrans.SetAsLastSibling();
        }

        public void UpdateTips(string content)
        {
            _tips.SetText(content);
        }

        public void UpdateSliderValue(float loadingProgress)
        {
            _loadingProgressText.text = (int)(loadingProgress * 100) + "%";
            _loadingProgressFG.fillAmount = loadingProgress;
            float positionLoadingProgressPoint = loadingProgress * _loadingProgressFGWidth - _loadingProgressFGWidth / 2;
            _loadingProgressPoint.transform.localPosition = new Vector3(positionLoadingProgressPoint, _loadingProgressPointYPos, 0);
        }

        private void OnMessageBoxOKBtnClicked()
        {
            _clickMessageBoxOkCB?.Invoke();
            _messageBoxTrans.gameObject.SetActive(false);
        }

        private void OnMessageBoxCancelBtnClicked()
        {
            _messageBoxContent.SetText("在测试服中，取消会无法进入游戏的哦");
        }
    }
}