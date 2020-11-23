using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepLookingAtCamera : MonoBehaviour
{
    private Transform target;
    public void Start() => target = Camera.main.transform;
    public void Update() => transform.rotation = Quaternion.LookRotation(transform.position - target.position);
}
