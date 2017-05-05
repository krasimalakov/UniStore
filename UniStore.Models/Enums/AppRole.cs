namespace UniStore.Models.Enums
{
    using System.ComponentModel;

    public enum AppRole
    {
        [Description("Администратор")] Administrator,
        User,
        Sealer,
        Accountant
    }
}