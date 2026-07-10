using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ReadyScreen : NetworkBehaviour
{
    [SerializeField] Button _colorBTN;
    [SerializeField] Button _carBTN;
    [SerializeField] Button _quitBTN;
    [SerializeField] Button _readyBTN;
    [Space(10)]
    [SerializeField] GameObject _colorMenu;
    [SerializeField] GameObject _carMenu;
    [Space(10)]
    [SerializeField] TMP_Text _playerName;

    private void Spawned()
    {
        _playerName.text = PlayerInfo.Data.name;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetCarModel(PlayerRef player, int index)
    {

    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Color(PlayerRef player, Color color)
    {

    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_Name(PlayerRef player, string name)
    {

    }
}
