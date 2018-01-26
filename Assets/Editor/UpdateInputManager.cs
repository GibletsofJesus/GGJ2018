#if UNITY_EDITOR
using UnityEditor;


public static class UpdateInputManager
{
    public static void ClearInputData()
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        axesProperty.ClearArray();
        serializedObject.ApplyModifiedProperties();
    }
    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        return null;
    }

    private static bool AxisDefined(string axisName)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false))
        {
            SerializedProperty axis = axesProperty.Copy();
            axis.Next(true);
            if (axis.stringValue == axisName) return true;
        }
        return false;
    }

    public static void CreateInputs(InputSetup _scriptable, int _controllers,float _deadZone)
    {
        ClearInputData();
        for (int i = 0; i < _controllers; i++)
        {
            foreach (InputSetup.JoystickAxis j in _scriptable.joystickAxis)
            {

                AddAxis(j, (i + 1),_deadZone);
            }
            foreach (InputSetup.Button b in _scriptable.buttons)
            {
                AddButton(b, ( i + 1));
            }
        }
        foreach (InputSetup.JoystickAxis j in _scriptable.joystickAxis)
        {

            AddAxis(j, 0, _deadZone);
        }
        foreach (InputSetup.Button b in _scriptable.buttons)
        {
            AddButton(b,0);
        }
    }
    private static void AddAxis(InputSetup.JoystickAxis _axis, int _joyNum, float _deadZone)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
        if (_joyNum == 0)
        {
            GetChildProperty(axisProperty, "m_Name").stringValue = _axis.name;
        }
        else
            GetChildProperty(axisProperty, "m_Name").stringValue = _axis.name + " " + (_joyNum - 1);
        GetChildProperty(axisProperty, "descriptiveName").stringValue = _axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = _axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = _axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = _axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = _axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = _axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = _axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = _deadZone;
        GetChildProperty(axisProperty, "sensitivity").floatValue = _axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = _axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = _axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)_axis.axisType;
        GetChildProperty(axisProperty, "axis").intValue = _axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = _joyNum;

        serializedObject.ApplyModifiedProperties();
    }
    private static void AddButton(InputSetup.Button _button, int _joyNum)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
        string buttonFix = _button.positiveButton.Replace('+', _joyNum.ToString()[0]);
        if(_joyNum==0)
        {
            int r = _button.positiveButton.IndexOf('+');
            buttonFix = _button.positiveButton.Remove(r,2);
            GetChildProperty(axisProperty, "m_Name").stringValue = _button.name;
        }
        else
        GetChildProperty(axisProperty, "m_Name").stringValue = _button.name+" "+(_joyNum-1);

        GetChildProperty(axisProperty, "descriptiveName").stringValue = _button.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = _button.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = _button.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = buttonFix;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = _button.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = _button.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = _button.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = _button.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = _button.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = _button.snap;
        GetChildProperty(axisProperty, "invert").boolValue = _button.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)_button.axisType;
        GetChildProperty(axisProperty, "axis").intValue = _button.axis;
        GetChildProperty(axisProperty, "joyNum").intValue = _joyNum;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif