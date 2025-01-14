using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class PlayerNetwork : NetworkBehaviour
{
    // Must be used in conjunction with the NetworkBehaviour Parent
    // NetwrokVariable may only contain value types or custom types
    // NetworkVariable must be initialised on definition or as soon as the object is created
    // "Everyone" can read but only the "Owner" can write
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData
        {_int = 56, _bool = true}, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        
        // This type of string to ensure the string is a value type
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }
    
    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }
    
    void Update()
    {
        // To ensure the following code is only for the owner
        if (!IsOwner) return;

        // Used for testing purposes of the NetworkVariable
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestClientRpc(new ClientRpcParams{Send = new ClientRpcSendParams{TargetClientIds = new List<ulong>(1)}});
            // TestServerRpc();
            // randomNumber.Value = new MyCustomData{_int = 10, _bool = false, message = "Testing strings"};
        }
        
        // Simple movement code to test networking
        Vector3 moveDirection = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) moveDirection.z = +2f;
        if (Input.GetKey(KeyCode.S)) moveDirection.z = -2f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -2f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +2f;
        float moveSpeed = 3f;
        transform.position += moveDirection * (moveSpeed * Time.deltaTime);
    }

    // The function must end with Rpc to no cause errors in the console
    // With ServerRpcs the code only runs on the Server not the Client
    // ServerRpcParams contain a Send/Recieve
    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }

    // Used to send a message from the Server to one or more Clients
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("TestClientRpc");
    }
    
}