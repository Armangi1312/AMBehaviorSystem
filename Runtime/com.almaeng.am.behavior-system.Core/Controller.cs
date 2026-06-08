using AMBehaviorSystem.Core.Pipelines;
using AMBehaviorSystem.Core.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Core
{
    public abstract class Controller : MonoBehaviour 
    {
        /// <summary>
        /// 프로세서가 요구하는 설정과 컨텍스트가 모두 등록되어 있는지 검증하고, 누락된 경우 자동으로 인스턴스를 생성하여 등록합니다.
        /// </summary>
        public abstract void ValidateDependencies();

        /// <summary>
        /// 프로세서가 요구하는 설정이 등록 해제되는 경우, 해당 타입이 여전히 필요한지 검증합니다. 필요한 경우 경고 메시지를 출력하고, 그렇지 않은 경우 제거를 허용합니다.
        /// </summary>
        /// <param name="type">확인할 타입</param>
        /// <returns>필요한 지</returns>
        public abstract bool IsSettingRequired(Type type);

        /// <summary>
        /// 프로세서가 요구하는 컨텍스트가 등록 해제되는 경우, 해당 타입이 여전히 필요한지 검증합니다. 필요한 경우 경고 메시지를 출력하고, 그렇지 않은 경우 제거를 허용합니다.
        /// </summary>
        /// <param name="type">확인할 타입</param>
        /// <returns>필요한 지</returns>
        public abstract bool IsContextRequired(Type type);
    }

    public abstract partial class Controller<TSetting, TContext, TProcessor> : Controller
        where TSetting : ISetting
        where TContext : IContext
        where TProcessor : Processor<TSetting, TContext>
    {
        //==== 공유 프로퍼티 ====//

        /// <summary>
        /// 런타임 준불변 데이터를 저장하는 객체입니다.
        /// </summary>
        [field: SerializeReference] public ObservableRegistry<TSetting> Settings { get; protected set; } = new();

        /// <summary>
        /// 런타임 가변 데이터를 저장하는 객체입니다.
        /// </summary>
        [field: SerializeReference] public ObservableRegistry<TContext> Contexts { get; protected set; } = new();

        /// <summary>
        /// 게임 로직을 처리하는 모듈 단위의 프로세서를 저장하는 컬렉션입니다.
        /// </summary>
        [field: SerializeReference] public ObservableList<TProcessor> Processors { get; protected set; } = new();

        // TODO: Pipeline 시스템 연동 시 활성화
        // [SerializeReference] protected PipelineGraph Graph;
        public Pipeline Pipeline { get; private set; }

        //==== 내부 프로퍼티 ====//

        [NonSerialized] protected bool IsInitialized = false;

        #region 초기화

        protected virtual void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;

            ValidateDependencies();
            InitializeProcessors();
            InitializePipeline();
            SubscribeEvents();
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
            // TODO: Pipeline 시스템 연동 시 활성화
            // Pipeline = PipelineFactory.CreatePipeline(Graph);
        }

        #endregion

        #region 의존성 검증

        /// <summary>
        /// 프로세서가 요구하는 설정과 컨텍스트가 모두 등록되어 있는지 검증하고, 누락된 경우 자동으로 인스턴스를 생성하여 등록합니다.
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
        /// 프로세서가 요구하는 설정이 등록 해제되는 경우, 해당 타입이 여전히 필요한지 검증합니다. 필요한 경우 경고 메시지를 출력하고, 그렇지 않은 경우 제거를 허용합니다.
        /// </summary>
        /// <param name="type">확인할 타입</param>
        /// <returns>필요한 지</returns>
        public override bool IsSettingRequired(Type type)
        {
            CollectDependencies(out HashSet<Type> settingTypes, out _);
            if (!settingTypes.Contains(type)) return false;

            Debug.LogWarning($"[Controller] Cannot remove setting '{type.Name}': required by one or more processors.");
            return true;
        }

        /// <summary>
        /// 프로세서가 요구하는 컨텍스트가 등록 해제되는 경우, 해당 타입이 여전히 필요한지 검증합니다. 필요한 경우 경고 메시지를 출력하고, 그렇지 않은 경우 제거를 허용합니다.
        /// </summary>
        /// <param name="type">확인할 타입</param>
        /// <returns>필요한 지</returns>
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
            for (int i = 0; i < Processors.Count; i++)
            {
                TProcessor processor = Processors[i];
                if (processor == null || (processor.InvokeTiming & timing) == 0) continue;

                processor.Process();
            }
        }

        private void Awake() { Initialize(); InvokeProcessors(InvokeTiming.Awake); }
        private void Start() => InvokeProcessors(InvokeTiming.Start);
        private void Update() => InvokeProcessors(InvokeTiming.Update);
        private void FixedUpdate() => InvokeProcessors(InvokeTiming.FixedUpdate);
        private void LateUpdate() => InvokeProcessors(InvokeTiming.LateUpdate);
        private void OnEnable() => InvokeProcessors(InvokeTiming.OnEnable);
        private void OnDisable() => InvokeProcessors(InvokeTiming.OnDisable);

        private void OnDestroy()
        {
            InvokeProcessors(InvokeTiming.Destroy);
            UnsubscribeEvents();
        }

        #endregion
    }
}