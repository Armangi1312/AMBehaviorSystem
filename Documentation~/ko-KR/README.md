<p align="center">
    <img src="https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/BannerDark.png#gh-dark-mode-only" width="500">
    <img src="https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/BannerLight.png#gh-light-mode-only" width="500">
</p>

<p align="center">
    <a href="https://github.com/Armangi1312/AMBehaviorSystem/releases"><img src="https://img.shields.io/github/v/release/Armangi1312/AMBehaviorSystem"/></a>
    <a href="https://github.com/Armangi1312/AMBehaviorSystem/blob/main/LICENSE"><img src="https://img.shields.io/github/license/Armangi1312/AMBehaviorSystem"/></a>
    <img src="https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity"/>
</p>

> [!CAUTION]
> 이 프로젝트는 현재 알파 단계로 안전하지 않을 수 있습니다.
> [Github Issues](https://github.com/Armangi1312/AMBehaviorSystem/issues)에 버그를 보고해주시면 감사하겠습니다.

---

- [한국어](https://github.com/Armangi1312/AMBehaviorSystem/tree/main/Documentation~/ko-KR/README.md)
- [English](https://github.com/Armangi1312/AMBehaviorSystem/blob/main/Documentation~/en-US/README.md)
- [中文](https://github.com/Armangi1312/AMBehaviorSystem/blob/main/Documentation~/zh-CN/README.md)

---


## 1. 소개

이 프로젝트는 유니티 엔진 용 행동 시스템 프레임워크입니다.
- **Setting**, **Context**, **Processor**, **Pipeline**로 높은 재사용성과 유연성을 제공합니다.
- 유니티 에디터에서 **시각적**으로 편집할 수 있습니다.
- **GC를 거의 발생시키지 않도록** 설계되었습니다.

### 각 요소의 역할

| 요소       | 역할 |
|------------|----------------------------------------------|
| Setting    | 행동의 설정을 정의하는 런타임 준불변 객체입니다. |
| Context    | 행동의 실행 상태를 관리하는 런타임 가변 객체입니다. |
| Processor  | Context와 Setting을 이용하여 행동의 실행 로직을 처리하는 객체입니다. |
| Pipeline   | 행동의 실행 흐름을 관리하는 객체입니다. |

### 인스펙터
![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot1.png)

유니티 에디터에서는 위 사진과 같이 보입니다. **Setting**과 **Context**, **Processor** 각각의 인스펙터에서 편집할 수 있습니다.
예를 들어서 이동은 가능하지만 점프는 불가능한 행동을 만들고 싶다면, **JumpProcessor**를 제거하면 됩니다.
AM Behavior System은 이런 동작들의 조합을 시각적으로 쉽게 만들어 낼 수 있도록 설계되었습니다. 높은 재활용성과 유연성을 제공하여 다양한 행동을 쉽게 만들 수 있습니다.

---

## 2. 설치

AM Behavior System은 유니티 패키지 매니저에서 설치할 수 있습니다.
유니티 패키지 매니저를 열고 좌측 상단에 있는 `+` 버튼을 클릭한 후 `Add package from git URL...`을 선택합니다.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot2.png)

`https://github.com/Armangi1312/AMBehaviorSystem.git` 을 복사 붙여넣고 `Add` 버튼을 클릭하면 설치가 시작됩니다.
설치가 다 되면 이제부터 AM Behavior System을 사용할 수 있습니다.
다음과 같은 샘플이 제공됩니다.

- **1. Basic**: AM Behavior System의 기본적인 사용법을 보여주는 샘플입니다.
- **2. Platformer**: 플랫폼 게임에서 흔히 볼 수 있는 행동들을 보여주는 샘플입니다.
- **3. FPS**: FPS 게임에서 흔히 볼 수 있는 행동들을 보여주는 샘플입니다.

---

## 3. 이외로

추가적인 사용법 및 자세한 설명은 [Documentation](https://github.com/Armangi1312/AMBehaviorSystem/tree/main/Documentation~/ko-KR/DOCUMENTATION.md)를 참고해주세요. 