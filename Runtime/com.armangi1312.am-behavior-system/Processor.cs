using System;

namespace AMBehaviorSystem
{
    /// <summary>
    /// A module-level processor that handles game logic.
    /// It provides access only to the necessary settings and state via TSetting and TContext.
    /// </summary>
    [Serializable]
    public abstract class Processor
    {
        /// <summary>
        /// Indicates the timing at which this processor executes.
        /// </summary>
        public abstract InvokeTiming InvokeTiming { get; }

        /// <summary>
        /// The execution point where the call is currently being made.
        /// </summary>
        public InvokeTiming CurrentTiming { get; internal set; }

        /// <summary>
        /// Processing logic called every frame or fixed frame.
        /// Uses the settings and context cached during initialization.
        /// </summary>
        public abstract void Process();
    }

    /// <summary>
    /// A module-level processor that handles game logic.
    /// It provides access only to the necessary settings and state via TSetting and TContext.
    /// </summary>
    /// <typeparam name="TSetting">The type of settings used by this processor.</typeparam>
    /// <typeparam name="TContext">The type of context accessed by this processor.</typeparam>
    [Serializable]
    public abstract class Processor<TSetting, TContext> : Processor
        where TSetting : ISetting
        where TContext : IContext
    {
        /// <summary>
        /// Initializes the processor.
        /// Retrieves necessary settings and contexts from the Registry and caches them.
        /// </summary>
        /// <param name="settings">The settings store.</param>
        /// <param name="contexts">The context store.</param>
        public abstract void Initialize(IReadOnlyRegistry<TSetting> settings, IReadOnlyRegistry<TContext> contexts);
    }
}