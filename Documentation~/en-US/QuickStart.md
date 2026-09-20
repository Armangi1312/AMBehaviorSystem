# Quick Start Guide

## 1. Installation

AM Behavior System can be installed through the Unity Package Manager.
In the Unity Editor's top menu, select `Window > Package Management > Package Manager`.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/ScreenShot2.png)

As shown above, click the `+` button in the top-left corner and select `Add package from git URL...`.

Paste in `https://github.com/Armangi1312/AMBehaviorSystem.git` and click `Install`.

Once loading finishes, `AM Behavior System` will be installed successfully.

---

## 2. Simple Example

## 2.1 Creating a Context, Setting, Processor, and Controller

In your Unity project, create a new folder, then `Right-click > Create > MonoBehavior Script`.
Name it `SwingObjectSetting`.

Open the file and enter the following code:

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

Create a new script named `SwingObjectContext` and enter the following code:

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

`SwingObjectSetting` and `SwingObjectContext` define the settings and state of the behavior.
Now you need to create a **Processor** class that actually performs the behavior.

Create a new script file named `SwingObjectProcessor` and enter the following code:

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

    // Defines the Unity lifecycle point when this processor will be invoked.
    public override InvokeTiming InvokeTiming => InvokeTiming.Update;

    // Initializes the setting and context used by this processor.
    public override void Initialize(IReadOnlyRegistry<ISwingObjectSetting> settings, IReadOnlyRegistry<ISwingObjectContext> contexts, Component owner)
    {
        setting = settings.Get<SwingObjectSetting>();
        context = contexts.Get<SwingObjectContext>();

        objectTransform = owner.GetComponent<Transform>();
    }

    // The method where the processor performs its behavior. Called according to InvokeTiming.
    public override void Process()
    {
        Vector3 position = objectTransform.position;
        objectTransform.position = new Vector3(position.x, Mathf.Sin(context.CurrentValue) * setting.SwingAmplitude, position.z);

        context.CurrentValue += setting.SwingSpeed * Time.deltaTime;
    }
}

```

Finally, you need to create a controller. Create a new script named `SwingObjectController` and enter the following code:
```csharp
using AMBehaviorSystem;

public class SwingObjectController : Controller<ISwingObjectSetting, 
				     ISwingObjectContext, 
				     BaseSwingObjectProcessor> 
{ }
```

Back in the Unity Editor, create a cube in the scene.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image1.png)

Select the cube and click `Add Component` to add `SwingObjectController`.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image2.png)

Next, select the `SwingObjectController` component and click the `+` button to add entries to `Settings` and `Contexts`.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image3.png)

Press Play, and you'll see the cube moving up and down.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image4.png)

---

## 2.2 Using Pipelines

Now let's build a more complex behavior. This time, we'll use a pipeline to run multiple processors based on a specific condition.

In your Unity project, `Right-click > Create > Pipelines Graph` to create a new pipeline graph, and name it `SwingPipeline`.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image5.png)

Next, on the controller component, click `Create or Select Pipeline` and select `SwingPipeline`.

Click `Open Pipeline Graph` to open it, and the pipeline graph will appear.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image6.png)

Select the Controller and configure the nodes as shown below.

![Image](https://raw.githubusercontent.com/Armangi1312/AMBehaviorSystem/main/Documentation~/Images/QuickStart_Image7.png)

Press `Ctrl+S` to save, then press Play. You'll see the cube swinging up and down only while its scale is less than 2.

## 3. Beyond This

For more examples and usage details, install the Sample to check them out.
