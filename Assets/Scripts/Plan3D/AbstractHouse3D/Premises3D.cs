using ArchitectureGrid;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Premies.Buildings.Building2D;
using Rooms;
using StraightSkeleton.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// класс который определяет действия общие для всех помещений
    /// </summary>
    public abstract class Premises3D : UnityEngine.Object, IVisualizer, IPremies3D
    {
        protected RoomSetting premisesPrefabs;
        public RoomSetting PremisesPrefabs { get => premisesPrefabs; set => premisesPrefabs = value; }

        public Premises3D(RoomSetting _premisesPrefabs)
        {
            premisesPrefabs = _premisesPrefabs;
        }
        public Premises3D()
        {

        }


        protected float FindWallRotation(PartOfWall wall, out float xOffset, out float yOffset, float maxOffset = 0.1f, bool moveDiractionInside = true)
        {

            var point1 = new Vector3((float)wall.V1.X, 0, (float)wall.V1.Y);
            var point2 = new Vector3((float)wall.V2.X, 0, (float)wall.V2.Y);

            var vector = Vector3.ClampMagnitude(point2 - point1, 1);

            float direction = 1;

            if (!moveDiractionInside)
                direction = -1;

            if (Vector3.Dot(vector, Vector3.forward) == 1)
            {
                yOffset = 0f;
                xOffset = maxOffset * direction;
                return 90;

            }

            else if (Vector3.Dot(vector, Vector3.forward) == -1)
            {
                yOffset = 0f;
                xOffset = -maxOffset * direction;
                return 270;
            }

            else if (Vector3.Dot(vector, Vector3.right) == 1)
            {
                yOffset = -maxOffset * direction;
                xOffset = 0;
                return 180;
            }

            else
            {
                yOffset = maxOffset * direction;
                xOffset = 0;
                return 0;
            }


        }


        public abstract void Visualize();


        public void InstantiateWallPrefab(PartOfWall partWall, GameObject prefab, Transform parent, Transform buildingTransform, int floor, Vector3 additionalOffset, float rotationPosOffset, bool moveDirectionInside)
        {
            float xOffset;
            float zOffset;
            var rotationY = FindWallRotation(partWall, out xOffset, out zOffset, rotationPosOffset, moveDirectionInside);
            var center = LineSegment2d.Center(partWall.V1, partWall.V2);


            var position = new Vector3((float)center.X + xOffset, Building2D.FloorHight * floor, (float)center.Y + zOffset) + buildingTransform.position + additionalOffset;

            var curRainDrain = Instantiate(prefab, position, Quaternion.identity);

            curRainDrain.transform.SetParent(parent);

            curRainDrain.transform.rotation = Quaternion.Euler(curRainDrain.transform.rotation.x, curRainDrain.transform.rotation.y + rotationY, curRainDrain.transform.rotation.z);
            curRainDrain.transform.localPosition += additionalOffset;
        }

        public void InstacntiateAngleObject(Angle angle, GameObject prefab, Transform parent, Transform buildingTransform, int floor, float YOffset = -0.2f)
        {
            float xOffset;
            float zOffset;
            var anglePoint = angle.AnglePoint;

            var position = new Vector3((float)anglePoint.X, Building2D.FloorHight * floor, (float)anglePoint.Y) + buildingTransform.position;

            

            FindWallRotation(angle.FirstWall, out xOffset, out zOffset, 0.1f, false);
            position += new Vector3(xOffset, 0, zOffset);

            FindWallRotation(angle.SecondWall, out xOffset, out zOffset, 0.1f, false);
            position += new Vector3(xOffset, YOffset, zOffset);
           
            var curRainDrain = Instantiate(prefab, position, Quaternion.identity);

            curRainDrain.transform.SetParent(parent);
        }
    }
}
