using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerData
{
    private int _carType;
    private ControlScheme _ctrlScheme;
    private PlayerType _playerType;
    private GameObject _carObject;
    private GameObject _prefab;
    private int _lives;
    private int ID;

    public enum ControlScheme { WASD, Arrows, XboxController1, XboxController2, NotAssigned };
    public enum PlayerType { AI, Player, None };
    public PlayerData() {  }
    public PlayerData(int id, int carType, ControlScheme ctrlScheme, PlayerType playerType)
    {
        _carType = carType;
        _ctrlScheme = ctrlScheme;
        _playerType = playerType;
        _lives = 10;
        ID = id;
    }

    public int getID() {

        return ID;
    }

    public bool IsAI()
    {
        return _playerType == PlayerType.AI;
    }

    public ControlScheme GetControlScheme()
    {
        return _ctrlScheme;
    }

    public PlayerType GetPlayerType()
    {
        return _playerType;
    }

    public int GetCarType()
    {
        return _carType;
    }

    public int getLives()
    {
        return _lives;
    }

    public void reduceLives()
    {
        _lives--;
    }

    public void AttachGameObject(GameObject car)
    {
        _carObject = car;
        if (_playerType == PlayerType.AI && _carObject.GetComponent<AIController>() == null)
        {
            _carObject.name = _carObject.name + "(AI)";
            _carObject.AddComponent<AIController>();
        }
    }

    public void AttachPrefab(GameObject prefab)
    {
        _prefab = prefab;
    }

    public GameObject GetPrefab()
    {
        return _prefab;
    }

    public GameObject GetGameObject()
    {
        return _carObject;
    }
}

