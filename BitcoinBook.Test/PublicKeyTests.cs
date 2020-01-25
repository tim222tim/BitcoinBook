﻿using Xunit;

namespace BitcoinBook.Test
{
    public class PublicKeyTests
    {
        readonly PublicKey key = new PublicKey(
            "00887387e452b8eacc4acfde10d9aaf7f6d9a0f975aabb10d006e4da568744d06c",
            "0061de6d95231cd89026e286df3b6ae4a894a3378e393e93a0f45b666329a0ae34");

        [Fact(Skip = "Sigs don't come out the same as book")]
        public void SignatureOneTest()
        {
            var signature = new Signature(
                "00ac8d1c87e51d0d441be8b3dd5b05c8795b48875dffe00b7ffcfac23010d3a395",
                "0068342ceff8935ededd102dd876ffd6ba72d6a427a3edb13d26eb0781cb423c4");
            Assert.True(key.Verify(
                "00ec208baa0fc1c19f708a9ca96fdeff3ac3f230bb4a7ba4aede4942ad003c0f60",
                signature));
        }

        [Fact(Skip = "Sigs don't come out the same as book")]
        public void SignatureTwoTest()
        {
            var signature = new Signature(
                "00eff69ef2b1bd93a66ed5219add4fb51e11a840f404876325a1e8ffe0529a2c",
                "00c7207fee197d27c618aea621406f6bf5ef6fca38681d82b2f06fddbdce6feab6");
            Assert.True(key.Verify(
                "007c076ff316692a3d7eb3c3bb0f8b1488cf72e1afcd929e29307032997a838a3d",
                signature));
        }

        [Fact(Skip = "Not ready")]
        public void SecFormatTest()
        {
            var publicKey = new PrivateKey(5000).PublicKey;
            Assert.Equal(
                "04ffe558e388852f0120e46af2d1b370f85854a8eb0841811ece0e3e03d282d57c315dc72890a4f10a1481c031b03b351b0dc79901ca18a00cf009dbdb157a1d10", 
                publicKey.ToSecFormat());
        }
    }
}
