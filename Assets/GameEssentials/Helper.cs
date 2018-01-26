using System.Collections;
using UnityEngine;

public static class Helper
{
    #region bool stuff
    public static int BoolToInt(bool _b)
   {
        if (_b)
        {
            return 1;
        }
        return 0;       
   }
	public static bool IntToBool(int _i)
    {
        if(_i==0)
        {
            return false;
        }
        return true;
    }

    public static bool RandomBool()
    {
        return Random.Range(0, 1) > 0.5f ? true : false;
    }
    #endregion

   
}
#region Timer
public class Timer
{
    float time = 0;
    public bool timeElapsed { get { return t; } }
    private bool t = false;
    private float length=0;

    public Timer(float _length)
    {
        length = _length;
        Reset();
    }
    
    public void ChangeTimer(float _time)
    {
        length = _time;
        Reset();
    }
    public void Reset()
    {
        time = 0;
        t = true;
    }

    public void StartTimer()
    {
        t = false;
    }

    bool Count()
    {
        if(time<length)
        {
            time += Time.deltaTime;
            return false;
        }
        return true;
    }
    private void Update()
    {
        if(!t)
        t = Count();
    }

    //IEnumerator Count(float _length)
    //{
    //    t = false;
    //    if (time<_length)
    //    {
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //    t = true;
    //}

}
#endregion