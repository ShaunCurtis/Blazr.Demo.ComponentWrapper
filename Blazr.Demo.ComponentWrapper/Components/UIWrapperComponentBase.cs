/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazr.UI;

/// <summary>
/// Base minimum footprint component for building UI Components
/// with single PreRender event method
/// </summary>
public abstract class UIWrapperComponentBase : UIComponentBase
{
    protected virtual void BuildComponent(RenderTreeBuilder builder)
    {
        if (this.Wrapper is not null)
        {
            this.Wrapper.Invoke(builder);
            return;
        }

        BuildRenderTree(builder);
    }

    /// <summary>
    /// This is the Wrapper Contwent
    /// </summary>
    protected virtual RenderFragment? Wrapper { get; }

    protected RenderFragment? Content => (builder) => this.BuildRenderTree(builder);

    public UIWrapperComponentBase()
    {
        this.renderFragment = builder =>
        {
            hasPendingQueuedRender = false;
            hasNeverRendered = false;
            if (!(this.hide | this.Hidden))
                this.BuildComponent(builder);
        };
    }
}
