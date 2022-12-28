using System.Reflection;
using AutoMapper;
using Firepuma.Email.Worker.Controllers;

namespace Firepuma.Email.Tests.Worker;

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

            configuration.AddMaps(typeof(PubSubListenerController).GetTypeInfo().Assembly);
        });

        // Assert
        config.AssertConfigurationIsValid();
    }
}