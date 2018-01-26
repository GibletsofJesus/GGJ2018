using System;
using UnityEngine;

public static class Controller
{
    static ControllerButton[] lastButton = new ControllerButton[8] {ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE };
    static ControllerButton[] currentButton = new ControllerButton[8] {ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE, ControllerButton.NONE };
    static bool normalisedAxes = false;
    static float deadZone = 0.1f;
    static float[] timer = new float[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
    static ControllerType type = ControllerType.PLAYSTATION;
    /// <summary>
    /// Call to resize button array size
    /// </summary>
    /// <param name="_controllers"></param>
    public static void SetControllerAmount(int _controllers)
    {
        lastButton = new ControllerButton[_controllers];
        currentButton = new ControllerButton[_controllers];
        for (int i=0;i<_controllers;i++)
        {
            lastButton[i] = ControllerButton.NONE;
            currentButton[i] = ControllerButton.NONE;
        }
    }
    public static void SetControllerType(ControllerType _type)
    {
        type = _type;
    }

    public static void NormalizeAxes(bool _normalise)
    {
        normalisedAxes = _normalise;
    }
    public static void SetDeadZoneAmount(float _deadZone)
    {
        deadZone = _deadZone;
    }
    #region AxisInputs
    public static float LX(int _contIndex=0)
    {
       return Input.GetAxis("LX " + _contIndex);
    }
    public static float LY (int _contIndex =0)
    {
        return Input.GetAxis("LY " + _contIndex);
    }
    public static float RX(int _contIndex =0)
    {
        return Input.GetAxis("RX " + _contIndex);
    }
    public static float RY(int _contIndex = 0)
    {
        return Input.GetAxis("RY " + _contIndex);
    }
    public static Vector2 LeftAxis(int _contIndex=0)
    {
        Vector2 axis =new Vector2(Input.GetAxis("LX "+_contIndex), Input.GetAxis("LY "+_contIndex));
        if (axis.magnitude < deadZone)
            axis = Vector2.zero;
        else
            axis = axis.normalized * ((axis.magnitude / deadZone) / (1 - deadZone));
        return axis;
    }
    public static Vector2 RightAxis(int _contIndex=0)
    {
       Vector2 axis = new Vector2(Input.GetAxis("RX " + _contIndex), Input.GetAxis("RY " + _contIndex));
        if (axis.magnitude < deadZone)
            axis = Vector2.zero;
        else
            axis = axis.normalized * ((axis.magnitude / deadZone) / (1 - deadZone));
        return axis;
    }
    public static Vector2 DPad(int _contIndex=0)
    {
        Vector2 axis =  new Vector2(Input.GetAxis("DPadX " + _contIndex), Input.GetAxis("DPadY " + _contIndex));
        if (axis.magnitude < deadZone)
            axis = Vector2.zero;
        else
            axis = axis.normalized * ((axis.magnitude / deadZone) / (1 - deadZone));
        return axis;
    }
    public static float LeftTrigger(int _contIndex=0)
    {
        float f = Input.GetAxis("L2 "+_contIndex);
        return f>0? f:0;
    }
    public static float DPadX(int _contIndex = 0)
    {
        return  Input.GetAxis("DPadX " + _contIndex);
    }
    public static float DPadY(int _contIndex = 0)
    {
        return Input.GetAxis("DPadY " + _contIndex);
    }
    #endregion

    #region Button Inputs

    public static bool LeftTriggerDown(int _contIndex =0)
    {
        bool b = false;
        b = Input.GetAxis("L2 " + _contIndex) > 0.5f ? true : false;   
        return b;
    }

    public static bool RightTriggerDown(int _contIndex =0)
    {
        bool b = false;
        b = Input.GetAxis("R2 " + _contIndex) > 0.5f ? true : false;
        return b;
    }

    public static float RightTrigger(int _contIndex=0)
    {
        float f = Input.GetAxis("L2 " + _contIndex);
        return f>0? f:0;
    }
   
    public static bool LeftBumperDown(int _contIndex=0)
    {
        return Input.GetButtonDown("L1 " + _contIndex);
    }

    public static bool RightBumperDown(int _contIndex=0)
    {
        return Input.GetButtonDown("R1 " + _contIndex);
    }
    public static ControllerButton ButtonDown(int _contIndex=0)
    {
        lastButton[_contIndex] = currentButton[_contIndex];
        currentButton[_contIndex] = ControllerButton.NONE;

        if (Input.GetButtonDown("L1 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.L1;
        }
        if (Input.GetButtonDown("L2 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.L2;
        }
        if (Input.GetButtonDown("R2 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.R2;
        }
        if (Input.GetButtonDown("R1 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.R1;
        }
        if (Input.GetAxis("LX " + _contIndex) != 0)
        {
            float axis = Input.GetAxis("LX " + _contIndex);
            if (axis < -0.5f)
            {
                currentButton[_contIndex] |= ControllerButton.LEFT;
            }
            else if (axis > 0.5f)
            {
                currentButton[_contIndex] |= ControllerButton.RIGHT;
            }
        }
        if (Input.GetAxis("LY " + _contIndex) != 0)
        {
            float axis = Input.GetAxis("LY " + _contIndex);
            if (axis < -0.5f)
            {
                currentButton[_contIndex] |= ControllerButton.UP;
            }
            else if (axis > 0.5f)
            {
                currentButton[_contIndex] |= ControllerButton.DOWN;
            }
        }
        if (Input.GetAxis("DPadY " + _contIndex) != 0)
        {
            if (Input.GetAxis("DPadY " + _contIndex) < 0)
            {
                currentButton[_contIndex] |= ControllerButton.DPAD_D;
            }
            if (Input.GetAxis("DPadY " + _contIndex) > 0)
            {
                currentButton[_contIndex] |= ControllerButton.DPAD_U;
            }
        }
        if (Input.GetAxis("DPadX " + _contIndex) != 0)
        {
            if (Input.GetAxis("DPadX " + _contIndex) > 0)
            {
                currentButton[_contIndex] |= ControllerButton.DPAD_R;
            }
            if (Input.GetAxis("DPadX " + _contIndex) < 0)
            {
                currentButton[_contIndex] |= ControllerButton.DPAD_L;
            }
        }
        if (Input.GetButtonDown("Circle " + _contIndex))
        {
            currentButton[_contIndex] |= (type == ControllerType.PLAYSTATION ? ControllerButton.SQUARE : ControllerButton.X);
        }
        if (Input.GetButtonDown("Cross " + _contIndex))
        {
            currentButton[_contIndex] |= (type == ControllerType.PLAYSTATION ? ControllerButton.CROSS : ControllerButton.A);
        }
        if (Input.GetButtonDown("Square " + _contIndex))
        {
            currentButton[_contIndex] |= (type == ControllerType.PLAYSTATION ? ControllerButton.CIRCLE : ControllerButton.B);
        }
        if (Input.GetButtonDown("Triangle " + _contIndex))
        {
            currentButton[_contIndex] |= (type == ControllerType.PLAYSTATION ? ControllerButton.TRIANGLE : ControllerButton.Y);
        }
        if (Input.GetButtonDown("Share " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.SHARE;
        }
        if (Input.GetButtonDown("Options " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.OPTIONS;
        }
        if (Input.GetButtonDown("L3 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.L3;
        }
        if (Input.GetButtonDown("R3 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.R3;
        }
        if (Input.GetButtonDown("PS " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.HOME;
        }
        if (Input.GetButtonDown("TouchPad " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.PAD;
        }
        return currentButton[_contIndex];
    }
    public static ControllerButton ButtonHeld(out float _time,int _contIndex = 0)
    {
       ControllerButton cb =  Button(_contIndex);
       

        Debug.Log(cb);
        if (currentButton[_contIndex]==lastButton[_contIndex])
        {
            timer[_contIndex] += Time.deltaTime;
            _time = timer[_contIndex];
            return currentButton[_contIndex];
        }
        timer[_contIndex] = 0;
        _time = 0;
        return ControllerButton.NONE;
    }
    public static ControllerButton Button(int _contIndex =0)
    {
        lastButton[_contIndex] = currentButton[_contIndex];
        currentButton[_contIndex] = ControllerButton.NONE;

        if (Input.GetButton("L1 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.L1;
        }
        if (Input.GetButton("L2 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.L2;
        }
        if (Input.GetButton("R2 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.R2;
        }
        if (Input.GetButton("R1 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.R1;
        }
        if (Input.GetAxis("LX "+ _contIndex)!=0)
        {
            float axis = Input.GetAxis("LX " + _contIndex);
            if (axis<-0.5f)
            {
                currentButton[_contIndex] |= ControllerButton.LEFT;
            }
            else if (axis>0.5f)
            {
                currentButton[_contIndex] |= ControllerButton.RIGHT;
            }
        }
        if(Input.GetAxis("LY "+_contIndex)!=0)
        {
            float axis = Input.GetAxis("LY " + _contIndex);
            if (axis<-0.5f)
            {
                currentButton[_contIndex] |= ControllerButton.UP;
            }
            else if (axis>0.5f)
            {
                currentButton[_contIndex] |= ControllerButton.DOWN;
            }
        }
        if(Input.GetAxis("DPadY "+_contIndex)!=0)
        {
            if(Input.GetAxis("DPadY "+_contIndex)<0)
            {
                currentButton[_contIndex] |= ControllerButton.DPAD_D;
            }
            if (Input.GetAxis("DPadY " + _contIndex) > 0)
            {
                currentButton[_contIndex] |= ControllerButton.DPAD_U;
            }
        }
        if (Input.GetAxis("DPadX "+_contIndex)!=0)
        {
            if(Input.GetAxis("DPadX " + _contIndex)>0)
            {
                currentButton[_contIndex] |= ControllerButton.DPAD_R;
            }
            if(Input.GetAxis("DPadX "+_contIndex)<0)
            {
                currentButton[_contIndex] |= ControllerButton.DPAD_L;
            }
        }
        if(Input.GetButton("Circle "+_contIndex))
        {
            currentButton[_contIndex] |=( type == ControllerType.PLAYSTATION? ControllerButton.SQUARE : ControllerButton.X);
        }
        if(Input.GetButton("Cross "+_contIndex))
        {
            currentButton[_contIndex] |= (type == ControllerType.PLAYSTATION? ControllerButton.CROSS : ControllerButton.A);
        }
        if(Input.GetButton("Square "+_contIndex))
        {
            currentButton[_contIndex] |= (type == ControllerType.PLAYSTATION? ControllerButton.CIRCLE: ControllerButton.B);
        }
        if (Input.GetButton("Triangle "+_contIndex))
        {
            currentButton[_contIndex] |= (type == ControllerType.PLAYSTATION? ControllerButton.TRIANGLE : ControllerButton.Y);
        }
        if(Input.GetButton("Share "+ _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.SHARE;
        }
        if(Input.GetButton("Options "+_contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.OPTIONS;
        }
        if (Input.GetButton("L3 "+ _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.L3;
        }
        if (Input.GetButton("R3 " + _contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.R3;
        }
        if (Input.GetButton("PS "+_contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.HOME;
        }
        if(Input.GetButton("TouchPad "+_contIndex))
        {
            currentButton[_contIndex] |= ControllerButton.PAD;
        }
        return currentButton[_contIndex];
    }
   
   private static ControllerButton ControllerCompat(ControllerButton _button)
    {
        switch(_button)
        {
            case ControllerButton.Y:
                return _button |= ControllerButton.TRIANGLE | ControllerButton.Y;
            case ControllerButton.X:
                return _button |= ControllerButton.SQUARE | ControllerButton.X;               
            case ControllerButton.A:
                return _button |= ControllerButton.CROSS;                
            case ControllerButton.B:
                return _button |= ControllerButton.CIRCLE;
            case ControllerButton.CIRCLE:
                return _button |= ControllerButton.B;
            case ControllerButton.CROSS:
                return _button |= ControllerButton.A;
            case ControllerButton.SQUARE:
                return _button |= ControllerButton.X;
            case ControllerButton.TRIANGLE:
                return _button |= ControllerButton.Y;
        }
        return _button;
    }
    /// <summary>
    /// Returns true if specified button is pressed
    /// </summary>
    /// <param name="_button"></param>
    /// <returns></returns>
    public static bool GetButton(ControllerButton _button, int _contIndex =0)
    {
     ///   _button = ControllerCompat(_button);
        return (_button & Button(_contIndex)) != 0;
    }

    /// <summary>
    /// returns true if specified button is held
    /// </summary>
    /// <param name="_button"></param>
    /// <returns></returns>
    public static bool GetButtonHeld(ControllerButton _button, int _contIndex =0)
    {
        //_button = ControllerCompat(_button);
        return (_button & Button(_contIndex)) != 0 && (_button & lastButton[_contIndex]) != 0;
    }
    /// <summary>
    /// returns true if specified button has just been pressed
    /// </summary>
    /// <param name="_button"></param>
    /// <returns></returns>
    public static bool GetButtonJustDown(ControllerButton _button, int _contIndex =0)
    {
        //_button = ControllerCompat(_button);
        //last button to be false, current button to be true
        return ((_button & lastButton[_contIndex]) == 0) && GetButton(_button, _contIndex);
    }
    /// <summary>
    /// returns true if specified button has just been released
    /// </summary>
    /// <param name="_button"></param>
    /// <returns></returns>
    public static bool GetButtonUp(ControllerButton _button, int _contIndex =0)
    {
        //_button = ControllerCompat(_button);
        //last button to be true, current button to be false
        return ((_button & lastButton[_contIndex]) != 0) && !GetButton(_button, _contIndex);
    }
#endregion
}

[Flags]
public enum ControllerButton
{
    
    NONE = 0,
    SQUARE = 1,  
    CROSS = 2,
    CIRCLE = 4,              
    TRIANGLE = 8,            
    L3 = 16,
    R3 = 32,
    L2 = 64,
    R2 = 128,
    L1 = 256,       //use for "Move Button Left" on PS -- use for "Grip Left" on Vive
    R1 = 512,       //use for "Move Button Right" on PS -- use for "Grip Right" on Vive
    HOME = 1024,
    OPTIONS = 2048,
    PAD = 4096,
    SHARE = 8192,
    DPAD_L = 16384,      //use for left Move controller "Square" on PS
    DPAD_R = 32768,      //use for left Move controller "Circle" on PS
    DPAD_U = 65536,      //use for left Move controller "Triangle" on PS
    DPAD_D = 131072,      //use for left Move controller "Cross" on PS
    LEFT = 262144,
    RIGHT = 524288,
    UP = 1048576,
    DOWN = 2097152,
    X = 4194304,
    Y = 8388608,
    A = 16777216,
    B = 33554432,
}
public enum ControllerType
{
    PLAYSTATION = 0,
    XBOX = 1,           //can be used for 360, one and steam
}