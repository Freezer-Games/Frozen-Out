using Scripts;

namespace Scripts.Level.Item
{
    public abstract class InventoryUIController : UIController<ItemInfo>
    {
        protected override bool IsOpen => base.IsOpen && StoredItem != null;
        protected ItemInfo StoredItem;

        public override void Open()
        {
            Open(null);
        }
        
        public override void Open(ItemInfo passingItem)
        {
            base.Open();

            StoredItem = passingItem;
        }

        public override void Close()
        {
            base.Close();

            StoredItem = null;
        }
    }
}