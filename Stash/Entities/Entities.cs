using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Entities
{
    /// <summary>
    /// New entity that has all item details that are in a box
    /// and contains the box key.
    /// </summary>
    /// <author>Daniel Sacdalan and Matt Lucrida</author>
    public class BoxedItems
    {
        public Item Item {get; set;}
        public int Boxkey { get; set; }
    }

    public class TransactionItems
    {
        public Itemtransaction Transaction { get; set; }
        public string RFIDKey { get; set; }
    }

    public class JsonBoxData
    {
        public int NumBoxes { get; set; }

        public CurrentBox[] Boxes { get; set; }
    }

    public class CurrentBox
    {
        public int BoxID;

        public string[] TagIDs;
    }

    /// <summary>
    /// DTO for the boxview
    /// </summary>
    /// <author>Daniel Sacdalan and Matt Lucrida</author>
    public class BoxViewDTO
    {
        public IEnumerable<Box> Boxes { get; set; }
        public IEnumerable<BoxedItems> BoxedItems {get; set;}
    }

    /// <summary>
    /// DTO for groupview
    /// </summary>
    /// <author>Daniel Sacdalan</author>
    public class GroupViewDTO
    {
        public IEnumerable<Itemgroup> CurrentItemGroups { get; set; }
        public IEnumerable<Item> ItemsInGroup{ get; set; }
    }
}
