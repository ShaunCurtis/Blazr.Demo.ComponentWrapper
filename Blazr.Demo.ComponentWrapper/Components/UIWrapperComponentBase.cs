/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazr.UI;

/// <summary>
/// Component for building Wrapper UI Components
/// </summary>
public abstract class UIWrapperComponentBase : UIComponentBase
{
    /// <summary>
    /// This is the Wrapper Content where we define the wrapper content
    /// Use @Content for the chidl content
    /// </summary>
    protected abstract RenderFragment? Wrapper { get; }

    // This is where we capture the content from the chidl component
    // The Blazor compiler overrides BuildRenderTree with this content
    protected RenderFragment? Content => (builder) => this.BuildRenderTree(builder);

    // Ctor that caches the component render fragment
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
