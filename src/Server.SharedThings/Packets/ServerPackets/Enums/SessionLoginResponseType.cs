namespace Server.SharedThings.Packets.ServerPackets.Enums
{
    public enum SessionLoginResponseType
    {
        SuccessfulLogin,
        AlreadyLoggedIn,
        MaxConnectionsReached,
        FailedLogin
    }
}
