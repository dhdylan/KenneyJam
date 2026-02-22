using System;
using UnityEngine;
using UnityEngine.Events;

public class InvokeOnFrequency : MonoBehaviour
{
    public UnityEvent OnTick;

    public double frequency;

    private double time = 0.0;

    private ulong tick = 0;

    private void Update()
    {
        time += Time.deltaTime;

        ulong thisTick = Convert.ToUInt64(time / (1.0 / frequency));
        if(thisTick > tick)
        {
            OnTick.Invoke();
            tick = thisTick;
        }
    }
}