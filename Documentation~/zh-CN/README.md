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
> 本项目目前处于 Alpha 阶段，可能存在不稳定的情况。
> 如发现任何问题，请通过 [Github Issues](https://github.com/Armangi1312/AMBehaviorSystem/issues) 提交 Bug 报告。

---

- [한국어](https://github.com/Armangi1312/AMBehaviorSystem/tree/main/Documentation~/ko-KR/README.md)
- [English](https://github.com/Armangi1312/AMBehaviorSystem/blob/main/Documentation~/en-US/README.md)
- [中文](https://github.com/Armangi1312/AMBehaviorSystem/blob/main/Documentation~/zh-CN/README.md)

---

## 1. 简介

AM Behavior System 是一个适用于 Unity 引擎的行为系统框架。
- 通过 **Setting**、**Context**、**Processor** 和 **Pipeline** 提供高度的可复用性与灵活性。
- 支持在 Unity 编辑器中进行**可视化编辑**。
- 设计上**尽量减少 GC 分配**，降低运行时开销。

### 各元素的职责

| 元素       | 描述 |
|------------|----------------------------------------------|
| Setting    | 定义行为配置的运行时准不可变对象。 |
| Context    | 管理行为执行状态的运行时可变对象。 |
| Processor  | 利用 Context 和 Setting 处理行为执行逻辑的对象。 |
| Pipeline   | 管理行为执行流程的对象。 |

### 检视器（Inspector）
![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot1.png)

在 Unity 编辑器中，界面如上图所示。可以分别在 **Setting**、**Context** 和 **Processor** 各自的检视器中进行编辑。
例如，若希望创建一个只能移动、不能跳跃的行为，只需移除 **JumpProcessor** 即可。
AM Behavior System 旨在让行为的组合与可视化变得简单直观，提供高度的可复用性与灵活性，帮助你轻松构建各种行为。

---

## 2. 安装

AM Behavior System 可通过 Unity Package Manager 进行安装。
打开 Unity Package Manager，点击左上角的 `+` 按钮，选择 `Add package from git URL...`。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot2.png)

粘贴 `https://github.com/Armangi1312/AMBehaviorSystem.git`，然后点击 `Add` 按钮即可开始安装。
安装完成后，即可立即开始使用 AM Behavior System。
以下示例项目已包含在内：

- **1. Basic**：展示 AM Behavior System 基本用法的示例。
- **2. Platformer**：展示平台类游戏中常见行为的示例。
- **3. FPS**：展示第一人称射击游戏中常见行为的示例。

---

## 3. 更多信息

如需了解更多用法及详细说明，请参阅 [文档](https://github.com/Armangi1312/AMBehaviorSystem/tree/main/Documentation~/zh-CN/DOCUMENTATION.md)。