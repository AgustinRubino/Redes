using Fusion;

namespace Host
{
    public struct InputData : INetworkInput
    {
        public const byte MouseButton0 = 1;

        public NetworkButtons Buttons;

        public sbyte right;
        public sbyte forward;
        public bool jump;
        public bool dash;
    }
}