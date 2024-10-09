using Nethereum.RPC.Accounts;
using Nethereum.RPC.AccountSigning;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VotaFacil.Domain.Model
{
    [Table("conta_ethereum")]
    public class ContaEthereumModel : IAccount
    {
        [Key, Column("id")] public Guid Id { get; set; }

        [Required, Column("address"), MaxLength(42)] public string Address { get; private set; }

        [Required, Column("private_key"), MaxLength(64)] public string PrivateKey { get; private set; }

        [NotMapped] public ITransactionManager TransactionManager { get; private set; }

        [NotMapped] public INonceService NonceService { get; set; }

        [NotMapped] public IAccountSigningService AccountSigningService { get; private set; }
        private readonly Web3 _web3;

        public ContaEthereumModel(string privateKey)
        {
            var account = new Account(privateKey);
            var id = Environment.GetEnvironmentVariable("INFURA_ETHEREUM_ID_ACCOUNT");
            var url = Environment.GetEnvironmentVariable("INFURA_ETHEREUM_CONTRACT_ADDRESS");

            PrivateKey = privateKey;
            Address = new EthECKey(privateKey).GetPublicAddress();
            _web3 = new Web3($"{url}{id}");
            TransactionManager = account.TransactionManager;
            NonceService = new InMemoryNonceService(Address, _web3.Client);
            AccountSigningService = new AccountSigningService(_web3.Client);
        }

        public EthECKey GetKey()
        {
            return new EthECKey(PrivateKey);
        }
    }
}
