<p align="center">
    <img src="https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/BannerDark.png#gh-dark-mode-only" width="500">
    <img src="https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/BannerLight.png#gh-light-mode-only" width="500">
</p>

<p align="center">
    <a href="https://github.com/Armangi1312/AMBehaviorSystem/releases"><img src="https://img.shields.io/github/v/release/Armangi1312/AMBehaviorSystem"/></a>
    <a href="https://github.com/Armangi1312/AMBehaviorSystem/blob/main/LICENSE.md"><img src="https://img.shields.io/github/license/Armangi1312/AMBehaviorSystem"/></a>
    <img src="https://img.shields.io/badge/Unity-2022.3%2B-black?logo=unity"/>
</p>

<p align="center">
    <i>Stop hardcoding behaviors.<br>
    AM Behavior System lets you compose, swap, and visualize them at runtime.</i>
</p>

---

- [한국어](README.md)
- [English](README_EN.md)
- [中文]()

---


## 1. Introduction

AM Behavior System is a behavior system framework for the Unity Engine.
- Provides high reusability and flexibility through **Setting**, **Context**, **Processor**, and **Pipeline**.
- Supports **visual editing** directly in the Unity Editor.
- Designed to **minimize GC allocations** at runtime.

### Role of Each Element

| Element    | Description |
|------------|----------------------------------------------|
| Setting    | A runtime quasi-immutable object that defines the configuration of a behavior. |
| Context    | A runtime mutable object that manages the execution state of a behavior. |
| Processor  | An object that handles the execution logic of a behavior using Context and Setting. |
| Pipeline   | An object that manages the execution flow of a behavior. |

### Inspector
![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot1.png)

In the Unity Editor, it appears as shown above. Each **Setting**, **Context**, and **Processor** can be edited through their respective inspectors.
For example, if you want to create a behavior that allows movement but not jumping, simply remove the **JumpProcessor**.
AM Behavior System is designed to make it easy to compose and visualize combinations of behaviors. It provides high reusability and flexibility, allowing you to create a wide variety of behaviors with ease.

---

## 2. Installation

AM Behavior System can be installed via the Unity Package Manager.
Open the Unity Package Manager, click the `+` button in the top-left corner, and select `Add package from git URL...`.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot2.png)

Paste `https://github.com/Armangi1312/AMBehaviorSystem.git` and click the `Add` button to begin installation.
Once installed, you can start using AM Behavior System right away.

---

Please report any bugs via [Github Issues](https://github.com/Armangi1312/AMBehaviorSystem/issues).