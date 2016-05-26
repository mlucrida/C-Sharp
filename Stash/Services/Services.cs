using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using DataAccess;
using System.Collections;


namespace Services
{
    /// <summary>
    /// All functions regarding box models
    /// </summary>
    /// <author>Matt Lucrida</author>
    public class BoxServices
    {
        DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
        public BoxViewDTO BoxIndex(string searchString) {
            BoxViewDTO DTO = new BoxViewDTO();
            DTO.Boxes = dataAccess.Boxes(searchString);
            DTO.BoxedItems = dataAccess.CurrentBoxedItems();

            return DTO;
        }

        
    }

    /// <summary>
    /// All functions regarding item models
    /// </summary>
    /// <author>Matt Lucrida</author>
    public class ItemServices
    {
        private DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

        public Item InitializeItem() {
            Item newItem = new Item();
            newItem.itemgroupkey = null;
            newItem.photokey = null;
            // TODO: change to normal time
            newItem.create_date = DateTime.UtcNow;
            newItem.delete_date = null;
            return newItem;
        }
        public void AddNewItem(Item newItem) {
            dataAccess.AddNewItem(newItem);
        }
        public IEnumerable<Item> GetDetails(string searchString) {
            return dataAccess.CurrentItemDetails(searchString);
        }

        public Item GetItem(int id)
        {
            return dataAccess.GetItem(id);
        }

        public void UpdateItem(Item item) {
            dataAccess.UpdateItem(item);
        }

        public void DeleteItem(Item item)
        {
            //TODO: UPDATE OLD TRANSACTIONS AND SET OUT TO NULL
            item.RFIDkey = string.Empty;
            item.delete_date = DateTime.UtcNow;
            dataAccess.UpdateItem(item);
        }
    }

    /// <summary>
    /// All functions regarding item group models
    /// </summary>
    /// <author>Matt Lucrida</author>
    public class GroupServices
    {
        private DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();

        public GroupViewDTO GroupIndex()
        {
            GroupViewDTO DTO = new GroupViewDTO();

            DTO.CurrentItemGroups = dataAccess.CurrentGroupDetails();
            DTO.ItemsInGroup = dataAccess.ItemsInGroup();

            return DTO;
        }

    }
}
