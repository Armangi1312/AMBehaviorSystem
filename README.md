<p align="center">
    <img src="Documentation~/Images/BannerDark.png#gh-dark-mode-only" width="500">
    <img src="Documentation~/Images/BannerLight.png#gh-light-mode-only" width="500">
</p>

<p align="center">
    <a href="https://github.com/Armangi1312/AMBehaviorSystem/releases"><img src="https://img.shields.io/github/v/release/Armangi1312/AMBehaviorSystem"/></a>
    <a href="https://github.com/Armangi1312/AMBehaviorSystem/blob/main/LICENSE"><img src="https://img.shields.io/github/license/Armangi1312/AMBehaviorSystem"/></a>
    <img src="https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity"/>
</p>

---

> [!CAUTION]
> 이 프로젝트는 현재 알파 단계로 안전하지 않을 수 있습니다.

## 소개

이 프로젝트는 유니티 엔진 용 행동 시스템 프레임워크입니다.
- **Setting**, **Context**, **Processor**로 높은 재사용성과 유연성을 제공합니다.
- 유니티 에디터에서 **시각적**으로 편집할 수 있습니다.
- **GC를 거의 발생시키지 않도록** 설계되었습니다.

### 각 요소의 역할

| 요소       | 역할 |
|------------|----------------------------------------------|
| Setting    | 행동의 설정을 정의하는 런타임 준불변 객체입니다. |
| Context    | 행동의 실행 상태를 관리하는 런타임 가변 객체입니다. |
| Processor  | Context와 Setting을 이용하여 행동의 실행 로직을 처리하는 객체입니다. |              

### 예시 코드

```csharp
// Setting
[Serializable]
public class MoveSetting : IMovementSetting
{
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float SprintSpeed { get; private set; }
}

[Serializable]
public class JumpSetting : IMovementSetting
{
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float SprintSpeed { get; private set; }
}
```

```csharp
// Context
[Serializable]
public class MoveContext : IMovementContext
{
    public Vector3 Direction { get; set; }
    public bool IsSprinting { get; set; }
}

[Serializable]
public class JumpContext : IMovementContext
{
    public bool IsGrounded { get; set; }
}
```

```csharp
[Serializable]
[Require(typeof(MoveSetting, JumpSetting))]
[Require(typeof(MoveContext, JumpContext))]
public class MoveProcessor : MovementProcessor
{
    public override InvokeTiming InvokeTiming => InvokeTiming.FixedUpdate;

    public override void Initialize(IReadOnlyRegistry<IMovementSetting> settingRegistry, IReadOnlyRegistry<IMovementContext> contextRegistry)
    {
    }

    public override void Process()
    {
        ...
    }
}
```