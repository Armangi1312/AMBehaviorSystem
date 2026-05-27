using System;
using UnityEngine;

namespace AMBehaviorSystem.Core
{
    /// <summary>
    /// 유니티 이벤트의 호출되는 시점을 나타내는 열거형입니다.
    /// </summary>
    [Flags]
    public enum InvokeTiming
    {
        /// <summary>
        /// Awake는 스크립트 인스턴스가 로드되는 중에 호출됩니다.
        /// </summary>
        Awake = 1 << 1,

        /// <summary>
        /// Start는 Update 메서드가 처음으로 호출되기 바로 전에 호출됩니다.
        /// </summary>
        Start = 1 << 2,

        /// <summary>
        /// Update는 매 프레임마다 호출됩니다.
        /// </summary>
        Update = 1 << 3,

        /// <summary>
        /// FixedUpdate는 고정 프레임마다 호출됩니다.
        /// </summary>
        FixedUpdate = 1 << 4,

        /// <summary>
        /// LateUpdate는 Update가 호출된 후 매 프레임마다 호출됩니다.
        /// </summary>
        LateUpdate = 1 << 5,

        /// <summary>
        /// Destroy는 오브젝트가 삭제될 때 호출됩니다.
        /// </summary>
        Destroy = 1 << 6,

        /// <summary>
        /// OnEnable은 개체가 사용하도록 설정되고 활성 상태가 되면 호출됩니다.
        /// </summary>
        OnEnable = 1 << 7,

        /// <summary>
        /// OnDisable는 동작이 사용할 수 없거나 비활성화되는 경우 호출됩니다.
        /// </summary>
        OnDisable = 1 << 8
    }
}
