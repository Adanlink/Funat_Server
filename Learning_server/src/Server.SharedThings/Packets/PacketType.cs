namespace Server.SharedThings.Packets
{
    public enum PacketType : uint
    {
        #region LoginServer
        RegisterRequest,
        RegisterResponse,
        LoginRequest,
        LoginSucceeded,
        LoginFailed,
        #endregion

        #region WorldMenu
        SessionLoginRequest,
        SessionLoginResponse,
        CharacterListRequest,
        CharacterListResponse,
        CharacterCreateRequest,
        CharacterCreateResponse,
        CharacterDeleteRequest,
        CharacterDeleteResponse,
        CharacterSelectRequest,
        CharacterSelectResponse,
        #endregion
        
        DeclareEntity,
        DeclareEntities,
        RevokeEntity,
        RevokeEntities,
        EntityUpdatePosition,
        MovementDirectionRequest,
        PlayerUpdateOwnPosition,
        PlayerStart,
        ClientSendChatMessage,
        ServerSendChatMessage
    }
}