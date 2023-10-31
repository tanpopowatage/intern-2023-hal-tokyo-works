using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 一つintのパラメータを持つシグナル
/// </summary>
public class SignalReceiverWithInt : MonoBehaviour, INotificationReceiver
{
    public SignalAssetEventPair[] signalAssetEventPairs;

    [Serializable]
    public class SignalAssetEventPair{
        public SignalAsset signalAsset;
        public ParameterizedEvent events;

        [Serializable]
        public class ParameterizedEvent : UnityEvent<int>{}
    }
    public void OnNotify(Playable origin, INotification notification, object context){
        if(notification is ParameterizedEmitter<int> intEmitter){
            var matches = signalAssetEventPairs.Where(x => ReferenceEquals(x.signalAsset, intEmitter.asset));
            foreach(var m in matches){
                m.events.Invoke(intEmitter.parameter);
            }
        }
    }
}


