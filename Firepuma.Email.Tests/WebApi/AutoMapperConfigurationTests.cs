using System.Reflection;
using AutoMapper;
using Firepuma.Email.WebApi.Controllers;

namespace Firepuma.Email.Tests.WebApi;

public class AutoMapperConfigurationTests
{
    [Fact]
    public void WhenProfilesAreConfigured_ItShouldNotThrowException()
    {
        // Arrange
        var config = new MapperConfiguration(configuration =>
        {
            //Uncomment this if we ever add mapping of Enums
            // configuration.EnableEnumMappingValidation();

            configuration.AddMaps(typeof(EmailsController).GetTypeInfo().Assembly);
        });

        // Assert
        config.AssertConfigurationIsValid();
    }
}