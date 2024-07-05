using BookStore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace BookStore.Permissions;

public class BookStorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var bookStoreGroup = context.AddGroup(BookStorePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(BookStorePermissions.MyPermission1, L("Permission:MyPermission1"));

        var authorPermission = bookStoreGroup.AddPermission(BookStorePermissions.Authors.Default, L("Permission:Authors"));
        authorPermission.AddChild(BookStorePermissions.Authors.Create, L("Permission:Authors:Create"));
        authorPermission.AddChild(BookStorePermissions.Authors.Update, L("Permission:Authors:Update"));
        authorPermission.AddChild(BookStorePermissions.Authors.Delete, L("Permission:Authors:Delete"));
        
        //"Permission:Authors": "Authors management",
        //"Permission:Authors:Create": "Creating Authors",
        //"Permission:Authors:Update": "Editing Authors",
        //"Permission:Authors:Delete": "Deleting Authors",


    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<BookStoreResource>(name);
    }
}
