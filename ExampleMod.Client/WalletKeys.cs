using OblivionMp.Sdk;
using ReadyM.SDK.Attributes;
using ReadyM.Sdk.Common.Input;

namespace ExampleMod.Client;

// A service: the SDK creates one, hands it what its constructor asks for, and calls Start once
// every mod has loaded. Services can also have Update, called every frame, and Stop.
[Service]
public sealed partial class WalletKeys(IInput input, RpcHandlers rpc)
{
    private void Start()
    {
        input.RegisterKeyBind(ModifierKeys.Alt, Key.N, () => rpc.SendAddWalletBalance());
        input.RegisterKeyBind(ModifierKeys.Alt, Key.B, () => rpc.SendGetWalletBalance());
        input.RegisterKeyBind(ModifierKeys.Alt, Key.P, () => rpc.SendPing("Hello!"));
    }
}
