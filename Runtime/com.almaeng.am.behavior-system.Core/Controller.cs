using AMBehaviorSystem.Core.Pipelines;
using AMBehaviorSystem.Core.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Core
{
    public partial class Controller<TSetting, TContext, TProcessor> : MonoBehaviour
        where TSetting : ISetting
        where TContext : IContext
        where TProcessor : Processor<TSetting, TContext>
    {
        //==== 공유 프로퍼티 ====//

        /// <summary>
        /// 런타임 준불변 데이터를 저장하는 객체입니다.
        /// </summary>
        public Registry<TSetting> Settings => settings;
        [SerializeField] private ObservableRegistry<TSetting> settings = new();

        /// <summary>
        /// 런타임 가변 데이터를 저장하는 객체입니다.
        /// </summary>
        public Registry<TContext> Contexts => contexts;
        [SerializeField] private ObservableRegistry<TContext> contexts = new();

        /// <summary>
        /// 게임 로직을 처리하는 모듈 단위의 프로세서를 저장하는 객체입니다.
        /// </summary>
        public List<TProcessor> Processors => processors;
        [SerializeField] private ObservableList<TProcessor> processors = new();

        //[SerializeField] protected PipelineGraph Graph;
        public Pipeline Pipeline { get; private set; }

        //==== 내부 프로퍼티 ====//

        protected bool IsInitialized;

        #region 초기화
        protected virtual void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;

            ValidateDependencies();
            InitializeProcessors();
            InitializePipeline();
            RegistryEvent();
        }

        private void InitializeProcessors()
        {
            for (int i = 0; i < processors.Count; i++)
            {
                TProcessor processor = processors[i];

                if (processor == null)
                {
                    Debug.LogWarning($"Processor at index {i} is null.");
                    continue;
                }

                processor.Initialize(settings, contexts);
            }
        }

        private void InitializePipeline()
        {
            //Pipeline = PipelineFactory.CreatePipeline(Graph);
        }
        #endregion

        #region 의존성 검증

        protected void ValidateDependencies()
        {
            CollectDependencies(out List<Type> settingTypes, out List<Type> contextTypes);
            SyncDependencies(settingTypes, contextTypes);
        }

        private void CollectDependencies(out List<Type> settingTypes, out List<Type> contextTypes)
        {
            settingTypes = new List<Type>();
            contextTypes = new List<Type>();

            for (int i = 0; i < processors.Count; i++)
            {
                TProcessor processor = processors[i];
                if (processor == null) continue;

                (Type[] Context, Type[] Setting) dependencies = ProcessorDependencyValidator.GetRequiredTypes(processor.GetType());

                settingTypes.AddRange(dependencies.Setting);
                contextTypes.AddRange(dependencies.Context);
            }
        }

        private void SyncDependencies(List<Type> settingTypes, List<Type> contextTypes)
        {
            SyncSettingDependencies(settingTypes);
            SyncContextDependencies(contextTypes);
        }

        private void SyncSettingDependencies(List<Type> settingTypes)
        {
            for (int i = 0; i < settingTypes.Count; i++)
            {
                Type settingType = settingTypes[i];

                if (!Settings.Contains(settingType))
                    Settings.Register(Activator.CreateInstance(settingType));
            }
        }

        private void SyncContextDependencies(List<Type> contextTypes)
        {
            for (int i = 0; i < contextTypes.Count; i++)
            {
                Type contextType = contextTypes[i];

                if (!Contexts.Contains(contextType))
                    Contexts.Register(Activator.CreateInstance(contextType));
            }
        }

        #endregion

        #region 이벤트 핸들러 등록

        protected virtual void RegistryEvent()
        {
            settings.OnUnregistered += OnSettingUnregistered;
            contexts.OnUnregistered += OnContextUnregistered;
            processors.OnAdded += OnProcessorAdded;
        }

        protected virtual void UnregistryEvent()
        {
            settings.OnUnregistered -= OnSettingUnregistered;
            contexts.OnUnregistered -= OnContextUnregistered;
            processors.OnAdded -= OnProcessorAdded;
        }

        private void OnSettingUnregistered(Type type, TSetting setting)
        {
            ValidateDependencies();
        }

        private void OnContextUnregistered(Type type, TContext context)
        {
            ValidateDependencies();
        }

        private void OnProcessorAdded(TProcessor processor)
        {
            if (processor == null) return;

            ValidateDependencies();
            processor.Initialize(settings, contexts);
        }

        #endregion

        #region 실행
        protected virtual void InvokeProcessors(InvokeTiming timing)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                TProcessor processor = processors[i];

                if (processor == null || (processor.InvokeTiming & timing) == 0) 
                    continue;

                processor.Process();
            }
        }

        private void Awake()
        {
            Initialize();

            InvokeProcessors(InvokeTiming.Awake);
        }

        private void Start()
        {
            InvokeProcessors(InvokeTiming.Start);
        }

        private void Update()
        {
            InvokeProcessors(InvokeTiming.Update);
        }

        private void FixedUpdate()
        {
            InvokeProcessors(InvokeTiming.FixedUpdate);
        }

        private void LateUpdate()
        {
            InvokeProcessors(InvokeTiming.LateUpdate);
        }

        private void OnDestroy()
        {
            InvokeProcessors(InvokeTiming.Destroy);
            UnregistryEvent();
        }

        private void OnEnable()
        {
            InvokeProcessors(InvokeTiming.OnEnable);
        }

        private void OnDisable()
        {
            InvokeProcessors(InvokeTiming.OnDisable);
        }
        #endregion
    }
}
