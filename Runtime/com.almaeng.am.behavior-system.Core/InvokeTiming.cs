using System;
using UnityEngine;

namespace AMBehaviorSystem.Core
{
    /// <summary>
    /// An enumeration representing the timing at which a Unity event is invoked.
    /// </summary>
    [Flags]
    public enum InvokeTiming
    {
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        Awake = 1 << 1,

        /// <summary>
        /// Start is called just before the Update method is called for the first time.
        /// </summary>
        Start = 1 << 2,

        /// <summary>
        /// Update is called every frame.
        /// </summary>
        Update = 1 << 3,

        /// <summary>
        /// FixedUpdate is called at fixed frame intervals.
        /// </summary>
        FixedUpdate = 1 << 4,

        /// <summary>
        /// LateUpdate is called every frame after Update has been called.
        /// </summary>
        LateUpdate = 1 << 5,

        /// <summary>
        /// Destroy is called when the object is destroyed.
        /// </summary>
        Destroy = 1 << 6,

        /// <summary>
        /// OnEnable is called when the object becomes enabled and active.
        /// </summary>
        OnEnable = 1 << 7,

        /// <summary>
        /// OnDisable is called when the behavior becomes disabled or inactive.
        /// </summary>
        OnDisable = 1 << 8
    }
}
