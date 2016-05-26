using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using Entities;
using System.Net.Sockets;

namespace DataAccess
{
    public class DataAccess
    {
        // TODO: Refractor when it gets too complicated
        private StashEntities db = new StashEntities(); // DataAccess Component

        /// <summary>
        /// Returns all current boxes
        /// </summary>
        /// <author>Daniel Sacdalan</author>
        /// <returns>IEnumerable of box objects</returns>
        public IEnumerable<Box> Boxes(string searchString)
        {
            //return from b in db.Boxes
            //       where b.delete_date == null
            //       select b;
            IQueryable<Box> Boxes = from b in db.Boxes
                   where b.delete_date == null
                   select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                Boxes = Boxes.Where(s => s.name.Contains(searchString));
        }
            return Boxes;
        }

        /// <summary>
        /// Returns a queryable list of all current items
        /// </summary>
        /// <author>Daniel Sacdalan</author>
        /// <returns>List of item objects</returns>
        public IEnumerable<Item> CurrentItemDetails(string searchString)
        {
            //return  from i in db.Items
            //        where i.delete_date == null
            //        select i;
            IQueryable<Item> Items = from i in db.Items
                    where i.delete_date == null
                    select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                Items = Items.Where(s => s.name.Contains(searchString));
            }
            return Items;
        }

        /// <summary>
        /// Returns all items in a box 
        /// </summary>
        /// <author>Daniel Sacdalan</author>
        /// <returns></returns>
        public IEnumerable<BoxedItems> CurrentBoxedItems()
        {
            return from it in db.Itemtransactions
                   join i in db.Items on it.itemkey equals i.itemkey
                   where it.out_time == null && i.delete_date == null
                   select new BoxedItems() { Item = i, Boxkey = it.boxkey };
        }

        /// <summary>
        /// Returns the current list of item groups
        /// </summary>
        /// <author>Daniel Sacdalan</author>
        /// <returns>IEnumerable of itemgroup objects</returns>
        public IEnumerable<Itemgroup> CurrentGroupDetails()
        {
            return from g in db.Itemgroups
                   where g.delete_date == null
                   select g;
        }

        /// <summary>
        /// Returns all items that belong in a group
        /// </summary>
        /// <author>Daniel Sacdalan and Matt Lucrida</author>
        /// <returns>IEnumerable of Item objects</returns>
        public IEnumerable<Item> ItemsInGroup()
        {
            return from g in db.Itemgroups
                   join i in db.Items on g.itemgroupkey equals i.itemgroupkey
                   where i.itemgroupkey != null &&
                   g.delete_date == null
                   select i;
        }

        /// <summary>
        /// Update db with new item
        /// </summary>
        /// <author>Matt Lucrida</author>
        /// <param name="newItem"></param>
        public void AddNewItem(Item newItem)
        {
            db.Items.Add(newItem);
            db.SaveChanges();
        }

        public async Task<JsonBoxData> ScanBoxes()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://10.10.14.13:8081/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("scanOnce");
                    if (response.IsSuccessStatusCode)
                    {
                        DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JsonBoxData));
                        Stream s = await response.Content.ReadAsStreamAsync();
                        JsonBoxData boxes = (JsonBoxData)ser.ReadObject(s);
                        return boxes;
                    }
                }

                //catch (SocketException SE)
                catch (Exception E)
                {
                    Console.WriteLine(E);
                    return null;
                }
            }
            return null;
        }

        public void ScanStash(JsonBoxData CurrentItems)
        {
            //Get all boxeditems in one list
            string[] BoxedItems = CurrentItems.Boxes[0].TagIDs.Concat(CurrentItems.Boxes[1].TagIDs).Concat(CurrentItems.Boxes[2].TagIDs).ToArray();

            UpdateRemovedItems(CurrentItems, BoxedItems);
            UpdateReturnedItems(CurrentItems, BoxedItems);
        }

        public void UpdateReturnedItems(JsonBoxData CurrentItems, String[] BoxedItems)
        {
            //numBoxes IS FOR DEBUGGING SET TO 3 WHEN FINSIHED
            int numBoxes = 3;
            for (int i = 0; i < numBoxes; i++)
            {

                int curBoxKey = CurrentItems.Boxes[i].BoxID;

                //Look at all the items that are boxed
                foreach (var item in BoxedItems)
                {
                    //If there is any record with an out_time NULL,
                    //that means it is in the box.
                    bool Out = (from I in db.Items
                                join T in db.Itemtransactions on I.itemkey equals T.itemkey
                                where I.RFIDkey == item && T.out_time == null
                                select T).Any();

                    //If there is NO record with the out_time NULL,
                    //Then that means the item is being returned.
                    if (!Out)
                    {
                        //Find the item's itemkey
                        int curItemKey = (from I in db.Items
                                          where I.RFIDkey == item
                                          select I.itemkey).FirstOrDefault();

                        //Create new transaction record
                        Itemtransaction Transaction = new Itemtransaction
                        {
                            itemkey = curItemKey,
                            boxkey = curBoxKey,
                            in_time = DateTime.UtcNow,
                            out_time = null
                        };

                        //Save the changes to the db
                        try
                        {
                            db.Itemtransactions.Add(Transaction);
                            db.SaveChanges();
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    //else
                    //{
                    //    bool newRFID = ((from I in db.Items
                    //                     where I.RFIDkey == item
                    //                     select I).Any())
                    //                    ||
                    //                    ((from I in db.Items
                    //                      where I.RFIDkey == item && I.delete_date != null
                    //                      select I).Any());
                                        

                    //    if (newRFID)
                    //    {
                    //        Item newItem = new Item
                    //        {
                    //            RFIDkey = item,
                    //            name = "NEW ITEM",
                    //            object_description = null,
                    //            itemgroupkey = null,
                    //            photokey = null,
                    //            create_date = DateTime.UtcNow
                    //        };

                    //        try
                    //        {
                    //            db.Items.Add(newItem);
                    //            db.SaveChanges();
                    //        }

                    //        catch (Exception e)
                    //        {
                    //            Console.WriteLine(e);
                    //        }

                    //    }
                    //}
                }
            }
        }

        public void UpdateRemovedItems(JsonBoxData CurrentItems, string[] BoxedItems)
        {
            //Find all entries with NULL out times
            string[] RecordedInItems = (from I in db.Items
                                        join T in db.Itemtransactions on I.itemkey equals T.itemkey
                                        where T.out_time == null
                                        select I.RFIDkey).ToArray();

            //Returns all items currently out of stash
            IEnumerable<string> OutItems = RecordedInItems.Except(BoxedItems);

            //Update each transaction to show item was removed.
            foreach (var item in OutItems)
            {
                //Find the transaction to update
                var update = from Item in db.Items
                             join T in db.Itemtransactions on Item.itemkey equals T.itemkey
                             where Item.RFIDkey == item && T.out_time == null
                             select T;

                //Update the out_time
                //Will only loop once
                foreach (Itemtransaction It in update)
                {
                    It.out_time = DateTime.UtcNow;
                }

                //Save changes to db
                try
                {
                    db.SaveChanges();
                }

                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }


        }

        public Item GetItem(int id)
        {
            return db.Items.Find(id);
        }

        public void UpdateItem(Item item) {
            db.Entry(item).State = EntityState.Modified;  // ItemKey is always 0??
            db.SaveChanges();
        }

        public void DeleteItem(Item item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}