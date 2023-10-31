using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 二つintのパラメータを持つシグナル
/// </summary>
public class SignalReceiverWithTwoInt : MonoBehaviour, INotificationReceiver
{
    public SignalAssetEventPair[] signalAssetEventPairs;

    [Serializable]
    public class SignalAssetEventPair{
        public SignalAsset signalAsset;
        public ParameterizedEvent events;

        [Serializable]
        public class ParameterizedEvent : UnityEvent<int, int>{}
    }
    public void OnNotify(Playable origin, INotification notification, object context){
        if(notification is ParameterizedEmitterWithTwoParams<int> intEmitter){
            var matches = signalAssetEventPairs.Where(x => ReferenceEquals(x.signalAsset, intEmitter.asset));
            foreach(var m in matches){
                m.events.Invoke(intEmitter.parameter1, intEmitter.parameter2);
            }
        }
    }
}
