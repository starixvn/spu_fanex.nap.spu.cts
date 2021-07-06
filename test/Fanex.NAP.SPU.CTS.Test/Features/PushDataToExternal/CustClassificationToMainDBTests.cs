namespace Fanex.NAP.SPU.CTS.Tests.Features.PushDataToExternal
{
    using Fanex.NAP.SPU.CTS.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.Shares;
    using FluentAssertions;
    using Newtonsoft.Json;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class CustClassificationToMainDBTests
    {
        private readonly ICustClassToMainDBDataAccess custClassToMainDBDataAccess;
        private readonly ICustClassToMainDBService custClassToMainDBService;

        public CustClassificationToMainDBTests()
        {
            custClassToMainDBDataAccess = Substitute.For<ICustClassToMainDBDataAccess>();

            custClassToMainDBService = new CustClassToMainDBService(custClassToMainDBDataAccess, null);
        }

        [Fact]
        public void GetProblemAccountsFromCN88_WhenSuccess_ReturnProblemAccountList()
        {
            var problemAccounts = new List<LatestClassificationEntity> {
                new LatestClassificationEntity
                {
                   CustId = 1,
                    Category = 1,
                    CustomerClass = 1,
                     ScannedTime = DateTime.Parse("2021-07-05 22:50:55")
                },
                 new LatestClassificationEntity {
                   CustId = 2,
                    Category = 2,
                    CustomerClass = 2,
                     ScannedTime = DateTime.Parse("2021-07-05 22:50:55")
                }
            };

            custClassToMainDBDataAccess.GetLatestClassification();

            var result = problemAccountServices.GetProblemAccounts_CN88(Constants.ProblemAccountByDateTime, null);

            result.Should().BeEquivalentTo(problemAccounts);
        }

    }
}
