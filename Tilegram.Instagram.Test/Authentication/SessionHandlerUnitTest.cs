using Newtonsoft.Json;
using Tilegram.Feature.Authentication;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Tilegram.Feature.Test.Authentication;

public class SessionHandlerTests
{
    [Fact]
    public async Task GetSessionData_ReturnsSession_WhenTokenAndFileExist()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(tempDir, "InstaSessions"));

        var userId = "12345";
        var fakeJson = "{ \"session\": \"data\" }";
        var filePath = Path.Combine(tempDir, "InstaSessions", $"{userId}.json");
        await File.WriteAllTextAsync(filePath, fakeJson);

        var mockEnv = new Mock<IWebHostEnvironment>();
        mockEnv.Setup(e => e.ContentRootPath).Returns(tempDir);

        var mockJwt = new Mock<IJwtService>();
        mockJwt.Setup(j => j.ReadPayload(It.IsAny<string>()))
               .Returns(new Dictionary<string, object>
               {
                   { "sub", userId },
                   { "exp", DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds() }
               });

        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer fake_token";
        var mockHttp = new Mock<IHttpContextAccessor>();
        mockHttp.Setup(h => h.HttpContext).Returns(context);

        var handler = new SessionHandler(mockEnv.Object, mockJwt.Object, mockHttp.Object);

        // Act
        var result = await handler.GetSessionData();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeJson, result.Json);
        Assert.Equal("fake_token", result.AccessToken);
        Assert.True(result.Expires > 0);
    }

    [Fact]
    public async Task SetSessionData_WritesFileAndReturnsSession()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Path.Combine(tempDir, "InstaSessions"));

        var mockEnv = new Mock<IWebHostEnvironment>();
        mockEnv.Setup(e => e.ContentRootPath).Returns(tempDir);

        var mockJwt = new Mock<IJwtService>();
        mockJwt.Setup(j => j.GenerateToken(It.IsAny<Dictionary<string, object>>(), null))
               .Returns("fake_jwt_token");

        var mockHttp = new Mock<IHttpContextAccessor>();

        var handler = new SessionHandler(mockEnv.Object, mockJwt.Object, mockHttp.Object);

        var instaApiSessionData = new InstaApiSessionData
        {
            UserSession = new InstaApiSessionData.UserSessionData
            {
                UserName = "testuser",
                LoggedInUser = new InstaApiSessionData.LoggedInUser
                {
                    Pk = 12345,
                    FullName = "Test User",
                    ProfilePicUrl = "http://example.com/pic.jpg"
                }
            }
        };
        var json = JsonConvert.SerializeObject(instaApiSessionData);

        // Act
        var result = await handler.SetSessionData(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(json, result.Json);
        Assert.Equal("fake_jwt_token", result.AccessToken);
        Assert.True(result.Expires > 0);

        var filePath = Path.Combine(tempDir, "InstaSessions", $"{instaApiSessionData.UserSession.LoggedInUser.Pk}.json");
        var fileExists = File.Exists(filePath);
        Assert.True(fileExists);
    }
}
