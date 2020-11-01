//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Linq;
//using UnityEngine;
//using Rooms;
//using System;
//using StraightSkeleton.Primitives;
//using DataStructures.PriorityQueue;
//

//namespace XML
//{
//    class XMLParser
//    {


//        public static void ParseFile(string filename, out List<RoomRequisite> rooms)
//        {

//            TextAsset txtXmlAsset = Resources.Load<TextAsset>(filename);
//            var doc = XDocument.Parse(txtXmlAsset.text);

//            var roomRequisites = doc.Element("Requisites").Element("RoomRequisites").Elements("Room");
//            rooms = new List<RoomRequisite>();
           
//            foreach (var roomReq in roomRequisites)
//            {
                              
                
//                var name = roomReq.Attribute("name").Value.ToString();               
//                var zoneType = SelectZoneType(roomReq.Attribute("zoneType").Value.ToString());
               
//                var roomType = SelectRoomType(roomReq.Attribute("roomType").Value.ToString());
//                var priority = Convert.ToInt32(roomReq.Attribute("priority").Value.ToString());

//                XElement Width = roomReq.Element("Width");
//                var str = Width.Attribute("min").Value.ToString();
                
//                var minWidth = Convert.ToInt32(Width.Attribute("min").Value.ToString());
//                var maxWidth = Convert.ToInt32(Width.Attribute("max").Value.ToString());

//                XElement Depth = roomReq.Element("Depth");
//                var minDepth = Convert.ToInt32(Depth.Attribute("min").Value.ToString());
//                var maxDepth = Convert.ToInt32(Depth.Attribute("max").Value.ToString());

//                XElement Area = roomReq.Element("Area");
//                var minArea = Convert.ToInt32(Area.Attribute("min").Value.ToString());
//                var maxArea = Convert.ToInt32(Area.Attribute("max").Value.ToString());

//                XElement Window = roomReq.Element("Window");
//                var needWindow = Convert.ToBoolean(Window.Attribute("required").Value.ToString());

                
//                var requisite = new RoomRequisite(
//                    maxWidth, minWidth, maxDepth, minDepth, minArea, maxArea, priority, zoneType, roomType, name
//                );

                           


//            }

         
//            var roomConnections = doc.Element("Requisites").Element("ConnectionRequisites").Elements("Connection");
//            foreach (var roomConA in roomConnections)
//            {
//                var linkName = roomConA.Attribute("name").Value.ToString();
//                var doorType = SelectDoorType(roomConA.Attribute("doorType").Value.ToString());


//                XElement LinkA = roomConA.Element("Link");

//                var from = rooms.Find(r => r.RoomName == LinkA.Attribute("a").Value.ToString());

               

//                foreach (var roomConB in roomConnections)
//                {
                    

//                    XElement LinkB = roomConB.Element("Link");


                   
//                    var to = rooms.Find(r => r.RoomName == LinkB.Attribute("b").Value.ToString() 
//                    && LinkB.Attribute("b").Value.ToString() == from.RoomName);

//                    //if(to!=null)
//                    //    from.RoomLinks.Add(new RoomLink(linkName, doorType, from.RoomName, to.RoomName));
//                }
                
//            }

//        }

//        private static ZoneType SelectZoneType(string str)
//        {
//            switch (str)
//            {
//                case "Public":
//                    return ZoneType.Public;
                 
//                case "Private":
//                    return ZoneType.Private;                                  
//            }

//            throw new InvalidOperationException("Error in parser { Invalid ZoneType name on xml file }");
//        }
//        private static DoorType SelectDoorType(string str)
//        {
//            switch (str)
//            {
//                case "Door":
//                    return DoorType.Door;

//                case "OpenWall":
//                    return DoorType.OpenWall;
//            }

//            throw new InvalidOperationException("Error in parser { Invalid DoorType name on xml file }");
//        }
//        private static RoomType SelectRoomType(string str)
//        {
//            switch (str)
//            {
//                case "Bathroom":
//                    return RoomType.Bathroom;
//                case "Bedroom":
//                    return RoomType.Bedroom;
//                case "Closet":
//                    return RoomType.Closet;
//                case "Corridor":
//                    return RoomType.Corridor;
//                case "Diningroom":
//                    return RoomType.Diningroom;
//                case "Garage":
//                    return RoomType.Garage;
//                case "Hall":
//                    return RoomType.Hall;
//                case "Kitchen":
//                    return RoomType.Kitchen;
//                case "Livingroom":
//                    return RoomType.Livingroom;
//                case "Porch":
//                    return RoomType.Porch;
//                case "Restroom":
//                    return RoomType.Restroom;
                

//            }

//            throw new InvalidOperationException("Error in parser { Invalid RoomType name on xml file }");
//        }
//    }
//}
