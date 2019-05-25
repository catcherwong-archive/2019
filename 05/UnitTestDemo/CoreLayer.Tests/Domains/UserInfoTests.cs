namespace CoreLayer.Tests.Domains
{
    using CoreLayer.Domains;
    using Shouldly;
    using Xunit;

    public class UserInfoTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void CheckIsStrongPassword_Should_Return_False_When_Password_Is_Empty(string pwd)
        {
            var user = new UserInfo { Password = pwd };

            var flag = user.CheckIsStrongPassword();

            flag.ShouldBe(false);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("123")]
        [InlineData("12")]
        [InlineData("1")]
        public void CheckIsStrongPassword_Should_Return_False_When_Password_Length_LessThan_Five(string pwd)
        {
            var user = new UserInfo { Password = pwd };

            var flag = user.CheckIsStrongPassword();

            flag.ShouldBe(false);
        }

        [Theory]
        [InlineData("12345")]
        [InlineData("123456")]
        public void CheckIsStrongPassword_Should_Return_True_When_Password_Length_GreatOrEqual_Five(string pwd)
        {
            var user = new UserInfo { Password = pwd };

            var flag = user.CheckIsStrongPassword();

            flag.ShouldBe(true);
        }

        [Fact]
        public void CheckUserStatus_Should_Succeed()
        {
            var user = new UserInfo { IsDel = false };

            var status = user.CheckUserStatus();

            status.ShouldBe(false);
        }

        [Fact]
        public void ModifyPassword_Should_Succeed()
        {
            var user = new UserInfo { Password = "123456" };

            user.ModifyPassword("abcdef");

            user.Password.ShouldBe("abcdef");
        }

        [Fact]
        public void DeleteUser_Should_Succeed()
        {
            var user = new UserInfo { IsDel = false };

            user.DeleteUser();

            user.IsDel.ShouldBe(true);
        }       
    }
}
