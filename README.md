# AM Behavior System

---

## 1. 개요

AM Behavior System은 모듈형 동작 시스템을 구현하기 위한 프레임워크입니다. 
이 시스템은 다양한 행동을 정의하고, 
이를 조합하여 복잡한 행동을 생성할 수 있도록 설계되었습니다.

이 시스템은 게임 오브젝트에 다양한 동작을 쉽게 추가하고 관리할 수 있도록 설계되었습니다.

각 동작은 독립적인 모듈로 구성되어 뛰어난 **재사용성**과 **유지보수성**을 제공합니다.

이 시스템은 네 가지 핵심 구성 요소로 이루어져 있습니다.

- **Setting** - 런타임 시 불변 또는 반불변 데이터를 저장합니다.

- **Context** - 런타임 시 현재 상태를 나타냅니다.

- **Processor** - 실제 동작 로직을 실행하는 유닛입니다.

- **Pipeline** - 조건에 따라 동작의 분기를 설정할 수 있는 로직입니다.

---

## 2. 주요 기능

### Unity 인스펙터 지원

- 설정, 프로세서, 컨텍스트를 인스펙터에서 간편하게 구성할 수 있습니다.

- 디자이너와 개발자가 직관적으로 동작을 협업하여 조정할 수 있습니다.

### 유연한 동작 구성

- 프로세서 구성을 변경하여 다양한 동작을 생성할 수 있습니다.

- 높은 확장성과 재사용성을 제공합니다.

### 최소한의 런타임 GC

- 런타임 중 GC 할당이 거의 발생하지 않습니다.

- 프레임 드롭을 방지하고 성능을 향상시킵니다.

---

## 3. 사용 방법

### 3.1 설치 방법

- 유니티 패키지 메니저로 이동을 합니다.
- Git URL로 패키지 추가를 선택하세요.
- `https://github.com/Armangi1312/AMBehaviorSystem.git`를 입력 후, 설치를 누르세요.

### 3.2 사용 방법

#### 각 요소의 역할

| 이름 | 설명 |
| --- | --- |
| Setting   | 런타임 준불변 데이터를 저장					 |
| Context   | 런타임 현재 상태를 나타냄				     | 
| Processor | 실제 동작 로직을 실행하는 유닛			     | 
| Pipeline  | 조건에 따라 동작의 분기를 설정하는 로직		 | 

#### 주의 사항

- Setting, Context, Processor는 모두 `[Serializable]` 어트리뷰트로 직렬확가 가능해야 합니다.
- Public 필드, `[SerializeField]`로 직렬화된 필드, 직렬화 가능한 속성 등만 인스펙터에서 편집할 수 있습니다.

#### Setting

```csharp
// ISetting을 상속받는 인터페이스를 정의하여 Setting을 정의하는 것이 권장됩니다.
public interface IExSetting : ISetting {}

// 직렬화가 가능해야 하며, ISetting 또는 ISetting을 구현하는 인터페이스를 상속 받아야 합니다.
[Serializable]
public class ExSetting1 : IExSetting
{
    // Public 필드 예시.
    public float Speed;
    public float JumpHeight;

    // [SerializeField] 필드 예시.
    [SerializeField] private float Speed
    [SerializeField] private float JumpHeight;

    [field: SerializeField] public float MaxHealth ;

}
```