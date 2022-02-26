using System;
using System.Net.Http;
using Xunit;

namespace BitcoinBook.Test;

public class SignerMapTests
{
    readonly SignerMap signerMap;

    public SignerMapTests()
    {
        var fetcher = new TransactionFetcher(new HttpClient());
        var hasher = new TransactionHasher(null!);

        signerMap = new SignerMap(
            new PayToPubKeySigner(fetcher, hasher), 
            new PayToPubKeyHashSigner(fetcher, hasher));
    }

    [Theory]
    [InlineData(typeof(PayToPubKeySigner), ScriptType.PayToPubKey)]
    [InlineData(typeof(PayToPubKeyHashSigner), ScriptType.PayToPubKeyHash)]
    public void SignerTest(Type signerType, ScriptType scriptType)
    {
        Assert.IsType(signerType, signerMap[scriptType]);
    }
}