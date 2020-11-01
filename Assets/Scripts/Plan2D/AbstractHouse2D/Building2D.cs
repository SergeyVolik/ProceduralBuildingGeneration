using ArchitectureGrid;
using Assets.Scripts.Plan2D.ArchitectureGrid.PlanProcessor2D;
using Assets.Scripts.Premies.Buildings.Floors;
using Floor;
using StraightSkeleton;
using StraightSkeleton.Corridor;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Premies.Buildings.Building2D
{
    public abstract class Polygon
    {
        public List<Vector2d> MainPolygon { get; set; }
        public List<Vector2d> BuildingForm { get; set; }

        protected Polygon(List<Vector2d> _OutsidePolygon)
        {
            MainPolygon = _OutsidePolygon;
            
        }
    
        protected Polygon()
        {
            MainPolygon = new List<Vector2d>();
            BuildingForm = new List<Vector2d>();
        }
    }
    public abstract class Building2D : Premises2D, IBuilding2D
    {
       

        public const float FloorHight = 2.5f;

        

        #region Protected fields

        protected int _numberOfFloors;
        protected int _angles;
        protected float _area;

        protected Building2D(int numberOfFloors, float area, List<Vector2d> outerPolygon)
        {
            NumberOfFloors = numberOfFloors;         
            Area = area;
            MainPolygon = outerPolygon;

            planProcessor2D = new BuildingPlanProcessor(outerPolygon, outerPolygon, Vector2d.Empty);
            planProcessor2D.CreatePlan();
        }


        #endregion

        #region Public Props

        public int NumberOfFloors { 
            get=> _numberOfFloors; 
            set 
            {
                if (value <= 0)
                    throw new ArgumentException("Invalid value: _numberOfFloors can't set negative number!");
                else _numberOfFloors = value;
            } 
        }
       
        public int Angles { 
            get=> _angles; 
            set 
            {
                if (value % 2 != 0 || value < 4)
                    throw new ArgumentException("Invalid value: _angels can't set " +
                        "incorect number! need if(value >= 4 and value % 2 == 0)");
                else _angles = value;
            }
        }
       
        public float Area
        {
            get => _area;
            set
            {
                if (value < 300 || value > 1000)
                    throw new ArgumentException("Invalid value: input correct area: area < 300 and area > 1000");
                else _area = value;
            }
        }

        #endregion

        protected List<Entrance2D> entrances;
        public Floor2D mainRoof { get; set; }
        public List<Entrance2D> Entraces { get => entrances; set => entrances = value; }

        public List<PartOfWall> GetPartOfBuilding()
        {
            return planProcessor2D.Rooms[0].Walls;
        }
    }
}
