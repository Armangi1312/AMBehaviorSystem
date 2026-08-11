using AMBehaviorSystem.Core.Utilities;
using AMBehaviorSystem.Pipelines;
using AMBehaviorSystem.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem
{
    public abstract class Controller : MonoBehaviour 
    {
        /// <summary>
        /// Verifies whether all settings and contexts required by the processor are registered, and automatically creates and registers instances if any are missing.
        /// </summary>
        public abstract void ValidateDependencies();

        /// <summary>
        /// Verifies whether the type is still required when a setting requested by the processor is being unregistered. Outputs a warning message if it is required; otherwise, allows the removal.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>Whether it is required</returns>
        public abstract bool IsSettingRequired(Type type);

        /// <summary>
        /// Verifies whether the type is still required when the context requested by the processor is being unregistered. Outputs a warning message if it is required; otherwise, allows removal.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>Whether it is required</returns>
        public abstract bool IsContextRequired(Type type);
    }

    public abstract partial class Controller<TSetting, TContext, TProcessor> : Controller
        where TSetting : ISetting
        where TContext : IContext
        where TProcessor : Processor<TSetting, TContext>
    {
        //==== 공유 프로퍼티 ====//

        /// <summary>
        /// An object that stores runtime semi-immutable data.
        /// </summary>
        [field: SerializeReference] public ObservableRegistry<TSetting> Settings { get; protected set; } = new();

        /// <summary>
        /// An object that stores runtime variable data.
        /// </summary>
        [field: SerializeReference] public ObservableRegistry<TContext> Contexts { get; protected set; } = new();

        /// <summary>
        /// A collection that stores module-level processors responsible for handling game logic.
        /// </summary>
        [field: SerializeReference] public ObservableList<TProcessor> Processors { get; protected set; } = new();

        [field: SerializeReference] public BasePipeline<TProcessor, TSetting, TContext> Pipeline { get; protected set; } = null;

        //==== 내부 프로퍼티 ====//

        [NonSerialized] protected bool IsInitialized = false;

        #region 초기화

        protected virtual void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;

            ValidateDependencies();
            InitializeProcessors();

            SubscribeEvents();
            InitializePipeline();
        }

        protected virtual void InitializeProcessors()
        {
            for (int i = 0; i < Processors.Count; i++)
            {
                TProcessor processor = Processors[i];
                if (processor == null)
                {
                    Debug.LogWarning($"Processor at index {i} is null.");
                    continue;
                }

                try
                {
                    processor.Initialize(Settings, Contexts);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to initialize processor '{processor.GetType().Name}': {e}");
                }
            }
        }

        protected virtual void InitializePipeline()
        {
            Pipeline?.Initialize(Processors, Settings, Contexts, this);
        }

        #endregion

        #region 의존성 검증

        /// <summary>
        /// Verifies whether all settings and contexts required by the processor are registered, and automatically creates and registers instances if any are missing.
        /// </summary>
        public override void ValidateDependencies()
        {
            CollectDependencies(out HashSet<Type> settingTypes, out HashSet<Type> contextTypes);
            SyncDependencies(settingTypes, contextTypes);
        }

        protected void CollectDependencies(out HashSet<Type> settingTypes, out HashSet<Type> contextTypes)
        {
            settingTypes = new HashSet<Type>();
            contextTypes = new HashSet<Type>();

            for (int i = 0; i < Processors.Count; i++)
            {
                TProcessor processor = Processors[i];
                if (processor == null) continue;

                (Type[] contexts, Type[] settings) = ProcessorDependencyValidator.GetRequiredTypes(processor.GetType());

                settingTypes.UnionWith(settings);
                contextTypes.UnionWith(contexts);
            }
        }

        protected void SyncDependencies(HashSet<Type> settingTypes, HashSet<Type> contextTypes)
        {
            SyncRegistryDependencies(settingTypes, Settings, "setting");
            SyncRegistryDependencies(contextTypes, Contexts, "context");
        }

        protected static void SyncRegistryDependencies<T>(HashSet<Type> types, ObservableRegistry<T> registry, string label)
        {
            foreach (Type type in types)
            {
                if (type == null || registry.Contains(type)) continue;

                if (!TryCreateInstance(type, out object instance))
                {
                    Debug.LogError($"Cannot create {label} instance: {type}");
                    continue;
                }

                registry.Register(instance);
            }
        }

        /// <summary>
        /// Verifies whether the type is still required when a setting requested by the processor is being unregistered. Outputs a warning message if it is required; otherwise, allows the removal.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>Whether it is required</returns>
        public override bool IsSettingRequired(Type type)
        {
            CollectDependencies(out HashSet<Type> settingTypes, out _);
            if (!settingTypes.Contains(type)) return false;

            Debug.LogWarning($"[Controller] Cannot remove setting '{type.Name}': required by one or more processors.");
            return true;
        }

        /// <summary>
        /// Verifies whether the type is still required when the context requested by the processor is being unregistered. Outputs a warning message if it is required; otherwise, allows removal.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>Whether it is required</returns>
        public override bool IsContextRequired(Type type)
        {
            CollectDependencies(out _, out HashSet<Type> contextTypes);
            if (!contextTypes.Contains(type)) return false;

            Debug.LogWarning($"[Controller] Cannot remove context '{type.Name}': required by one or more processors.");
            return true;
        }

        private static bool TryCreateInstance(Type type, out object instance)
        {
            instance = null;

            try
            {
                instance = Activator.CreateInstance(type);
                return instance != null;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create instance of '{type}': {e.Message}");
                return false;
            }
        }

        #endregion

        #region 이벤트 구독

        protected virtual void SubscribeEvents()
        {
            Settings.OnUnregistered += OnSettingUnregistered;
            Contexts.OnUnregistered += OnContextUnregistered;
            Processors.OnAdded += OnProcessorAdded;
        }

        protected virtual void UnsubscribeEvents()
        {
            Settings.OnUnregistered -= OnSettingUnregistered;
            Contexts.OnUnregistered -= OnContextUnregistered;
            Processors.OnAdded -= OnProcessorAdded;
        }

        private void OnSettingUnregistered(Type type, TSetting _)
        {
            if (IsSettingRequired(type))
                ValidateDependencies();
        }

        private void OnContextUnregistered(Type type, TContext _)
        {
            if (IsContextRequired(type))
                ValidateDependencies();
        }

        private void OnProcessorAdded(TProcessor processor)
        {
            if (processor == null) return;

            ValidateDependencies();
            processor.Initialize(Settings, Contexts);
        }

        #endregion

        #region 실행

        protected virtual void InvokeProcessors(InvokeTiming timing)
        {
            if(Pipeline != null)
            {
                Pipeline.Invoke(timing);
                return;
            }

            for (int i = 0; i < Processors.Count; i++)
            {
                TProcessor processor = Processors[i];
                if (processor == null || (processor.InvokeTiming & timing) == 0) continue;

                processor.Process();
            }
        }

        protected virtual void Awake() { Initialize(); InvokeProcessors(InvokeTiming.Awake); }
        protected virtual void Start() => InvokeProcessors(InvokeTiming.Start);
        protected virtual void Update() => InvokeProcessors(InvokeTiming.Update);
        protected virtual void FixedUpdate() => InvokeProcessors(InvokeTiming.FixedUpdate);
        protected virtual void LateUpdate() => InvokeProcessors(InvokeTiming.LateUpdate);
        protected virtual void OnEnable() => InvokeProcessors(InvokeTiming.OnEnable);
        protected virtual void OnDisable() => InvokeProcessors(InvokeTiming.OnDisable);

        private void OnDestroy()
        {
            InvokeProcessors(InvokeTiming.Destroy);
            UnsubscribeEvents();
        }

        #endregion
    }
}