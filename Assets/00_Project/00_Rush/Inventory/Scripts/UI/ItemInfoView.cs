using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rush
{
    public partial class ItemInfoView : UIView
    {
        [SerializeField]
        private AssetReferenceGameObject m_ItemInfoViewAsset;
        [SerializeField]
        private Transform m_ItemViewSpawn;
        [SerializeField, ReadOnly]
        private ItemView m_ItemView;

        [SerializeField]
        private TextMeshProUGUI m_NameText;
        [SerializeField]
        private TextMeshProUGUI m_DescriptionText;

        public void SetItemView(IItem item)
        {
            ShowInternal();
            CoroutineUtility.BeginCoroutine($"{GetInstanceID()}/{nameof(SettingItemView)}", SettingItemView(item));
        }

        private IEnumerator SettingItemView(IItem item)
        {
            if (m_ItemView == null)
            {
                if (!AddressableUtility.HasPrefab(m_ItemInfoViewAsset.AssetGUID))
                {
                    yield return AddressableUtility.LoadingPrefab(m_ItemInfoViewAsset.AssetGUID);
                }
                else
                {
                    ItemView prefab = AddressableUtility.GetPrefab<ItemView>(m_ItemInfoViewAsset.AssetGUID);
                    m_ItemView = Instantiate(prefab, m_ItemViewSpawn, false);
                }
            }
            
            m_ItemView.Show();
            m_ItemView.SetUp(item);
            SetNameAndDescriptionText(item);
        }

        private void SetNameAndDescriptionText(IItem item)
        {
            m_NameText.text = item.Label;
            m_DescriptionText.text = item.Description;
        }
    }
}
