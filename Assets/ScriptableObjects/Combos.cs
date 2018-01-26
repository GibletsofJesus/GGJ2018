using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Combo", menuName = "Move Set", order = 1)]
[Serializable]
public class Combos : ScriptableObject
{
    public ComboInput[] combos =new ComboInput[1];
    
    private void OnEnable()
    {

        for(int i=0;i<combos.Length;i++)
        {
            combos[i].InitializeCombo();
        }
    }
    public ComboInput ReturnCombo()
    {
        return new ComboInput();
    }
}
[Serializable]
public class ComboInput
{
    [Tooltip("name of combo for animator")] public string comboName;
    [SerializeField] ComboButton[] move = new ComboButton[1];
    [HideInInspector]
    public ControllerButton[] combo = null;
    bool initialized = false;
    public void InitializeCombo()
    {
        if (!initialized)
        {
            Debug.Log("initializing "+comboName);
            combo = ConsolidateCombo();
            initialized = true;
        }
    }

    public ComboInput GetCombo()
    {
        if (combo == null)
        {
            combo = ConsolidateCombo();
        }
        return this;
    }
    
    ControllerButton[] ConsolidateCombo()
    {
        ControllerButton[] cmbo = new ControllerButton[move.Length];
        for (int i = 0; i < cmbo.Length; i++)
        {
            cmbo[i] = move[i].GetButton();
        }
        
        return cmbo;
    }
}
[Serializable]
public class ComboButton
{
    [Tooltip("Button or combination of buttons in the combo")]
    public ControllerButton[] button = new ControllerButton[1] { ControllerButton.NONE };

    public ControllerButton GetButton()
    {
        ControllerButton temp = ControllerButton.NONE;
        for (int i=0;i<button.Length;i++)
        {
            temp |= button[i];
        }
        return temp;
    }
}