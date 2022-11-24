using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MauiBugCrashInLazyView;

public class VisibilityLazyView<TView> : LazyView<TView> where TView : View, new()
{
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == VisualElement.IsVisibleProperty.PropertyName)
        {
            EnsureLoadedIfVisible();
        }
    }

    // The method is public for situations when it is necessary to force manual checking of visibility
    // (for example when IsVisible is bound to some property in the view model, but the BindingContext with
    // the view model is set after the property has been changed).
    public void EnsureLoadedIfVisible()
    {
        if (IsVisible && !IsLoaded)
        {
            Debug.WriteLine("Loading lazy view...");
            LoadView();
        }
    }

    public override void LoadView()
    {
        base.LoadView();
    }
}


// https://github.com/xamarin/XamarinCommunityToolkit/blob/main/src/CommunityToolkit/Xamarin.CommunityToolkit/Views/LazyView/LazyView.shared.cs
public class LazyView<TView> : BaseLazyView where TView : View, new()
{
    public Action ActionAfterViewIsLoaded { get; set; } = null;
    public TView InnerView { get; private set; } = null;


    /// <summary>
    /// This method initialize your <see cref="LazyView{TView}"/>.
    /// </summary>
    /// <returns><see cref="ValueTask"/></returns>
    public override void LoadView()
    {
        InnerView = new TView { BindingContext = BindingContext };
        Content = InnerView;

        SetIsLoaded(true);

        ActionAfterViewIsLoaded?.Invoke();
    }
}

// https://github.com/xamarin/XamarinCommunityToolkit/blob/main/src/CommunityToolkit/Xamarin.CommunityToolkit/Views/LazyView/BaseLazyView.shared.cs
public abstract class BaseLazyView : ContentView, IDisposable
{
    internal static readonly BindablePropertyKey IsLoadedPropertyKey = BindableProperty.CreateReadOnly(nameof(IsLoaded), typeof(bool), typeof(BaseLazyView), default);

    public static readonly BindableProperty IsLoadedProperty = IsLoadedPropertyKey.BindableProperty;
    public bool IsLoaded => (bool)GetValue(IsLoadedProperty);

    protected void SetIsLoaded(bool isLoaded) => SetValue(IsLoadedPropertyKey, isLoaded);

    /// <summary>
    /// Use this method to do the initialization of the <see cref="View"/> and change the status IsLoaded value here.
    /// </summary>
    /// <returns><see cref="ValueTask"/></returns>
    public abstract void LoadView();

    public void Dispose()
    {
        if (Content is IDisposable disposable)
            disposable.Dispose();
    }

    protected override void OnBindingContextChanged()
    {
        if (Content != null && !(Content is ActivityIndicator))
            Content.BindingContext = BindingContext;
    }
}