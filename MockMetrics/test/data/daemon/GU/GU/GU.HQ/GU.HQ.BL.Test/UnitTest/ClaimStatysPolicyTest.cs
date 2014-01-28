using System;
using Common.BL.Exceptions;
using GU.DataModel;
using GU.HQ.BL.Policy;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;
using NUnit.Framework;

namespace GU.HQ.BL.Test.UnitTest
{
    /// <summary>
    /// тестирование бизнес логики по смене статусов у заявления
    /// </summary>
    [TestFixture]
    public class ClaimStatysPolicyTest
    {
        private Claim _claim;
        private readonly IClaimStatusPolicy _claimStatusPolicy = new ClaimStatusPolicy();

        [SetUp]
        public void Setup()
        {
            _claim = Claim.CreateInstance();
        }

        /// <summary>
        /// проверка возможности мены статуса
        /// </summary>
        [TestCase(ClaimStatusType.DataCheck, ClaimStatusType.QueueReg, Result = true)]
        [TestCase(ClaimStatusType.DataCheck, ClaimStatusType.Rejected, Result = true)]
        [TestCase(ClaimStatusType.QueueReg, ClaimStatusType.QueuePrivReg, Result = true)]
        [TestCase(ClaimStatusType.QueueReg, ClaimStatusType.HouseProvided, Result = true)]
        [TestCase(ClaimStatusType.QueueReg, ClaimStatusType.Rejected, Result = true)]
        [TestCase(ClaimStatusType.QueuePrivReg, ClaimStatusType.QueuePrivDeReg, Result = true)]
        [TestCase(ClaimStatusType.QueuePrivDeReg, ClaimStatusType.QueueReg, Result = true)]
        [TestCase(ClaimStatusType.QueuePrivDeReg, ClaimStatusType.HouseProvided, Result = true)]
        [TestCase(ClaimStatusType.HouseProvided, ClaimStatusType.Rejected, Result = true)]
        [TestCase(ClaimStatusType.DataCheck, ClaimStatusType.HouseProvided, Result = false)]
        public bool CanSetStatusTest(ClaimStatusType st1, ClaimStatusType st2)
        {
            _claim.CurrentStatusTypeId = st1;

            return _claimStatusPolicy.CanSetStatus(_claim, st2);
        }

        /// <summary>
        /// Проверяем присвоение нового статуса заявке
        /// </summary>
        [TestCase(ClaimStatusType.DataCheck, ClaimStatusType.QueueReg, Result = true)]
        [TestCase(ClaimStatusType.DataCheck, ClaimStatusType.Rejected, Result = true)]
        [TestCase(ClaimStatusType.QueueReg, ClaimStatusType.QueuePrivReg, Result = true)]
        [TestCase(ClaimStatusType.QueueReg, ClaimStatusType.HouseProvided, Result = true)]
        [TestCase(ClaimStatusType.QueueReg, ClaimStatusType.Rejected, Result = true)]
        [TestCase(ClaimStatusType.QueuePrivReg, ClaimStatusType.QueuePrivDeReg, Result = true)]
        [TestCase(ClaimStatusType.QueuePrivDeReg, ClaimStatusType.QueueReg, Result = true)]
        [TestCase(ClaimStatusType.QueuePrivDeReg, ClaimStatusType.HouseProvided, Result = true)]
        [TestCase(ClaimStatusType.HouseProvided, ClaimStatusType.Rejected, Result = true)]
        //[TestCase(ClaimStatusType.DataCheck, ClaimStatusType.HouseProvided, ExpectedException = typeof(DomainBLLException))]
        public bool SetStatusTest(ClaimStatusType st1, ClaimStatusType st2)
        {
            var uUser = DbUser.CreateInstance();
            uUser.Id = 1;

            _claim.CurrentStatusTypeId = st1;

            _claimStatusPolicy.SetStatus(_claim, st2, uUser, "");

            return _claim.CurrentStatusTypeId == st2;
        }
    }
}
