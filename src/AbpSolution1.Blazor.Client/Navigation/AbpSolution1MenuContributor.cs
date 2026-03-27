using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AbpSolution1.Localization;
using AbpSolution1.Permissions;
using AbpSolution1.MultiTenancy;
using Volo.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;
using Localization.Resources.AbpUi;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.Users;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.Identity.Blazor;
using Microsoft.Extensions.Localization;

namespace AbpSolution1.Blazor.Client.Navigation;

public class AbpSolution1MenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public AbpSolution1MenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }

        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var localizer = context.GetLocalizer<AbpSolution1Resource>();
        string GetText(string key, string fallback)
        {
            try
            {
                if (localizer != null)
                {
                    var localized = localizer[key];
                    if (!string.IsNullOrEmpty(localized)) return localized;
                }
            }
            catch
            {
                // ignore localization errors
            }
            return fallback;
        }

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        context.Menu.AddItem(new ApplicationMenuItem(
            AbpSolution1Menus.Home,
            GetText("Menu:Home", "Home"),
            "/",
            icon: "fas fa-home",
            order: 1
        ));
        
        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);
    
        var bookStoreMenu = new ApplicationMenuItem(
            "BooksStore",
            GetText("Menu:Books", "Books"),
            icon: "fa fa-book"
        );

        context.Menu.AddItem(bookStoreMenu);

        // Add Books submenu
        bookStoreMenu.AddItem(new ApplicationMenuItem(
            "BooksStore.Books",
            GetText("Menu:Books", "Books"),
            url: "/books"
        ));

        var ToDoItemMenu = new ApplicationMenuItem(
            "ToDoItems",
            GetText("Menu:ToDoItems", "ToDoItems"),
            icon: "fa fa-list"
        );

        context.Menu.AddItem(ToDoItemMenu);

        // Add ToDoItems submenu
        ToDoItemMenu.AddItem(new ApplicationMenuItem(
            "ToDoItems.ToDoItems",
            GetText("Menu:ToDoItems", "ToDoItems"),
            url: "/ToDoItems"
        ));

        var DemoMenu = new ApplicationMenuItem(
            "Demo",
            "Demo",
            icon: "fa fa-cube"
        );

        context.Menu.AddItem(DemoMenu);

        DemoMenu.AddItem(new ApplicationMenuItem(
            "Demo.Listing",
            "Demo Management",
            url: "/demo"
        ));
    }
    
    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        if (OperatingSystem.IsBrowser())
        {
            var authServerUrl = _configuration["AuthServer:Authority"] ?? "";
            var accountResource = context.GetLocalizer<AccountResource>();

            var myAccountText = "My Account";
            try { if (accountResource != null) myAccountText = accountResource["MyAccount"]; } catch { }

            context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", myAccountText, $"{authServerUrl.EnsureEndsWith('/')}Account/Manage", icon: "fa fa-cog", order: 900, target: "_blank").RequireAuthenticated());
        }
        else
        {
            // Blazor server menu items
        }
        await Task.CompletedTask;
    }
}
