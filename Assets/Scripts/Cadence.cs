using DapperLabs.Flow.Sdk.Unity;
using UnityEngine;

public class Cadence : MonoBehaviour
{
    public static Cadence Instance { get; private set; }

    [Header("Contracts")]
    public CadenceContractAsset greeting;

    [Header("Transactions")]
    public CadenceTransactionAsset changeGreeting;

    [Header("Scripts")]
    public CadenceScriptAsset readGreeting;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}