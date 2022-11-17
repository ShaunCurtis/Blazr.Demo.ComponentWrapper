# Creating Wrapper Components.


It's relatively easy to do this:

```
<h3>DivContainerComponent</h3>
<div class="bg-dark text-white m-3 p-2">
    @ChildContent
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

However, there are situations where this doesn't quite fit.  

1. You want to enforce a wrapper layout around any content entered in the component.
2. You need all the code under one roof to access the shared data mand methods.

Forms are a good example.  You want to impose a standard edit or view form wrapper to all your data forms.  It's just the internal field definitions that are specific.  You can use the method above, but it's clumpsy and contains repetitive code.

Unfortunately the standard Blazor component wasn't designed for this.  There are workarounds, but none are nice.

To implement this more cleanly we need a new componwent where we control how the component render fragment is built.

For this we'll use my lean, mean, green component.  There's a refernce link in the Appendix.

First this is a wrapper in action.  Note:

1. There's no content in the main razor block.  The contents will get built into the base `BuildRenderTree` which we ignore.
2. The wrapper markup is defined in a separate `Wrapper` render fragment.
3. `@this.Content` is where the content from the child will be rendered.

```csharp
@inherits UIWrapperComponentBase

@code {
    protected override RenderFragment Wrapper => (__builder) =>
    {
        <div class="bg-primary text-white p-3 b-2">
        @this.Content
        </div>
    };
}
```

Now `UIWrapperComponentBase`.

Note:

1. `Wrapper` defined as abstract.  It must be implemented in child classes.
2. `this.BuildRenderTree(builder)` is assigned to `Content`.  `this.BuildRenderTree(builder)` contains the compiled Razor code that represents the child component's content. 
3. The CTor caches the component `renderFragment` for performance.  If not hidden it renders the contents of `Wrapper` if it's not null, or the child content directly.

```csharp
public abstract class UIWrapperComponentBase : UIComponentBase
{
    protected virtual RenderFragment? Wrapper { get; }
    protected RenderFragment? Content => (builder) => this.BuildRenderTree(builder);

    public UIWrapperComponentBase()
    {
        this.renderFragment = builder =>
        {
            hasPendingQueuedRender = false;
            hasNeverRendered = false;
            var hide = this.hide | this.Hidden;

            if (hide)
                return;

            if (this.Wrapper is not null)
            {
                this.Wrapper.Invoke(builder);
                return;
            }

            BuildRenderTree(builder);
        };
    }
}
```

## Demo

Here's a simple demo setting `Index` to inherit from `MyWrapper`.

The result:
 
![Wrapper Demo](./assets/Wrapper-Components/wrapper-demo.png)







## Appendix

[The Lean, Mean, Green Component]()
