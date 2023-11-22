
//using Xunit;
//using ClimateCamp.PowerBI;
//using Microsoft.PowerBI.Api;
//using System;

//namespace ClimateCamp.Common.Tests.PowerBIService_Tests
//{
//    public class PowerBIService_Tests : CommonTestBase
//    {
//        private readonly IPowerBIService _powerBIService;

//        private readonly IPowerBIAuthentication _powerBIAuthentication;
//        public PowerBIService_Tests()
//        {
//            _powerBIAuthentication = Resolve<IPowerBIAuthentication>();
//            //_powerBIService = Resolve<IPowerBIService>();
//        }

//        [Fact]
//        public  PowerBIClient   GetPowerBIClient_Test()
//        {
//            PowerBiService powerBIService = new PowerBiService(_powerBIAuthentication);
//            // Act
//            var output = powerBIService.GetPowerBIClient();
//            return output;
//            // Assert
//        }

//        [Fact]
//        public void GetPowerBIAuthToken_Test()
//        {
//            Guid reportId = Guid.Parse("87808bbc-3611-4e5e-bd9f-3eea20a81cb7");
//            Guid workspaceId = Guid.Parse("ca7e2e70-bdaa-4d6f-a1ae-6ed114aa7551");
//            PowerBiService powerBIService = new PowerBiService(_powerBIAuthentication);
//            // Act
//            var output = powerBIService.GetPowerBIAuthToken();
//            var embedparams = powerBIService.GetEmbedParams(workspaceId, reportId);
//           // Assert (output);
//            // Assert
//        }

//    }
//}
