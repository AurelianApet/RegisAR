﻿/*     INFINITY CODE 2013-2019      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

/// <summary>
/// 3D marker instance class.
/// </summary>
[AddComponentMenu("")]
public class OnlineMapsMarker3DInstance : OnlineMapsMarkerInstanceBase
{
    private double _longitude;
    private double _latitude;
    private float _scale;

    private OnlineMapsMarker3D _marker;

    public override OnlineMapsMarkerBase marker
    {
        get { return _marker; }
        set { _marker = value as OnlineMapsMarker3D; }
    }

    private void Awake()
    {
        Collider cl = GetComponent<Collider>();
        if (cl == null) cl  = gameObject.AddComponent<BoxCollider>();
        cl.isTrigger = true;
    }

    private void LateUpdate()
    {
        if (marker as OnlineMapsMarker3D == null) 
        {
            OnlineMapsUtils.Destroy(gameObject);
            return;
        }

        UpdateBaseProps();
    }

    private void Start()
    {
        marker.GetPosition(out _longitude, out _latitude);
        _scale = marker.scale;
        transform.localRotation = (marker as OnlineMapsMarker3D).rotation;
        transform.localScale = new Vector3(_scale, _scale, _scale);
    }

    private void UpdateBaseProps()
    {
        double mx, my;
        marker.GetPosition(out mx, out my);

        if (Math.Abs(_longitude - mx) > double.Epsilon || Math.Abs(_latitude - my) > double.Epsilon)
        {
            _longitude = mx;
            _latitude = my;

            marker.Update();
        }

        if (Math.Abs(_scale - marker.scale) > float.Epsilon)
        {
            _scale = marker.scale;
            transform.localScale = new Vector3(_scale, _scale, _scale);
        }
    }
}