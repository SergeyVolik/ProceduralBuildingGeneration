using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Rooms
{
    [System.Serializable]
    public class RoomLink
    {
        [SerializeField]
        private string _name;
        [SerializeField]
        private DoorType _doorType;
        [SerializeField]
        private string _to;
        [SerializeField]
        private string _from;


        public string Name { get => _name; }        
        public DoorType DoorType { get => _doorType; }      
        public string From { get => _from; }     
        public string To { get => _to; }

        public RoomLink(string name, DoorType door, string from, string to)
        {
            _name = name;
            _doorType = door;
            _from = from;
            _to = to;
        }
    }

}
