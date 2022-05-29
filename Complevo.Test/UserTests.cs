using Application.Services;
using Core.Dtos.User;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System;
using Xunit;

namespace Complevo.Test
{
    public class UserTests
    {
        [Theory]
        [InlineData("", "parishah")]
        [InlineData("ali", "parishah")]
        public async System.Threading.Tasks.Task RegisterUser_InvalidUsername_ShouldThrowExceptionAsync(string username, string password)
        {
            var accountmanager = new AccountManager();
            var model = new RegisterUserModel() { Username = username, Password = password };
            await accountmanager.RegisterUser(new RegisterUserModel() { Username = "ali", Password = "as432" });
            var exception = Record.ExceptionAsync(() => accountmanager.RegisterUser(model));
            Assert.NotNull(exception);
        }
        [Theory]
        [InlineData("pari", "parishah")]
        [InlineData("ali", "parishah")]
        [InlineData("parisa", "parishah")]
        public void RegisterUser_ShouldNotThrowException(string username, string password)
        {
            var accountmanager = new AccountManager();
            var model = new RegisterUserModel() { Username = username, Password = password };
            var exception = Record.ExceptionAsync(() => accountmanager.RegisterUser(model));
            Assert.Null(exception);
        }
        [Theory]
        [InlineData("pari", "GJvnhsfF")]
        [InlineData("ali", "as6gy345")]
        [InlineData("parisa", "skmad09")]
        public async System.Threading.Tasks.Task IsValidUser_ShouldValidateAsync(string username, string password)
        {
            var accountManager = new AccountManager();
            await accountManager.RegisterUser(new RegisterUserModel() { Password = password, Username = username });
            Assert.True(accountManager.IsValidUser(username, password).Result);
        }
        [Theory]
        [InlineData("pari", "GJvnhsfF")]
        [InlineData("ali", "as6gy345")]
        [InlineData("parisa", "skmad09")]
        public void IsValidUser_ShouldNotValidate(string username, string password)
        {
            var mockRepo = new Mock<IAccountManager>();
            mockRepo.Setup(x => x.GetUser(username)).Returns(new User() { Username = username, Password = CryptoHelper.GetMD5Hash(password) });
            Random random = new();
            Assert.False(await mockRepo.Object.IsValidUser(username, random.Next(1000).ToString()));
        }
    }
}
