using Nethereum.RPC.Accounts;
using Nethereum.RPC.AccountSigning;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;
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

        [NotMapped] public ITransactionManager TransactionManager => throw new NotImplementedException();

        [NotMapped] public INonceService NonceService { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        [NotMapped] public IAccountSigningService AccountSigningService => throw new NotImplementedException();

        public ContaEthereumModel(string privateKey)
        {
            PrivateKey = privateKey;
            Address = new EthECKey(privateKey).GetPublicAddress();
        }

        public EthECKey GetKey()
        {
            return new EthECKey(PrivateKey);
        }
    }
}
