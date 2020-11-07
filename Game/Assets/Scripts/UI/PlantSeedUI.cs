using UnityEngine;

namespace UI
{
    public class PlantSeedUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_content;
        [SerializeField] private GameObject m_seedRowPrefab;

        public void Show(Field targetField)
        {
            this.ClearCurrentContent();
            foreach (var seed in PlayerController.Instance.PlayerInventory.Seeds)
            {
                var seedRowUI = Instantiate(this.m_seedRowPrefab, this.m_content.transform).GetComponent<PlantSeedRowUI>();
                seedRowUI.ShowSeed(seed, targetField);
            }

            this.gameObject.SetActive(true);
        }

        public void CloseUi()
        {
            this.gameObject.SetActive(false);
        }

        private void ClearCurrentContent()
        {
            foreach (Transform child in this.m_content.transform)
                Destroy(child.gameObject);
        }
    }
}