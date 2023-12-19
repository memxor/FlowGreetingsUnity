using System.Collections;
using System.Threading.Tasks;
using DapperLabs.Flow.Sdk;
using DapperLabs.Flow.Sdk.Crypto;
using DapperLabs.Flow.Sdk.DataObjects;
using DapperLabs.Flow.Sdk.Unity;
using DapperLabs.Flow.Sdk.WalletConnect;
using UnityEngine;
using TMPro;
using DapperLabs.Flow.Sdk.Cadence;

public class FlowHandler : MonoBehaviour
{
    public GameObject qrCodeCustomPrefab;
    public GameObject walletSelectCustomPrefab;
    private FlowControl.Account scriptsExecutionAccount;
    private FlowAccount flowAccount;
    private string walletAddress;

    private async void Start() 
    {
        FlowConfig flowConfig = new()
        {
            NetworkUrl = FlowConfig.TESTNETURL,
            Protocol = FlowConfig.NetworkProtocol.HTTP
        };

        FlowSDK.Init(flowConfig);
        IWallet walletProvider = new WalletConnectProvider();

        walletProvider.Init(new WalletConnectConfig
        {
            ProjectId = "c5a0e570828c856d8d6908a95e64d40c",
            ProjectDescription = "Dungeon Flow is a game developed for Flow Hackathon",
            ProjectIconUrl = "https://walletconnect.com/meta/favicon.ico",
            ProjectName = "Dungeon Flow",
            ProjectUrl = "https://linktr.ee/intotheverse",
            QrCodeDialogPrefab = qrCodeCustomPrefab,
            WalletSelectDialogPrefab = walletSelectCustomPrefab
        });

        FlowSDK.RegisterWalletProvider(walletProvider);

        scriptsExecutionAccount = new()
        {
            GatewayName = "Flow Testnet"
        };

        await AuthenticateWithWallet();
    }

    public async Task AuthenticateWithWallet()
    {
        await FlowSDK.GetWalletProvider().Authenticate("", async (string flowAddress) =>
        {
            walletAddress = flowAddress;
            await SetFlowAccout();
        }, () =>
        {
            Debug.LogError("Authentication failed!");
            return;
        });
    }

    public async Task SetFlowAccout()
    {
        var acc = await Accounts.GetByAddress(walletAddress);
        if (acc.Error != null)
        {
            Debug.LogError("Setting Flow Account Failed!");
            return;
        }
        else
        {
            flowAccount = acc;
        }
    }

    public TMP_InputField CreateUserNameText;
    public TextMeshProUGUI CreateProfileInfoText;

    public void CreateNewUser()
    {
        StartCoroutine(CallNewUserTransaction(CreateUserNameText.text));
    }

    private IEnumerator CallNewUserTransaction(string name)
    {
        var txResponse = Transactions.SubmitAndWaitUntilSealed
        (
            Cadence.Instance.createUserProfile.text,
            Convert.ToCadence(name, "String")
        );

        yield return new WaitUntil(() => txResponse.IsCompleted);

        var txResult = txResponse.Result;

        if (txResult.Error != null)
        {
            CreateProfileInfoText.text = txResult.ErrorMessage;
            yield break;
        }
        else
        {
            CreateProfileInfoText.text = "New Profile Created!";
            yield break;
        }
    }

    public TMP_InputField NewUserNameText;
    public TextMeshProUGUI NewProfileInfoText;

    public void UpdateUserName()
    {
        StartCoroutine(CallUpdateUsernameTransaction(NewUserNameText.text));
    }

    private IEnumerator CallUpdateUsernameTransaction(string name)
    {
        var txResponse = Transactions.SubmitAndWaitUntilSealed
        (
            Cadence.Instance.changeUsername.text,
            Convert.ToCadence(name, "String")
        );

        yield return new WaitUntil(() => txResponse.IsCompleted);

        var txResult = txResponse.Result;

        if (txResult.Error != null)
        {
            NewProfileInfoText.text = txResult.ErrorMessage;
            yield break;
        }
        else
        {
            NewProfileInfoText.text = "Username Changed!";
            yield break;
        }
    }

    public TextMeshProUGUI ReadProfileInfoText;
    public void ReadProfileValue()
    {
        StartCoroutine(ExecuteReadProfileScript());
    }

    private IEnumerator ExecuteReadProfileScript()
    {
        var scpRespone = scriptsExecutionAccount.ExecuteScript(
            Cadence.Instance.readUserProfile.text,
            Convert.ToCadence(walletAddress, "Address")
        );

        yield return new WaitUntil(() => scpRespone.IsCompleted);
        var scpResult = scpRespone.Result;

        if (scpResult.Error != null)
        {
            ReadProfileInfoText.text = scpResult.Error.Message;
            yield break;
        }
        else
        {
            ScriptResponseStruct response = Convert.FromCadence<ScriptResponseStruct>(scpResult.Value);
            ReadProfileInfoText.text = $"ID: {response.id}, Name: {response.name}, Address: {response.address}";
            yield break;
        }
    }

    public struct ScriptResponseStruct
    {
        public System.UInt64 id;
        public string name;
        public string address;
    }
}