using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MonologueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_monologueText;
        [SerializeField] private float m_autoCloseTimer;

        private Coroutine m_currentMonologueCoroutine;
        
        public void ShowMonologue(string text)
        {
            if (this.m_currentMonologueCoroutine != null)
                StopCoroutine(this.m_currentMonologueCoroutine);
            
            this.m_monologueText.text = text;
            this.gameObject.SetActive(true);
            this.m_currentMonologueCoroutine = StartCoroutine(this.AutoCloseMonologue());
        }

        private IEnumerator AutoCloseMonologue()
        {
            yield return new WaitForSeconds(this.m_autoCloseTimer);
            this.gameObject.SetActive(false);
            this.m_currentMonologueCoroutine = null;
        }

        public void CloseUI()
        {
            if (this.m_currentMonologueCoroutine != null)
            {
                StopCoroutine(this.m_currentMonologueCoroutine);
                this.m_currentMonologueCoroutine = null;
            }
            this.gameObject.SetActive(false);
        }
    }
}