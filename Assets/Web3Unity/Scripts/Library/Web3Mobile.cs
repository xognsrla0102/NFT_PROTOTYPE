using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using UnityEngine;
using WalletConnectSharp.Core.Models.Ethereum;
using WalletConnectSharp.Unity;

public class Web3Mobile
{
	async public static Task<string> SendTransaction(string _to, string _value, string _gas, string _data)
	{
		string address = WalletConnect.ActiveSession.Accounts[0];

		BigInteger bnValue = BigInteger.Parse(_value);
		string hexVal = "0x" + bnValue.ToString("X");

		BigInteger bnGas = BigInteger.Parse(_gas);
		string hexGas = "0x" + bnGas.ToString("X");
		
		var transaction = new TransactionData()
		{
			from = address,
			to = _to,
			value = hexVal,
			gas = hexGas,
			data = _data,
		};

		string receipt = await WalletConnect.ActiveSession.EthSendTransaction(transaction);
		return receipt;
	}
}
