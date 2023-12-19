using DapperLabs.Flow.Sdk.Unity;
using UnityEngine;

public class Cadence : MonoBehaviour
{
    public static Cadence Instance { get; private set; }

    [Header("Contracts")]
    public CadenceContractAsset profile;

    [Header("Transactions")]
    public CadenceTransactionAsset createUserProfile;
    public CadenceTransactionAsset changeUsername;

    [Header("Scripts")]
    public CadenceScriptAsset readUserProfile;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
}