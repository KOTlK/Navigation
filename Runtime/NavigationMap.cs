using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace Navigation.Runtime
{
    [ExecuteAlways]
    public class NavigationMap : MonoBehaviour
    {
        [SerializeField] private string _mapName = "TestMap";
        [SerializeField] private string _path = "Assets/Game/Runtime/TacticMaps/Navigation";
        [SerializeField] private Terrain _terrain;
        [SerializeField] private float _defaultCost = 1f;
        [SerializeField] private float _angle = 45f;

        [SerializeField] private bool _draw = false;
        [SerializeField] private bool _drawNeighbours = false;
        [SerializeField] private bool _drawCells = false;

        [SerializeField] private NavigationData _navigationData;

        private NavigationGraph _navigationGraph;

        private const string Postfix = "NavigationData";
        private const string Extension = ".asset";

        public NavigationData NavigationData => _navigationData;

        public NavigationData ReBuild()
        {
            GenerateNavData();
            return _navigationData;
        }

        public void GenerateNavData()
        {
            var terrainData = _terrain.terrainData;
            var scale = terrainData.heightmapScale;
            var size = new Vector2Int(terrainData.heightmapResolution, terrainData.heightmapResolution);
            var heights = terrainData.GetHeights(0, 0, size.x, size.y);
            var navigationNodes = new Vector3[size.x * size.y];
            var obstacles = FindObjectsOfType<NavigationObstacle>();

            for (var y = 0; y < size.y; y++)
            {
                for (var x = 0; x < size.x; x++)
                {
                    var h = heights[y, x];
                    var currentPosition = new Vector3(x * scale.x, h * scale.y, y * scale.z);

                    navigationNodes[y + x * size.x] = currentPosition;
                }

            }
            
            //Carve obstacles

            foreach (var obstacle in obstacles)
            {
                for (var y = 0; y < size.y; y++)
                {
                    for (var x = 0; x < size.x; x++)
                    {
                        var node = navigationNodes[y + x * size.x];

                        if (obstacle.Contains(node))
                        {
                            navigationNodes[y + x * size.x] = Vector3.negativeInfinity;
                        }
                    }
                }
            }

            //Generate cells

            var directions = new Vector2Int[]
            {
                new(1, 0),
                new(1, 1),
                new(0, 1)
            };

            var cells = new NavigationCell[(size.x - 1) * (size.y - 1)];

            for (var y = 0; y < size.y; y++)
            {
                if (y == size.y - 1)
                    break;
                
                for (var x = 0; x < size.x; x++)
                {
                    if (x == size.x - 1)
                        break;
                    
                    var vertices = new List<Vector3>();

                    vertices.Add(navigationNodes[y + x * size.x]);


                    foreach (var direction in directions)
                    {
                        var nextX = x + direction.x;
                        var nextY = y + direction.y;
                        
                        if(nextX >= size.x)
                            continue;
                        
                        if(nextX < 0)
                            continue;
                        
                        if(nextY >= size.y)
                            continue;
                        
                        if(nextY < 0)
                            continue;

                        vertices.Add(navigationNodes[nextY + nextX * size.x]);
                    }

                    var center = Vector3.zero;

                    foreach (var vertex in vertices)
                    {
                        center += vertex;
                    }

                    center /= vertices.Count;

                    var cell = new NavigationCell()
                    {
                        PositionInGraph = new Vector2Int(x, y),
                        Position = center,
                        Cost = _defaultCost,
                        Vertices = vertices.ToArray()
                    };

                    cells[y + x * (size.x - 1)] = cell;
                }
            }

            //Apply costs
            var areas = FindObjectsOfType<PassableNavigationArea>();

            foreach (var area in areas)
            {
                for (var i = 0; i < cells.Length; i++)
                {
                    var cell = cells[i];
                    
                    if(area.Contains(cell.Position))
                    {
                        cells[i].Cost = area.Cost;
                    }
                }
            }
            
            

            //Save data
            var fullPath = _path + "/" + _mapName + Postfix + Extension;

            if (Directory.Exists(_path) == false)
            {
                Directory.CreateDirectory(_path);
            }

            if (_navigationData != null)
            {
                AssetDatabase.DeleteAsset(_path + _navigationData.name + Extension);
                AssetDatabase.SaveAssets();
                _navigationData = null;
            }
            
            var navData = ScriptableObject.CreateInstance<NavigationData>();
            navData.Cells = cells;
            navData.Size = new Vector2Int(size.x - 1, size.y - 1);
            navData.Scale = scale;

            AssetDatabase.CreateAsset(navData, fullPath);
            AssetDatabase.SaveAssets();
            _navigationData = navData;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_draw == false)
                return;

            if (_navigationGraph == null)
            {
                _navigationGraph = new NavigationGraph(_navigationData, _angle);
            }

            _navigationGraph.Visualize(_drawNeighbours, _drawCells);
        }
    }
}