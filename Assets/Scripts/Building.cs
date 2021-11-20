using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Vector2 _buildingSize;
    [SerializeField] private Renderer _renderer;

    private Color originalColor = Color.black;

    public Vector2 BuildingSize { get { return _buildingSize; } private set {; } }

    public void SetColor(bool isAvailableToBuild)
    {
        if (isAvailableToBuild)
            _renderer.material.color = Color.green;
        else
            _renderer.material.color = Color.red;
    }

    public void ResetColor()
    {

        _renderer.material.color = Color.black;
    }
}
