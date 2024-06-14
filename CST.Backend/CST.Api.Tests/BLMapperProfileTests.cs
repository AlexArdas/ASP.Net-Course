using AutoMapper;
using CST.BusinessLogic.Configuration.BLMapperProfiles;
using Xunit;

namespace CST.Api.Tests
{
    public class BLMapperProfileTests
    {
        [Fact]
        public void BLMapperProfiles_ShouldMapTheirObjects()
        {
            //Arrange
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<KeyNumberSetProfile>();
                cfg.AddProfile<LocationProfile>();
                cfg.AddProfile<MailingProfile>();
                cfg.AddProfile<NotificationChannelProfile>();
                cfg.AddProfile<ReportColumnSetProfile>();
                cfg.AddProfile<ReportProfile>();
                cfg.AddProfile<RequestFormProfile>();
                cfg.AddProfile<RequestMessageProfile>();
                cfg.AddProfile<RequestProfile>();
                cfg.AddProfile<UserProfile>();
            });

            //Act & Assert
            config.AssertConfigurationIsValid();
        }
    }
}
