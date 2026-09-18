using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Patients.Controllers;
using Patients.DTOs;
using Patients.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Patients.Tests.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
        private readonly AccountController _accountController;

        public AccountControllerTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                store.Object, null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object,
                Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<Microsoft.AspNetCore.Identity.IUserClaimsPrincipalFactory<IdentityUser>>(),
                null, null, null, null);

            _refreshTokenServiceMock = new Mock<IRefreshTokenService>();

            _accountController = new AccountController(
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _refreshTokenServiceMock.Object);
        }


        // -- Login --

        [Fact]
        public async Task Login_IdentifiantsValides_RetourneTokenEtRefreshToken()
        {
            // Arrange
            var dto = new LoginDto { Email = "test@test.com", Password = "123456" };
            var user = new IdentityUser { Id = "user123", Email = dto.Email };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _refreshTokenServiceMock.Setup(r => r.GenerateTokensAsync(user))
                .ReturnsAsync(("jwt-token", "refresh-token"));

            // Act
            var result = await _accountController.Login(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);

            var data = Assert.IsType<TokenResponseDto>(ok.Value);
            Assert.NotNull(data);
            Assert.Equal("jwt-token", data.Token);
            Assert.Equal("refresh-token", data.RefreshToken);
        }

        [Fact]
        public async Task Login_ModelStateInvalide_RetourneBadRequest()
        {
            // Arrange
            _accountController.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _accountController.Login(new LoginDto());

            // Assert
            _userManagerMock.Verify(u => u.FindByEmailAsync(It.IsAny<string>()), Times.Never);
            _signInManagerMock.Verify(s => s.CheckPasswordSignInAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), false), Times.Never);
            _refreshTokenServiceMock.Verify(r => r.GenerateTokensAsync(It.IsAny<IdentityUser>()), Times.Never);
            
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Les informations fournies pour l'utilisateur sont invalides.", badRequest.Value);
        }

        [Fact]
        public async Task Login_EmailInexistant_RetourneUnauthorized()
        {
            // Arrange
            var dto = new LoginDto { Email = "unknown@test.com", Password = "123456" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await _accountController.Login(dto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Identifiants invalides.", unauthorized.Value);
        }

        [Fact]
        public async Task Login_MotDePasseIncorrect_RetourneUnauthorized()
        {
            // Arrange
            var dto = new LoginDto { Email = "test@test.com", Password = "wrong" };
            var user = new IdentityUser { Id = "user123", Email = dto.Email };

            _userManagerMock.Setup(u => u.FindByEmailAsync(dto.Email))
                .ReturnsAsync(user);

            _signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(user, dto.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _accountController.Login(dto);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Identifiants invalides.", unauthorized.Value);
        }

        [Fact]
        public async Task Login_ServiceLanceException_RetourneInternalServerError()
        {
            // Arrange
            var dto = new LoginDto { Email = "test@test.com", Password = "PasswordTest" };

            _userManagerMock.Setup(u => u.FindByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Erreur de test"));

            // Act
            var result = await _accountController.Login(dto);

            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("Une erreur interne est survenue.", internalServerError.Value);
        }


            // -- Refresh Token --

        [Fact]
        public async Task Refresh_TokenValide_RetourneNouveauJwtEtRefreshToken()
        {
            // Arrange
            var dto = new RefreshTokenDto { RefreshToken = "abc" };

            _refreshTokenServiceMock.Setup(r => r.RefreshAsync("abc"))
                .ReturnsAsync(("new-jwt", "new-refresh"));

            // Act
            var result = await _accountController.Refresh(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(ok.Value);

            var data = Assert.IsType<TokenResponseDto>(ok.Value);
            Assert.NotNull(data);
            Assert.Equal("new-jwt", data.Token);
            Assert.Equal("new-refresh", data.RefreshToken);
        }

        [Fact]
        public async Task Refresh_TokenInvalide_RetourneUnauthorized()
        {
            // Arrange
            var dto = new RefreshTokenDto { RefreshToken = "invalid" };

            _refreshTokenServiceMock.Setup(r => r.RefreshAsync("invalid"))
                .ReturnsAsync((ValueTuple<string, string>?)null);

            // Act
            var result = await _accountController.Refresh(dto);

            // Assert
            _refreshTokenServiceMock.Verify(r => r.RefreshAsync(It.IsAny<string>()), Times.Once);
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Refresh_ServiceLanceException_RetourneInternalServerError()
        {
            // Arrange
            var dto = new RefreshTokenDto { RefreshToken = "abc" };

            _refreshTokenServiceMock.Setup(r => r.RefreshAsync("abc"))
                .ThrowsAsync(new Exception("Erreur de test"));

            // Act
            var result = await _accountController.Refresh(dto);

            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("Une erreur interne est survenue.", internalServerError.Value);
        }


        // -- Logout --

        [Fact]
        public async Task Logout_RetourneOk()
        {
            // Arrange
            var dto = new RefreshTokenDto { RefreshToken = "abc" };

            _refreshTokenServiceMock.Setup(r => r.RevokeAsync("abc"))
                .ReturnsAsync(true);

            // Act
            var result = await _accountController.Logout(dto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Logout_ServiceLanceException_RetourneInternalServerError()
        {
            // Arrange
            var dto = new RefreshTokenDto { RefreshToken = "abc" };

            _refreshTokenServiceMock.Setup(r => r.RevokeAsync("abc"))
                .ThrowsAsync(new Exception("Erreur de test"));

            // Act
            var result = await _accountController.Logout(dto);

            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("Une erreur interne est survenue.", internalServerError.Value);
        }
    }
}
