namespace Fanex.NAP.SPU.CTS.Tests.Features.Transform
{
    using Fanex.NAP.Client;
    using Fanex.NAP.Client.AspNetCore.Configuration;
    using Fanex.NAP.Client.AspNetCore.Session;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Models.ValueObject;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.SPU.CTS.Features.UserPermission;
    using Fanex.NAP.SPU.CTS.Shares;
    using FluentAssertions;
    using NSubstitute;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class UserPermissionUnitTest
    {
        private readonly UserPermissionService permissionService;
        private readonly IUserPermissionDataAccess dataAccess;
        private readonly ISentryClient sentryClient;
        //private readonly UserService userService;
        
        public UserPermissionUnitTest()
        {
            dataAccess = Substitute.For<IUserPermissionDataAccess>();
            sentryClient = Substitute.For<ISentryClient>();
            permissionService = new UserPermissionService(dataAccess, sentryClient);
            //userService = Substitute.For<UserService>();            
        }

        //[Theory]
        //[InlineData(FSFunctionKeys.AssociatedAccountMonitor, 1)]
        //[InlineData(FSFunctionKeys.ProbationManagement, 1)]
        //public void GetUserPolicy_Success_ReturnPolicyList(string functionName, int siteId)
        //{
        //    var userPolicyEntity = new List<UserPolicy>() {
        //        new UserPolicy
        //        {
        //            CreatedDate = DateTime.Now,
        //            Creator = new Client.Models.User {
        //                Id = 1,
        //                Username = "test"
        //            },
        //            Policy = 1,
        //            ModifiedDate = DateTime.Now,
        //            User = new Client.Models.User
        //            {
        //                Id= 1,
        //                Username = "test"
        //            }
        //        },
        //         new UserPolicy
        //        {
        //            CreatedDate = DateTime.Now,
        //            Creator = new Client.Models.User {
        //                Id = 1,
        //                Username = "test"
        //            },
        //            Policy = 2,
        //            ModifiedDate = DateTime.Now,
        //            User = new Client.Models.User
        //            {
        //                Id= 1,
        //                Username = "test"
        //            }
        //        }

        //    };

        //    userService.GetUserPolicies(functionName, siteId).Returns(userPolicyEntity);

        //    var result = permissionService.GetUserPolicies(functionName, siteId);

        //    result.Should().BeEquivalentTo(userPolicyEntity);
        //}

        [Theory]
        [InlineData(FSFunctionKeys.AssociatedAccountMonitorKey)]
        [InlineData(FSFunctionKeys.ProbationManagementKey)]
        public void GetUserPermissionFromNAP_Success_ReturnUserPermissionList(int functionKey)
        {
            var userPolicyEntity = new List<UserPolicy>() {
                    new UserPolicy
                    {
                        CreatedDate = DateTime.Now,
                        Creator = new Client.Models.User {
                            Id = 1,
                            Username = "test"
                        },
                        Policy = 1,
                        ModifiedDate = DateTime.Now,
                        User = new Client.Models.User
                        {
                            Id= 1,
                            Username = "test"
                        }
                    }
                };

            var result = permissionService.GetUserPermissionFromNAP(userPolicyEntity, functionKey);

            result.Should().HaveCount(x => x > 0);
            result.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(FSFunctionKeys.AssociatedAccountMonitorKey)]
        [InlineData(0)]
        public void GetUserPermissionFromNAP_Success_ReturnEmpty(int functionKey)
        {           
            var result = permissionService.GetUserPermissionFromNAP(new List<UserPolicy>(), functionKey);
                        
            result.Should().BeEmpty();
        }
    }
}