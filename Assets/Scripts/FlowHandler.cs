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
            ProjectDescription = "Flow Greetings Example",
            ProjectIconUrl = "https://walletconnect.com/meta/favicon.ico",
            ProjectName = "Hello Flow",
            ProjectUrl = "",
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

    public TMP_InputField ChangeGreetingText;
    public TextMeshProUGUI ChangeGreetingInfoText;

    public void ChangeGreeting()
    {
        StartCoroutine(CallChangeGreetingTransaction(ChangeGreetingText.text));
    }

    private IEnumerator CallChangeGreetingTransaction(string greeting)
    {
        var txResponse = Transactions.SubmitAndWaitUntilSealed
        (
            Cadence.Instance.changeGreeting.text,
            Convert.ToCadence(greeting, "String")
        );

        yield return new WaitUntil(() => txResponse.IsCompleted);

        var txResult = txResponse.Result;

        if (txResult.Error != null)
        {
            ChangeGreetingInfoText.text = txResult.ErrorMessage;
            yield break;
        }
        else
        {
            ChangeGreetingInfoText.text = "Greeting Changed!";
            yield break;
        }
    }

    public TextMeshProUGUI ReadGreetingInfoText;
    public void ReadGreeting()
    {
        StartCoroutine(ExecuteReadGreetingScript());
    }

    private IEnumerator ExecuteReadGreetingScript()
    {
        var scpRespone = scriptsExecutionAccount.ExecuteScript(
            Cadence.Instance.readGreeting.text,
            Convert.ToCadence(walletAddress, "Address")
        );

        yield return new WaitUntil(() => scpRespone.IsCompleted);
        var scpResult = scpRespone.Result;

        if (scpResult.Error != null)
        {
            ReadGreetingInfoText.text = scpResult.Error.Message;
            yield break;
        }
        else
        {
            string response = Convert.FromCadence<string>(scpResult.Value);
            ReadGreetingInfoText.text = response;
            yield break;
        }
    }
}