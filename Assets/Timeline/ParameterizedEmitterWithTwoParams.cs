using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ParameterizedEmitterWithTwoParams<T> : SignalEmitter{
    public T parameter1;
    public T parameter2;
}