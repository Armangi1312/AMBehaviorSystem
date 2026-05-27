using System;

namespace AMBehaviorSystem.Core
{
    /// <summary>
    /// 게임 로직을 처리하는 모듈 단위의 프로세서입니다.
    /// TSetting과 TContext를 통해 필요한 설정과 상태에만 접근할 수 있습니다.
    /// </summary>
    /// <typeparam name="TSetting">이 프로세서가 사용할 설정 타입입니다.</typeparam>
    /// <typeparam name="TContext">이 프로세서가 접근할 컨텍스트 타입입니다.</typeparam>
    [Serializable]
    public abstract class Processor
    {
        /// <summary>
        /// 이 프로세서가 실행될 타이밍을 나타냅니다.
        /// </summary>
        public abstract InvokeTiming InvokeTiming { get; }

        /// <summary>
        /// 매 프레임 또는 고정 프레임마다 호출되는 처리 로직입니다.
        /// Initialize에서 캐싱한 설정과 컨텍스트를 사용합니다.
        /// </summary>
        public abstract void Process();
    }

    /// <summary>
    /// 게임 로직을 처리하는 모듈 단위의 프로세서입니다.
    /// TSetting과 TContext를 통해 필요한 설정과 상태에만 접근할 수 있습니다.
    /// </summary>
    /// <typeparam name="TSetting">이 프로세서가 사용할 설정 타입입니다.</typeparam>
    /// <typeparam name="TContext">이 프로세서가 접근할 컨텍스트 타입입니다.</typeparam>
    [Serializable]
    public abstract class Processor<TSetting, TContext> : Processor
        where TSetting : ISetting
        where TContext : IContext
    {
        /// <summary>
        /// 프로세서를 초기화합니다.
        /// 필요한 설정과 컨텍스트를 Registry에서 가져와 캐싱합니다.
        /// </summary>
        /// <param name="settings">설정 저장소입니다.</param>
        /// <param name="contexts">컨텍스트 저장소입니다.</param>
        public abstract void Initialize(IReadOnlyRegistry<TSetting> settings, IReadOnlyRegistry<TContext> contexts);
    }
}