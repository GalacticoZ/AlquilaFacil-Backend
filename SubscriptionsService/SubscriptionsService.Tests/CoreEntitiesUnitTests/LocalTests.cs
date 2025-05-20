using SubscriptionsService.Locals.Domain.Model.Aggregates;
using SubscriptionsService.Locals.Domain.Model.Commands;

namespace SubscriptionsService.Tests.CoreEntitiesUnitTests;

public class LocalTests
{
    [Fact]
    public void Local_Constructor_WithParameters_ShouldInitializeProperties()
    {
        // Arrange
        var district = "Miraflores";
        var street = "Malecon Cisneros";
        var localName = "Casa Urbana";
        var country = "Perú";
        var city = "Lima";
        var price = 125;
        var photoUrl = "https://a0.muscache.com/im/pictures/pro_photo_tool/Hosting-14127027-unapproved/original/af62b9d0-db54-4d2d-aa89-20cc5a40394f.JPEG?im_w=720";
        var descriptionMessage = "Esta casa urbana combina diseño contemporáneo con comodidades de lujo. Disfruta de espacios amplios, luz natural y una ubicación privilegiada cerca de restaurantes, tiendas y parques.";
        var localCategoryId = 1;
        var userId = 1;
        var features = "Wi-Fi;Baños";
        var capacity = 0;

        // Act
        var local = new Local(district, street, localName, country, city, price, photoUrl, descriptionMessage,
            localCategoryId, userId,features,capacity);
        
        // Assert
        Assert.Equal(localName, local.LocalName);
        Assert.Equal(district, local.Address.District);
        Assert.Equal(street, local.Address.Street);
        Assert.Equal(price, local.Price.PriceNight);
        Assert.Equal(photoUrl, local.PhotoUrl);
        Assert.Equal(country, local.Place.Country);
        Assert.Equal(city, local.Place.City);
        Assert.Equal(descriptionMessage, local.DescriptionMessage);
        Assert.Equal(localCategoryId, local.LocalCategoryId);
        Assert.Equal(userId, local.UserId);
        Assert.Equal(features, local.Features);
        Assert.Equal(capacity, local.Capacity);
    }

    [Fact]
    public void Local_Constructor_WithCommand_ShouldInitializeProperties()
    {
        // Arrange
        var createLocal = new CreateLocalCommand(
            "Miraflores",
            "Malecon Cisneros",
            "Casa Urbana",
            "Perú",
            "Lima",
            125,
            "https://a0.muscache.com/im/pictures/pro_photo_tool/Hosting-14127027-unapproved/original/af62b9d0-db54-4d2d-aa89-20cc5a40394f.JPEG?im_w=720",
            "Esta casa urbana combina diseño contemporáneo con comodidades de lujo. Disfruta de espacios amplios, luz natural y una ubicación privilegiada cerca de restaurantes, tiendas y parques.",
            1,1,"Wi-Fi, Baños",10);
        
        // Act
        var local = new Local(createLocal);

        // Assert
        Assert.Equal(createLocal.LocalType, local.LocalName);
        Assert.Equal(createLocal.District, local.Address.District);
        Assert.Equal(createLocal.Street, local.Address.Street);
        Assert.Equal(createLocal.Price, local.Price.PriceNight);
        Assert.Equal(createLocal.PhotoUrl, local.PhotoUrl);
        Assert.Equal(createLocal.Country, local.Place.Country);
        Assert.Equal(createLocal.City, local.Place.City);
        Assert.Equal(createLocal.LocalCategoryId, local.LocalCategoryId);    
        Assert.Equal(createLocal.Features, local.Features);
        Assert.Equal(createLocal.Capacity, local.Capacity);
    }
}