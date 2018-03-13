namespace OutOfPaper.Drivers.Commands
{
    interface ICommandable
    {
        void LoginCachier(int cachierPassword);
        void LogoutCachier();

    }
}
