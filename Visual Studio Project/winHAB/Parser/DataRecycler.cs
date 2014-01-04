using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_Parser.Parser
{
    public class DataRecycler : JsonOpenHABDataContract
    {
        public LinkedList<Widget> floor { get; private set; }
        public Queue<Widget> rooms { get; private set; }
        public Queue<Widget> widgets { get; private set; }
        public Queue<Item> items { get; private set; }
        public Int32 floor_counter { get; private set; }
        public Int32 room_counter { get; private set; }
        public LinkedList<Floor> floors { get; set; }
        public static Dictionary<String, String> iconDictionary { get; set; }
        public DataRecycler()
        {
            floor = new LinkedList<Widget>();
            rooms = new Queue<Widget>();
            widgets = new Queue<Widget>();
            items = new Queue<Item>();
            floors = new LinkedList<Floor>();
            iconDictionary = new Dictionary<string, string>();
        }

        //public void readRooms(JsonOpenHABDataContract smartHomeDataContract)
        //{
        //    /*
        //     * Reads the Floors
        //     */
        //    floor = new LinkedList<Widget>();
        //    Widget master_floor;//Contains the Room-Widget wich contains all the Rooms of the house
        //    Widget[] master_floor_array;//Simply an Array to convert the LinkedList Array for traversal
        //    master_floor = smartHomeDataContract.homepage.widget.First.Value;
        //    master_floor_array = master_floor.widget.ToArray();
        //    floor_counter = 0;
        //    Floor buffer;
        //    LinkedListNode<Widget> first = master_floor_array[floor_counter].linkedPage.widget.First;
        //    while (floor_counter != master_floor.widget.Count)
        //    {
        //        //floors.AddLast(new Floor(master_floor_array[floor_counter].linkedPage.widget
        //        floor.AddLast(master_floor_array[floor_counter]);//Reads each segment of the Master_Floor and enqueues it  
        //        if (!iconDictionary.ContainsKey((master_floor_array[floor_counter].icon)))
        //        {
        //            iconDictionary.Add(master_floor_array[floor_counter].icon, "Assets/images/" + master_floor_array[floor_counter].icon + ".png");
        //        }
        //        if (!iconDictionary.ContainsKey((master_floor_array[floor_counter].linkedPage.icon)))
        //        {
        //            iconDictionary.Add(master_floor_array[floor_counter].linkedPage.icon, "Assets/images/" + master_floor_array[floor_counter].linkedPage.icon + ".png");
        //        }
        //        while (first != null)
        //        {
        //            if (!iconDictionary.ContainsKey((first.Value.icon)))
        //            {
        //                iconDictionary.Add(first.Value.icon, "Assets/images/" + first.Value.icon + ".png");
        //            }
        //            first = first.Next;
        //        }
        //        floor_counter++;
        //        if (floor_counter < master_floor_array.Length)
        //            first = master_floor_array[floor_counter].linkedPage.widget.First;
        //    }
        //    //#####################################################################################################################
        //    /**
        //     * Reads the Rooms --> Bath, Kitchen, Living Room etc.
        //     */
        //    room_counter = 0;
        //}


        public class Floor//<Widget>
        {
            public LinkedList<Widget> rooms { get; set; }
            public Floor(LinkedList<Widget> rooms)
            {
                this.rooms = rooms;
            }
        }




        public class Room//<Widget>
        {
            public Room()
            {
            }
        }
    }
}
