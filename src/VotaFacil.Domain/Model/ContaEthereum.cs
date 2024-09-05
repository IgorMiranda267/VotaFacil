using Nethereum.RPC.Accounts;
using Nethereum.RPC.AccountSigning;
using Nethereum.RPC.NonceServices;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Signer;

namespace VotaFacil.Domain.Model
{
    public class ContaEthereum : IAccount
    {
        public string Address { get; private set; }
        public string PrivateKey { get; private set; }

        public ITransactionManager TransactionManager => throw new NotImplementedException();

        public INonceService NonceService { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IAccountSigningService AccountSigningService => throw new NotImplementedException();

        public ContaEthereum(string privateKey)
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
