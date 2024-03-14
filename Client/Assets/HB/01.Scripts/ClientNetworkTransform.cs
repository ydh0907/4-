using Unity.Netcode.Components;

namespace HB
{
    public class ClientNetworkTransform : NetworkTransform
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            CanCommitToTransform = IsOwner;
        }

        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
