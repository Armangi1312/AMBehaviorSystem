# 快速入门指南

## 1. 安装

AM Behavior System 可以通过 Unity Package Manager 安装。

在 Unity 编辑器顶部菜单中选择 `Window > Package Management > Package Manager`。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot2.png)

如上图所示，点击左上角的 `+` 按钮，然后选择 `Add package from git URL...`。

输入 `https://github.com/Armangi1312/AMBehaviorSystem.git`，然后点击 `Install`。

安装完成后，`AM Behavior System` 即可使用。

---

## 2. 简单示例

## 2.1 创建 Context、Setting、Processor 和 Controller

在 Unity 项目中新建一个文件夹，然后选择 `右键 > Create > MonoBehaviour Script`。

将其命名为 `SwingObjectSetting`。

打开文件并输入以下代码：

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

新建一个名为 `SwingObjectContext` 的脚本，并输入以下代码：

```csharp
using AMBehaviorSystem;
using System;
using UnityEngine;

public interface ISwingObjectContext : IContext { }

[Serializable]
public class SwingObjectContext : ISwingObjectContext
{
    [field: Header("Swing Contexts")]
    [field: SerializeField] public float CurrentValue { get; set; } = 0f;
}
```

`SwingObjectSetting` 和 `SwingObjectContext` 用于定义该行为的配置和状态。

接下来，创建一个实际执行该行为的 **Processor** 类。

新建一个名为 `SwingObjectProcessor` 的脚本文件，并输入以下代码：

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

    // 指定调用该 Processor 的 Unity 生命周期时机。
    public override InvokeTiming InvokeTiming => InvokeTiming.Update;

    // 初始化该 Processor 使用的 Setting 和 Context。
    public override void Initialize(IReadOnlyRegistry<ISwingObjectSetting> settings, IReadOnlyRegistry<ISwingObjectContext> contexts, Component owner)
    {
        setting = settings.Get<SwingObjectSetting>();
        context = contexts.Get<SwingObjectContext>();

        objectTransform = owner.GetComponent<Transform>();
    }

    // Processor 实际执行行为的方法。
    // 该方法会根据 InvokeTiming 被调用。
    public override void Process()
    {
        Vector3 position = objectTransform.position;
        objectTransform.position = new Vector3(position.x, Mathf.Sin(context.CurrentValue) * setting.SwingAmplitude, position.z);

        context.CurrentValue += setting.SwingSpeed * Time.deltaTime;
    }
}
```

最后，创建一个 Controller。新建一个名为 `SwingObjectController` 的脚本，并输入以下代码：

```csharp
using AMBehaviorSystem;

public class SwingObjectController : Controller<ISwingObjectSetting, 
				     ISwingObjectContext, 
				     BaseSwingObjectProcessor> 
{ }
```

回到 Unity 编辑器，在场景中创建一个 Cube。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image1.png)

选中该 Cube，点击 `Add Component`，添加 `SwingObjectController`。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image2.png)

接着选中 `SwingObjectController` 组件，点击 `+` 按钮，为 `Settings` 和 `Contexts` 添加条目。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image3.png)

点击 Play 运行游戏，Cube 将会上下移动。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image4.png)

---

## 2.2 使用 Pipeline

现在我们来构建一个更复杂的行为。这次，我们将使用 Pipeline，根据特定条件运行多个 Processor。

在 Unity 项目中，选择 `右键 > Create > Pipelines Graph` 创建一个新的 Pipeline Graph，并将其命名为 `SwingPipeline`。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image5.png)

接着，在 Controller 组件中点击 `Create or Select Pipeline`，然后选择 `SwingPipeline`。

点击 `Open Pipeline Graph` 打开 Pipeline Graph。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image6.png)

选中 Controller，并按照下图所示配置节点。

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image7.png)

按 `Ctrl+S` 保存 Pipeline Graph，然后点击 Play 运行游戏。

此时，只有当 Cube 的缩放值小于 2 时，它才会上下移动。

## 3. 更多内容

如需查看更多示例和用法，请安装 Sample。
