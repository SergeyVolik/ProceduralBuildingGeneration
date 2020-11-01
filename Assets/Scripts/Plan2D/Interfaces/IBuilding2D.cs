using Floor;
using StraightSkeleton;
using StraightSkeleton.Primitives;
using System.Collections.Generic;
/* любой дом обяз комнаты - кухня, спальня(1), зала, уборная
* частный дом (1-3 этажа) обяз комнаты - кухня, спальня(1), зала, уборная, 
* многоэтажный дом(квартирный) - любой дом + гараж + доп спальня + доп уборная, 
* склад - основная комата склада + офис + уборная + комната для инструментов,  
*/
[System.Serializable]
public enum BuildingType {  PrivateHouse, ApartmentPanelHouse  };

public interface IBuilding2D
{

    int NumberOfFloors { get; set; }
    float Area { get; set; }
    int Angles { get; set; }
    
    List<Entrance2D> Entraces { get; set; }
    Floor2D mainRoof { get; set; }    
    List<Vector2d> MainPolygon { get; set; }


}