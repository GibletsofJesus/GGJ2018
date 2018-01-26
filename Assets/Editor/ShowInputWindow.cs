#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


public class ShowInputWindow :EditorWindow
{
    InputSetup inputs = null;
    int controllerAmount = 1;
    float deadZone = 0.3f;
    ControllerType type;
   [MenuItem("Input Manager/Custom Input %i")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ShowInputWindow));
    }
    private void Awake()
    {
        inputs = inputs==null?AssetDatabase.LoadAssetAtPath<InputSetup>("Assets/GameEssentials/Inputs/GamePad.asset"):inputs;
        UpdateInputManager.CreateInputs(inputs, controllerAmount, deadZone);
        Controller.SetControllerAmount(controllerAmount);
    }
    void OnGUI()
    {
        GUILayout.Label("Generate Inputs", EditorStyles.boldLabel);
        GUILayout.Label(" ");
        controllerAmount = EditorGUILayout.IntSlider("Amount of Controllers", controllerAmount, 1, 8);
        GUILayout.Label("Controller Template File (Default is DualShock4)");
        inputs = EditorGUILayout.ObjectField(inputs, typeof(InputSetup), false) as  InputSetup;
        GUILayout.Label("Axis DeadZone (0 = none, 1 = full");
        deadZone = EditorGUILayout.Slider("Dead Zone", deadZone, 0, 1);
        GUILayout.Label("GamePad - switches face button names for ease of development");
        type = (ControllerType)GUILayout.Toolbar((int)type, new string[2] { "DualShock4", "Xbox" });
        if( GUILayout.Button("Press to Set up " + controllerAmount + " Controllers", EditorStyles.toolbarButton))
        {
            UpdateInputManager.CreateInputs(inputs,controllerAmount,deadZone);
            Controller.SetControllerAmount(controllerAmount);
            Controller.SetDeadZoneAmount(deadZone);
            Controller.SetControllerType(type);
        }
    }

}

#endif




