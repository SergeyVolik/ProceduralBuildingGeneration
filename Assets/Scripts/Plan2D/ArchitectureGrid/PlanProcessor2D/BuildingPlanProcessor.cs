using ArchitectureGrid;

using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Plan2D.ArchitectureGrid.PlanProcessor2D
{
    public class BuildingPlanProcessor : BasePlanProcessor2D
    {
        public BuildingPlanProcessor(List<Vector2d> polygon, List<Vector2d> buildingForm, Vector2d exit) : base(polygon, buildingForm, exit)
        {                     
          
        }
        public override void CreatePlan()
        {
            var building = new Room2D(RoomType.Building, "Building");
            Rooms.Add(building);

            for (var i = 0; i < Grid.GetLength(0); i++)
                for (var j = 0; j < Grid.GetLength(1); j++)
                    building.AddCell(Grid[i, j]);
        }
    }
}
