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

    public enum ControlScheme { WASD, Arrows, XboxController1, XboxController2 };
    public enum PlayerType { AI, Player, None };
    public PlayerData() {  }
    public PlayerData(int carType, ControlScheme ctrlScheme, PlayerType playerType)
    {
        _carType = carType;
        _ctrlScheme = ctrlScheme;
        _playerType = playerType;
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

    public void AttachGameObject(GameObject car) //Gives me an error if I leave this uncommented
    {
        _carObject = car;
        //if (_playerType == PlayerType.AI && _carObject.GetComponent<AIController>() == null)
        //{
        //    _carObject.name = _carObject.name + "(AI)";
        //    _carObject.AddComponent<AIController>();
        //}
    }
}

