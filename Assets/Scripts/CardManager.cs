using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Card _cardSO;
    public Card CardSO { get { return _cardSO; } set { _cardSO = value; } }

    private GameObject _draggingBuilding;
    private Building _building;

    private Vector2Int _gridSize = new Vector2Int(15, 10);
    private bool _isAvailableToBuild;

    private GridController _gridController;

    private void Awake()
    {
        _gridController = GridController.Instance;
        _gridController.Grid = new Building[_gridSize.x, _gridSize.y];
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_draggingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float pos))
            {
                Vector3 worldPosition = ray.GetPoint(pos);
                int x = Mathf.RoundToInt(worldPosition.x);
                int z = Mathf.RoundToInt(worldPosition.z);

                if (x < 0 || x > _gridSize.x - _building.BuildingSize.x)
                    _isAvailableToBuild = false;
                else if (z < 0 || z > _gridSize.y - _building.BuildingSize.y)
                    _isAvailableToBuild = false;
                else
                    _isAvailableToBuild = true;

                if (_isAvailableToBuild && IsPlaceTaken(x, z)) _isAvailableToBuild = false;
                // if ((z % 2 == 1) || (x % 2 == 1)) _isAvailableToBuild = false;

                _draggingBuilding.transform.position = new Vector3(x, 0, z);

                _building.SetColor(_isAvailableToBuild);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _draggingBuilding = Instantiate(_cardSO.Prefab, Vector3.zero, Quaternion.identity);

        _building = _draggingBuilding.GetComponent<Building>();

        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float pos))
        {
            Vector3 worldPosition = ray.GetPoint(pos);
            int x = Mathf.RoundToInt(worldPosition.x);
            int z = Mathf.RoundToInt(worldPosition.z);

            _draggingBuilding.transform.position = new Vector3(x, 0, z);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isAvailableToBuild)
            Destroy(_draggingBuilding);
        else
        {
            _gridController.Grid[(int)_draggingBuilding.transform.position.x,
                (int)_draggingBuilding.transform.position.z] = _building;

            _building.ResetColor();
        }
    }

    private bool IsPlaceTaken(int x, int z)
    {
        if (_gridController.Grid[x, z] != null)
            return true;
        return false;
    }
}
