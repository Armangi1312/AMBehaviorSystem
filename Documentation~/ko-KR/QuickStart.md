# 빠른 시작 가이드

## 1. 설치

AM Behavior System은 유니티 패키지 메니저를 통해 설치될 수 있습니다.
유니티 에디터 상단 메뉴에서 `Window > Package Management > Package Manager`를 선택합니다.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot2.png)

위 사진처럼 상단 좌측의 `+` 버튼을 누르고, `Add package from git URL...`를 선택합니다.

`https://github.com/Armangi1312/AMBehaviorSystem.git`를 붙여놓고 `Install` 버튼을 누릅니다.

로딩이 끝나면, `AM Behavior System`가 성공적으로 설치됩니다.

---

## 2. 간단한 예제

## 2.1 Context, Setting, Processor, Controller 생성

유니티 프로젝트에서 새로운 폴더를 만들고, `우클릭 > Create > MonoBehavior Script`를 선택합니다.
이름을 `SwingObjectSetting`으로 지정합니다. 

파일을 열고 다음 코드를 입력합니다:

```csharp
using AMBehaviorSystem;
using System;
using UnityEngine;

public interface ISwingObjectSetting : ISetting { }

[Serializable]
public class SwingObjectSetting : ISwingObjectSetting
{
    [field: Header("Swing Settings")]
    [field: SerializeField] public float SwingSpeed { get; private set; } = 1f;
    [field: SerializeField] public float SwingAmplitude { get; private set; } = 1f;
}
```

`SwingObjectContext`라는 이름의 새로운 스크립트를 만들고, 다음 코드를 입력합니다:

```csharp
using AMBehaviorSystem;
using System;
using UnityEngine;

public interface ISwingObjectContext : IContext {}

[Serializable]
public class SwingObjectContext : ISwingObjectContext
{
    [field: Header("Swing Contexts")]
    [field: SerializeField] public float CurrentValue { get; set; } = 0f;
}

```

`SwingObjectSetting`과 `SwingObjectContext`는 행동의 설정과 상태를 정의하는 역할을 합니다. 
이제 실제로 행동을 수행할 **Process** 클래스를 만들어야 합니다.

`SwingObjectProcessor` 새로운 스크립트 파일을 생성하고 다음 코드를 입력합니다:

```csharp
using AMBehaviorSystem;
using System;
using UnityEngine;

[Serializable]
public abstract class BaseSwingObjectProcessor : Processor<ISwingObjectSetting, ISwingObjectContext> { }

[Serializable]
public class SwingObjectProcessor : BaseSwingObjectProcessor
{
    private SwingObjectSetting setting;
    private SwingObjectContext context;
    private Transform objectTransform;

    //이 프로세서가 호출될 유니티 라이프 사이클 시점을 설정합니다. 
    public override InvokeTiming InvokeTiming => InvokeTiming.Update;

    //프로세서에서 사용할 세팅과 컨텍스트를 초기화합니다.
    public override void Initialize(IReadOnlyRegistry<ISwingObjectSetting> settings, IReadOnlyRegistry<ISwingObjectContext> contexts, Component owner)
    {
        setting = settings.Get<SwingObjectSetting>();
        context = contexts.Get<SwingObjectContext>();

        objectTransform = owner.GetComponent<Transform>();
    }

    //프로세서에서 실제로 동작을 수행하는 메서드입니다. 이 메서드는 InvokeTiming에 따라 호출됩니다.
    public override void Process()
    {
        Vector3 position = objectTransform.position;
        objectTransform.position = new Vector3(position.x, Mathf.Sin(context.CurrentValue) * setting.SwingAmplitude, position.z);

        context.CurrentValue += setting.SwingSpeed * Time.deltaTime;
    }
}

```

마지막으로 컨트롤러를 생성해야 합니다. `SwingObjectController`라는 이름의 새로운 스크립트를 만들고 다음 코드를 입력합니다:
```csharp
using AMBehaviorSystem;

public class SwingObjectController : Controller<ISwingObjectSetting, 
				     ISwingObjectContext, 
				     BaseSwingObjectProcessor> 
{ }
```

유니티 에디터로 돌아와서, 씬에 큐브를 생성합니다.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image1.png)

큐브를 선택하고, `Add Component` 버튼을 눌러 `SwingObjectController`를 추가합니다.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image2.png)

그 다음 `SwingObjectController` 컴포넌트를 선택하고, `Settings`와 `Contexts`를 `+` 버튼을 눌러 추가합니다.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image3.png)

게임을 시작해보면, 큐브가 상하로 왕복운동하는 것을 볼 수 있습니다.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image4.png)

---

## 2.2 파이프라인 사용해보기

이제 더 복잡한 행동을 만들어보겠습니다. 이번에는 파이프라인을 사용하여 특정 조건에 따라 여러 프로세서를 실행해보겠습니다.

유니티 프로젝트애서 `우클릭 > Create > Pipelines Graph`를 선택하여 새로운 파이프라인 그래프를 생성하고, 이름을 `SwingPipeline`으로 지정합니다.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image5.png)

그다음 컨트롤러 컴포넌트에서 `Create or Select Pipeline` 버튼을 눌러 `SwingPipeline`을 선택합니다.

`Open Pipeline Graph`를 눌러 열어보면 파이프라인 그래프가 나타납니다.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image6.png)

Controller를 선택하고, 노드를 다음과 같이 구성해보세요.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image7.png)

`Ctrl+S`를 눌러 저장하고, 게임을 시작하면 큐브의 크기가 2 미만일 때만 상하로 왕복운동하는 것을 볼 수 있습니다.

## 3. 이외에

더 많은 예제와 사용법은 Sample를 설치해 확인해보세요.