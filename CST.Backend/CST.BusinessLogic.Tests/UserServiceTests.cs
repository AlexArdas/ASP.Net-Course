using System;
using System.Threading.Tasks;
using AutoFixture;
using CST.BusinessLogic.Configuration.BLMapperProfiles;
using CST.BusinessLogic.Services;
using CST.Common.Models.Domain;
using CST.Common.Models.DTO;
using CST.Common.Models.Pagination;
using CST.Common.Repositories;
using CST.Tests.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace CST.BusinessLogic.Tests
{    
    public class UserServiceTests : AutoMockerTestsBase<UserService>
    {
        private readonly Fixture _fixture;
        private readonly Mock<IUserRepository> _userRepository;

        public UserServiceTests()
        {
            _fixture = FixtureInitializer.InitializeFixture();
            _userRepository = GetMock<IUserRepository>();
            UseMapperWithProfiles(new UserProfile());
        }

        [Fact]
        public async Task GetUserInfoByEmailAsync_ShouldReturnUserResponseModel()
        {
            //Arrange
            var userResponse = _fixture.Create<UserResponse>();
            _userRepository.Setup(ur => ur.GetUserInfoByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(userResponse);

            //Act
            var result = await Target.GetUserInfoByEmailAsync(It.IsAny<string>());

            //Assert
            result.Should().BeOfType(userResponse.GetType());
            _userRepository.Verify(ur => ur.GetUserInfoByEmailAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task FilterUsersByFullNameAsync_ShouldReturnPaginatedListOfUserBriefResponses()
        {
            //Arrange
            var users = _fixture.Create<PaginatedList<UserBriefResponse>>();
            _userRepository.Setup(rr => rr.FilterUsersByFullNameAsync(It.IsAny<string>(), It.IsAny<PaginationParameters>()))
                .ReturnsAsync(users);

            //Act
            var result = await Target.FilterUsersByFullNameAsync(It.IsAny<string>(), It.IsAny<PaginationParameters>());

            //Assert
            result.Should().BeOfType(users.GetType());
            _userRepository.Verify(rr => rr.FilterUsersByFullNameAsync(It.IsAny<string>(), It.IsAny<PaginationParameters>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUserDomainEntity()
        {
            //Arrange
            var userEntity = _fixture.Create<UserDomainEntity>();

            _userRepository.Setup(ur => ur.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(userEntity);

            //Act
            var result = await Target.GetUserByEmailAsync(It.IsAny<string>());

            //Assert
            result.Should().BeOfType(userEntity.GetType());
            _userRepository.Verify(ur => ur.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetUserInfoByIdAsync_ShouldReturnUserResponseModel()
        {
            //Arrange
            var userResponse = _fixture.Create<UserResponse>();
            _userRepository.Setup(ur => ur.GetUserInfoByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(userResponse);

            //Act
            var result = await Target.GetUserInfoByIdAsync(It.IsAny<Guid>());

            //Assert
            result.Should().BeOfType(userResponse.GetType());
            _userRepository.Verify(ur => ur.GetUserInfoByIdAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
