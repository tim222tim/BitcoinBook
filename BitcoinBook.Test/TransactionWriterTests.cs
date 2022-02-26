﻿using System;
using System.IO;
using Xunit;

namespace BitcoinBook.Test;

public class TransactionWriterTests
{
    [Theory]
    [InlineData("01", 1UL, 1)]
    [InlineData("01000000", 1UL, 4)]
    [InlineData("02000000", 2, 4)]
    [InlineData("120100", 274, 3)]
    [InlineData("ef01737a00000000", 2054357487, 8)]
    public void WriteIntTest(string expected, ulong i, int count)
    {
        Assert.Equal(expected, GetResult(w => w.Write(i, count)));
    }

    [Theory]
    [InlineData("00", 0)]
    [InlineData("01", 1)]
    [InlineData("fc", 252)]
    [InlineData("fdff00", 255)]
    [InlineData("fd0101", 257)]
    [InlineData("feef01737a", 2054357487)]
    [InlineData("ff0102030405060708", 578437695752307201)]
    public void WriteVarIntTests(string expected, ulong i)
    {
        Assert.Equal(expected, GetResult(w => w.WriteVar(i)));
    }

    [Fact]
    public void WriteTransaction1Test()
    {
        var transaction = new TransactionReader(hex1).ReadTransaction();
        Assert.Equal(hex1, GetResult(w => w.Write(transaction)));
    }

    [Fact]
    public void WriteTransaction2Test()
    {
        var transaction = new TransactionReader(hex2AltNoWitness).ReadTransaction();
        Assert.Equal(hex2AltNoWitness, GetResult(w => w.Write(transaction)));
    }

    [Fact]
    public void WriteTransaction3Test()
    {
        var transaction = new TransactionReader(hex3).ReadTransaction();
        Assert.Equal(hex3, GetResult(w => w.Write(transaction)));
        Assert.Equal("4ea6e2222c4d59dea646e21a103d8b812a6db433f8ca331778a9408990fa17ee", transaction.Id);
    }

    [Fact]
    public void WriteTransaction4Test()
    {
        var transaction = new TransactionReader(hex4WithWitnessData).ReadTransaction();
        Assert.Equal(hex4WithWitnessData, GetResult(w => w.Write(transaction)));
        Assert.Equal("b7773b4204686925e0cf607fb03250f0a18ce35cda48ac3ca8c004c33a9c3841", transaction.Id);
    }

    [Fact]
    public void TransactionIdTest()
    {
        var bytes = Cipher.Hash256(Cipher.ToBytes(hex3));
        Array.Reverse(bytes);
        Assert.Equal("4ea6e2222c4d59dea646e21a103d8b812a6db433f8ca331778a9408990fa17ee", bytes.ToHex());
    }

    static string GetResult(Action<TransactionWriter> action)
    {
        var stream = new MemoryStream();
        var writer = new TransactionWriter(stream);
        action(writer);
        var array = stream.ToArray();
        return array.ToHex();
    }

    const string hex1 = "010000000456919960ac691763688d3d3bcea9ad6ecaf875df5339e148a1fc61c6ed7a069e010000006a47304402204585bcdef85e6b1c6af5c2669d4830ff86e42dd205c0e089bc2a821657e951c002201024a10366077f87d6bce1f7100ad8cfa8a064b39d4e8fe4ea13a7b71aa8180f012102f0da57e85eec2934a82a585ea337ce2f4998b50ae699dd79f5880e253dafafb7feffffffeb8f51f4038dc17e6313cf831d4f02281c2a468bde0fafd37f1bf882729e7fd3000000006a47304402207899531a52d59a6de200179928ca900254a36b8dff8bb75f5f5d71b1cdc26125022008b422690b8461cb52c3cc30330b23d574351872b7c361e9aae3649071c1a7160121035d5c93d9ac96881f19ba1f686f15f009ded7c62efe85a872e6a19b43c15a2937feffffff567bf40595119d1bb8a3037c356efd56170b64cbcc160fb028fa10704b45d775000000006a47304402204c7c7818424c7f7911da6cddc59655a70af1cb5eaf17c69dadbfc74ffa0b662f02207599e08bc8023693ad4e9527dc42c34210f7a7d1d1ddfc8492b654a11e7620a0012102158b46fbdff65d0172b7989aec8850aa0dae49abfb84c81ae6e5b251a58ace5cfeffffffd63a5e6c16e620f86f375925b21cabaf736c779f88fd04dcad51d26690f7f345010000006a47304402200633ea0d3314bea0d95b3cd8dadb2ef79ea8331ffe1e61f762c0f6daea0fabde022029f23b3e9c30f080446150b23852028751635dcee2be669c2a1686a4b5edf304012103ffd6f4a67e94aba353a00882e563ff2722eb4cff0ad6006e86ee20dfe7520d55feffffff0251430f00000000001976a914ab0c0b2e98b1ab6dbf67d4750b0a56244948a87988ac005a6202000000001976a9143c82d7df364eb6c75be8c80df2b3eda8db57397088ac46430600";
    const string hex2AltNoWitness = "010000010158f85521054b0ef5009046c01440f45a49851c613e4544a9c43e877511dbd96e0100000000ffffffff0220d06d06000000001976a9146c0e16d43b03f94c4673373b8fb8547eb4ff543588ac40fd260100000000220020701a8d401c84fb13e6baf169d59684e17abd9fa216c8cc5b9fc63d622ff8c58d04004730";
    const string hex3 = "0100000001be2ae9a2a3d91cdbbc53aca340a0837416519b129f627239a7dcf7fd069ae074000000006a473044022035a874a246f4de3570295fa8e32ca48a3eb1cf3a4bea6cbea6d18f122f2da51a02204ee9fe995e4934445d381be89b5635bf16bcf9bd023d81e5dc54991d7124921101210279fc02b440c755d18e80add59b5f1ec9452ab8348e75ced61e47c0750408e028feffffff042ec00900000000001976a914664403549f16e3ded11e2a5049e7837269d129ed88acf8e20100000000001976a9149a0093bfcbd8cb1162fd3c3355c6a8fddba2df6488ac5512fb12000000001976a9141a225100a114159cd16dc22650bf39043ba5eaca88ac8f990f00000000001976a914888a6f53d62ea69aabd94f59e8356fc73ad1f37f88ac2dc70600";
    const string hex4WithWitnessData = "0200000000010209c06e8f02709bbd24df4601aaf7aec0622dc054ccf1efdef05c2496cc2bda360100000017160014920824945937eea43c243e24c396516da1297a27feffffff1f84bc94408b4629e817241bb510b0270750521d067d36bb134dbbbace5827890000000017160014b4eb55d256755ac15b9331a46d29a874cac6130efeffffff02d65715000000000017a9147edc432af47075df934929131c67afe121c759488740420f00000000001976a914a9b50000d3ffa8d58cafd00f192ea9730315f4f588ac0247304402201ef5e7ab4722d922271e0a671a7536e55e3b264c5c34ca4f1acc957a43c00a8c02202cdab51c896593e3d1c062a19b0198d61c9140e37b67639efe1cb065b3eb5f8b012102d1ec7fa73629f43fc7d39664819ee031656d1f992f02f6aea4989ce76479fd4e02473044022044bcc6acb9b0dce2ccb1bd3f775979634b540c98a347cb3923883a767ac8927f02201d9f13b7c37aeae5df00390cf93b22604c3dbc32fd3f0925f642368d0d5b3258012103ec8c39c7147f3baeacd3eb8c53ba712027a13b34b518f250eead782595479dcc94641900";
}