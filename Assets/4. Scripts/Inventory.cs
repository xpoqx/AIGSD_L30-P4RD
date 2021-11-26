using UnityEngine;

namespace _4._Scripts
{
    // 이후 수정할 것, UIManager.cs
    public class Inventory : MonoBehaviour
    {
        public bool isOpen;
        [SerializeField] private GameObject myInventory;

        private void Start()
        {
            isOpen = false;
            myInventory.SetActive(false);
        }

        public void ManipulateWindow()
        {
            if (isOpen) Close();
            else Open();
        }

        private void Open()
        {
            if (isOpen) return;
            isOpen = true;
            myInventory.SetActive(true);
        }

        private void Close()
        {
            if (!isOpen) return;
            isOpen = false;
            myInventory.SetActive((false));
        }
    }
}
