using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ScriptedAnimationsBuilder
{
    public class ScriptedAnimationConfigBuilder<T> where T : ScriptedAnimationConfigBuilder<T>, new()
    {
        public GameObject prefab;
        public GameObject target;
        public Vector3 sourcePosition;
        public int quantity = 1;
        public float duration = 1;
        public string type = "";
        public string layerName = "AboveAll";
        public int orderId = 30;
        public float scale = 1 / 3f;
        public float spread = 50f;

        public Action callback = null;

        public T SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
            return (T)this;
        }
        public T SetTarget(GameObject target)
        {
            this.target = target;
            return (T)this;
        }
        public T SetSourcePosition(Vector3 sourcePosition)
        {
            this.sourcePosition = sourcePosition;
            return (T)this;
        }
        public T SetQuantity(int quantity)
        {
            this.quantity = quantity;
            return (T)this;
        }
        public T SetDuration(float time)
        {
            duration = time;
            return (T)this;
        }
        public T SetType(string type)
        {
            this.type = type;
            return (T)this;
        }
        public T SetLayer(string layerName)
        {
            this.layerName = layerName;
            return (T)this;
        }
        public T SetOrderId(int orderId)
        {
            this.orderId = orderId;
            return (T)this;
        }
        public T SetScale(float scale)
        {
            this.scale = scale;
            return (T)this;
        }
        public T SetSpread(float spread)
        {
            this.spread = spread;
            return (T)this;
        }
        public T AddCallback(Action callback)
        {
            this.callback += callback;
            return (T)this;
        }

        public T Copy()
        {
            var copy = new T();

            copy.prefab = prefab;
            copy.target = target;
            copy.sourcePosition = sourcePosition;
            copy.quantity = quantity;
            copy.duration = duration;
            copy.type = type;
            copy.layerName = layerName;
            copy.orderId = orderId;
            copy.scale = scale;
            copy.callback = callback;

            return copy;
        }
    }
}

public class ScriptedAnimationConfig : ScriptedAnimationsBuilder.ScriptedAnimationConfigBuilder<ScriptedAnimationConfig> { }