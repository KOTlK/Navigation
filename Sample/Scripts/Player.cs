using System;
using Navigation.Runtime;
using Navigation.Runtime.Algorithms;
using UnityEngine;

namespace Navigation.Sample.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private NavigationMap _navigation;

        private PathFinding _pathFinding;
        private NavigationPath _path;
        private int _point;

        private void Awake()
        {
            NavigationGraph graph;
            
            _pathFinding = new PathFinding(
                graph = new NavigationGraph(
                    _navigation.NavigationData,
                    45f),
                new AStar(
                    graph,
                    new DefaultHeuristic()));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    if (_pathFinding.CanReach(hit.point))
                    {
                        _path = _pathFinding.FindPath(_character.transform.position, hit.point);
                    }
                }
            }

            if (_path.Corners != null)
            {
                var direction = _path.Corners[_point] - _character.transform.position;
                direction.y = 0;
                if (direction.sqrMagnitude <= 0.1f)
                {
                    _point++;
                    direction = _path.Corners[_point] - _character.transform.position;
                    direction.y = 0;
                }

                if (_point >= _path.Corners.Length - 1)
                    return;

                _character.Move(direction);
            }
        }
    }
}