using System.Collections.Generic;

namespace EthereumTransactions.Services.ThirdPartyServices.Dtos.Infura
{
	public class Transaction
    {
        public string BlockHash { get; set; }
        public string BlockNumber { get; set; }
        public string From { get; set; }
        public string Gas { get; set; }
        public string GasPrice { get; set; }
        public string Hash { get; set; }
        public string Input { get; set; }
        public string Nonce { get; set; }
        public string R { get; set; }
        public string S { get; set; }
        public string To { get; set; }
        public string TransactionIndex { get; set; }
        public string Type { get; set; }
        public string V { get; set; }
        public string Value { get; set; }
    }

    public class Result
    {
        public string Difficulty { get; set; }
        public string ExtraData { get; set; }
        public string GasLimit { get; set; }
        public string GasUsed { get; set; }
        public string Hash { get; set; }
        public string LogsBloom { get; set; }
        public string Miner { get; set; }
        public string MixHash { get; set; }
        public string Nonce { get; set; }
        public string Number { get; set; }
        public string ParentHash { get; set; }
        public string ReceiptsRoot { get; set; }
        public string Sha3Uncles { get; set; }
        public string Size { get; set; }
        public string StateRoot { get; set; }
        public string Timestamp { get; set; }
        public string TotalDifficulty { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string TransactionsRoot { get; set; }
        public List<object> Uncles { get; set; }
    }

    public class GetBlockByNumberResponse
    {
        public string Jsonrpc { get; set; }
        public int Id { get; set; }
        public Result Result { get; set; }
    }
}
