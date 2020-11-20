using Assets.Scripts.Premies.Buildings.Building2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Builders
{
    public interface IBuilding3D
    {
        GameObject BuildingRoot { get; set; }
      
        List<RoomSetting> RoomsSettings { get; }
    

    }
}
