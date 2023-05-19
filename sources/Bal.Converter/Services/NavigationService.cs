using System.Diagnostics.CodeAnalysis;
using Bal.Converter.Contracts.ViewModels;
using Bal.Converter.Extensions;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Bal.Converter.Services;

// For more information on navigation between pages see
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WinUI/navigation.md
public class NavigationService : INavigationService
{
    private readonly IPageService pageService;
    private object? lastParameterUsed;
    private Frame? frame;

    public event NavigatedEventHandler? Navigated;

    public Frame? Frame
    {
        get
        {
            if (this.frame == null)
            {
                this.frame = App.MainWindow.Content as Frame;
                this.RegisterFrameEvents();
            }

            return this.frame;
        }

        set
        {
            this.UnregisterFrameEvents();
            this.frame = value;
            this.RegisterFrameEvents();
        }
    }

    [MemberNotNullWhen(true, nameof(Frame), nameof(frame))]
    public bool CanGoBack
    {
        get => Frame != null && Frame.CanGoBack;
    }

    public NavigationService(IPageService pageService)
    {
        this.pageService = pageService;
    }

    private void RegisterFrameEvents()
    {
        if (this.frame != null)
        {
            this.frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (this.frame != null)
        {
            this.frame.Navigated -= OnNavigated;
        }
    }

    public bool GoBack()
    {
        if (this.CanGoBack)
        {
            object? vmBeforeNavigation = this.frame.GetPageViewModel();
            this.frame.GoBack();

            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }

            return true;
        }

        return false;
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = this.pageService.GetPageType(pageKey);

        if (this.frame != null && (this.frame.Content?.GetType() != pageType || (parameter != null && !parameter.Equals(lastParameterUsed))))
        {
            this.frame.Tag = clearNavigation;
            object? vmBeforeNavigation = this.frame.GetPageViewModel();

            bool navigated = this.frame.Navigate(pageType, parameter);

            if (navigated)
            {
                lastParameterUsed = parameter;
                if (vmBeforeNavigation is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedFrom();
                }
            }

            return navigated;
        }

        return false;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            if (frame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }

            this.Navigated?.Invoke(sender, e);
        }
    }
}
