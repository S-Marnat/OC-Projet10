using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Patients.Services.Interfaces;
using Patients.Services.Implementations;
using Patients.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Patients.Tests.UnitTests.Services
{
    public class RefreshTokenServiceTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenServiceTests()
        {
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _jwtServiceMock = new Mock<IJwtService>();

            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            _refreshTokenService = new RefreshTokenService(
                 _refreshTokenRepositoryMock.Object,
                _jwtServiceMock.Object,
                _userManagerMock.Object);
        }


        // -- GenerateTokensAsync --

        [Fact]
        public async Task GenerateTokensAsync_UtilisateurValide_RetourneJwtEtRefreshToken()
        {
            // Arrange
            var user = new IdentityUser { Id = "user123", Email = "test@test.com" };

            _jwtServiceMock.Setup(j => j.GenerateTokenAsync(user))
                .ReturnsAsync("fake-jwt-token");

            _refreshTokenRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<RefreshToken>()))
                .ReturnsAsync(new RefreshToken
                {
                    Token = "abc",
                    Expiration = DateTime.UtcNow.AddDays(1),
                    UserId = "user123"
                });

            // Act
            var result = await _refreshTokenService.GenerateTokensAsync(user);

            // Assert
            _jwtServiceMock.Verify(j => j.GenerateTokenAsync(user), Times.Once);
            _refreshTokenRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<RefreshToken>()), Times.Once);

            Assert.Equal("fake-jwt-token", result.jwt);
            Assert.False(string.IsNullOrEmpty(result.refreshToken));
        }


        // -- RefreshAsync --

        [Fact]
        public async Task RefreshAsync_TokenInexistant_RetourneNull()
        {
            // Arrange
            _refreshTokenRepositoryMock.Setup(r => r.GetAsync("abc"))
                .ReturnsAsync((RefreshToken?)null);

            // Act
            var result = await _refreshTokenService.RefreshAsync("abc");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RefreshAsync_TokenExpire_RetourneNull()
        {
            // Arrange
            var token = new RefreshToken
            {
                Token = "abc",
                Expiration = DateTime.UtcNow.AddDays(-1), // expiré
                UserId = "user123"
            };

            _refreshTokenRepositoryMock.Setup(r => r.GetAsync("abc"))
                .ReturnsAsync(token);

            // Act
            var result = await _refreshTokenService.RefreshAsync("abc");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RefreshAsync_TokenValide_RetourneNouveauJwtEtRefreshToken()
        {
            // Arrange
            var token = new RefreshToken
            {
                Token = "abc",
                Expiration = DateTime.UtcNow.AddDays(1),
                UserId = "user123"
            };

            var user = new IdentityUser { Id = "user123", Email = "test@test.com" };

            _refreshTokenRepositoryMock.Setup(r => r.GetAsync("abc"))
                .ReturnsAsync(token);

            _userManagerMock.Setup(u => u.FindByIdAsync("user123"))
                .ReturnsAsync(user);

            _refreshTokenRepositoryMock.Setup(r => r.RevokeAsync(token))
                .Returns(Task.CompletedTask);

            _jwtServiceMock.Setup(j => j.GenerateTokenAsync(user))
                .ReturnsAsync("new-jwt");

            _refreshTokenRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<RefreshToken>()))
                .ReturnsAsync(new RefreshToken
                {
                    Token = "new-refresh-token",
                    Expiration = DateTime.UtcNow.AddDays(1),
                    UserId = "user123"
                });

            // Act
            var result = await _refreshTokenService.RefreshAsync("abc");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new-jwt", result.Value.jwt);
            Assert.False(string.IsNullOrEmpty(result.Value.refreshToken));

            _refreshTokenRepositoryMock.Verify(r => r.RevokeAsync(token), Times.Once);
            _refreshTokenRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<RefreshToken>()), Times.Once);
        }


        // -- RevokeAsync --

        [Fact]
        public async Task RevokeAsync_TokenInexistant_RetourneFalse()
        {
            // Arrange
            _refreshTokenRepositoryMock.Setup(r => r.GetAsync("abc"))
                .ReturnsAsync((RefreshToken?)null);

            // Act
            var result = await _refreshTokenService.RevokeAsync("abc");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task RevokeAsync_TokenValide_RetourneTrue()
        {
            // Arrange
            var token = new RefreshToken { Token = "abc" };

            _refreshTokenRepositoryMock.Setup(r => r.GetAsync("abc"))
                .ReturnsAsync(token);

            _refreshTokenRepositoryMock.Setup(r => r.RevokeAsync(token))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _refreshTokenService.RevokeAsync("abc");

            // Assert
            Assert.True(result);
            _refreshTokenRepositoryMock.Verify(r => r.RevokeAsync(token), Times.Once);
        }
    }
}
