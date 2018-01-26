using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Setup", menuName = "Custom Inputs", order =1)]
[Serializable]
public class InputSetup : ScriptableObject
{
    public JoystickAxis[] joystickAxis;
    public Button[] buttons;
    string[] axisNames = { "LX", "LY", "RX", "RY", "L2", "R2", "DPadX", "DPadY" };
    string[] buttonNames = { "Circle", "Cross", "Square", "Triangle", "L1", "R1", "L2", "R2", "Share", "Options", "L3", "R3","PS", "TouchPad" };


    void Awake()
    {
        joystickAxis = new JoystickAxis[8];
        for (int i = 0; i < joystickAxis.Length; i++)
        {
            joystickAxis[i] = new JoystickAxis() { name = axisNames[i],axis = i+1 , joyNum = 0} ;
        }
        buttons = new Button[buttonNames.Length];
        for (int i=0;i<buttons.Length;i++)
        {
            buttons[i] = new Button() { name = buttonNames[i],positiveButton = "joystick + button " + i, joyNum = 0 };
        }
    }

    [Serializable]
    public class JoystickAxis
    {
        public JoystickAxis()
        {
            name = "Joystick Axis";
            gravity = 3;
            dead = 0.001f;
            sensitivity = 3;
            axisType = AxisType.JoystickAxis;
        }
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;

        public float gravity = 3;
        public float dead = 0.001f;
        public float sensitivity = 3;

        public bool snap = false;
        public bool invert = false;
        [Tooltip("KeyOrMouseButton = 0, MouseMovement = 1, JoystickAxis = 2")]
        public AxisType axisType; //KeyOrMouseButton = 0, MouseMovement = 1, JoystickAxis = 2

        public int axis = 0;
        public int joyNum=0;
    }

    [Serializable]
    public class Button
    {
        public Button()
        {
            gravity = 1000;
            dead = 0.001f;
            sensitivity = 1000;
            axisType = AxisType.KeyOrMouseButton;
            axis = 0;
            name = "button";
            joyNum = 0;
        }
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;

        public float gravity = 1000;
        public float dead = 0.001f;
        public float sensitivity = 1000;

        public bool snap = false;
        public bool invert = false;

        [Tooltip("KeyOrMouseButton = 0, MouseMovement = 1, JoystickAxis = 2")]
        public AxisType axisType; //KeyOrMouseButton = 0, MouseMovement = 1, JoystickAxis = 2

        public int axis =0;
        public int joyNum;
    }
}
public enum AxisType
{
    KeyOrMouseButton =0,
    MouseMovement =1,
    JoystickAxis = 2,
}