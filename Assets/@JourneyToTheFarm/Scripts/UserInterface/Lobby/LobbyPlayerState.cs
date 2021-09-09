using MLAPI.Serialization;

namespace JTTF.Lobby
{
	public struct LobbyPlayerState : INetworkSerializable
    {
        public ulong clientId;
        public string playerName;
        public bool isReady;

        public LobbyPlayerState(ulong clientId, string playerName, bool isReady)
		{
            this.clientId = clientId;
            this.playerName = playerName;
            this.isReady = isReady;
		}

        public void NetworkSerialize(NetworkSerializer serializer)
		{
            serializer.Serialize(ref clientId);
            serializer.Serialize(ref playerName);
            serializer.Serialize(ref isReady);
		}
    }
}
