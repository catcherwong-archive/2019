namespace BizLayer.Tests.UserInfo
{
    using BizLayer.Notify;
    using BizLayer.UserInfo;
    using BizLayer.UserInfo.Dtos;
    using CoreLayer.Repositories;
    using FakeItEasy;
    using Shouldly;
    using System.Threading.Tasks;
    using Xunit;

    public class UserInfoBizTests
    {
        private IUserInfoRepository _repo;
        private INotifyBiz _notifyBiz;
        private UserInfoBiz _biz;

        public UserInfoBizTests()
        {
            _repo = A.Fake<IUserInfoRepository>();
            _notifyBiz = A.Fake<INotifyBiz>();
            var loggerFactory = Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance;

            _biz = new UserInfoBiz(loggerFactory, _repo, _notifyBiz);
        }

        [Fact]
        public async Task CreateUser_Should_Return_1001_When_Password_Is_Weak()
        {
            var dto = new CreateUserDto { UserName = "catcher" , Password = "23", Email = "aa@example.com" };

            var (code, msg) = await _biz.CreateUserAsync(dto);

            code.ShouldBe(1001);
            msg.ShouldBe("password is too weak");
        }

        [Fact]
        public async Task CreateUser_Should_Return_9000_When_Add_Failed()
        {
            FakeCreateUserAsyncReturnFalse();

            var dto = new CreateUserDto { UserName = "catcher", Password = "123456", Email = "aa@example.com" };

            var (code, msg) = await _biz.CreateUserAsync(dto);

            code.ShouldBe(9000);
            msg.ShouldBe("error");
        }

        [Fact]
        public async Task CreateUser_Should_Return_0_When_Add_Succeed()
        {
            FakeCreateUserAsyncReturnTrue();

            var dto = new CreateUserDto { UserName = "catcher", Password = "123456", Email = "aa@example.com" };

            var (code, msg) = await _biz.CreateUserAsync(dto);

            code.ShouldBe(0);
            msg.ShouldBe("ok");
        }

        [Fact]
        public async Task CreateUser_Should_Trigger_SendEmail_When_Add_Succeed()
        {
            FakeCreateUserAsyncReturnTrue();

            var dto = new CreateUserDto { UserName = "catcher", Password = "123456", Email = "aa@example.com" };

            var (code, msg) = await _biz.CreateUserAsync(dto);

            A.CallTo(() => _notifyBiz.SendEmail(dto.Email)).MustHaveHappened();
        }

        [Fact]
        public async Task CreateUser_Should_Not_Trigger_SendEmail_When_Add_Failed()
        {
            FakeCreateUserAsyncReturnFalse();

            var dto = new CreateUserDto { UserName = "catcher", Password = "123456", Email = "aa@example.com" };

            var (code, msg) = await _biz.CreateUserAsync(dto);

            A.CallTo(() => _notifyBiz.SendEmail(dto.Email)).MustNotHaveHappened();
        }

        [Fact]
        public async Task DeleteUser_Should_Return_2001_When_User_Is_Not_Exist()
        {
            A.CallTo(() => _repo.GetUserInfoByUserNameAsync(A<string>._)).Returns(Task.FromResult<CoreLayer.Domains.UserInfo>(null));

            var dto = new DeleteUserDto { UserName = "catcher"};

            var (code, msg) = await _biz.DeleteUserAsync(dto);

            code.ShouldBe(2001);
            msg.ShouldBe("can not find user");
        }

        [Fact]
        public async Task DeleteUser_Should_Return_2002_When_User_Is_Deleted()
        {
            A.CallTo(() => _repo.GetUserInfoByUserNameAsync(A<string>._)).Returns(Task.FromResult(new CoreLayer.Domains.UserInfo { IsDel = true }));

            var dto = new DeleteUserDto { UserName = "catcher" };

            var (code, msg) = await _biz.DeleteUserAsync(dto);

            code.ShouldBe(2002);
            msg.ShouldBe("user is already been deleted");
        }

        [Fact]
        public async Task DeleteUser_Should_Return_0_When_Delete_Succeed()
        {
            A.CallTo(() => _repo.GetUserInfoByUserNameAsync(A<string>._)).Returns(Task.FromResult(new CoreLayer.Domains.UserInfo { IsDel = false }));
            A.CallTo(() => _repo.DeleteUserAsync(A<CoreLayer.Domains.UserInfo>._)).Returns(Task.FromResult(true));

            var dto = new DeleteUserDto { UserName = "catcher" };

            var (code, msg) = await _biz.DeleteUserAsync(dto);

            code.ShouldBe(0);
            msg.ShouldBe("ok");
        }


        [Fact]
        public async Task DeleteUser_Should_Return_9000_When_Delete_Failed()
        {
            A.CallTo(() => _repo.GetUserInfoByUserNameAsync(A<string>._)).Returns(Task.FromResult(new CoreLayer.Domains.UserInfo { IsDel = false }));
            A.CallTo(() => _repo.DeleteUserAsync(A<CoreLayer.Domains.UserInfo>._)).Returns(Task.FromResult(false));

            var dto = new DeleteUserDto { UserName = "catcher" };

            var (code, msg) = await _biz.DeleteUserAsync(dto);

            code.ShouldBe(9000);
            msg.ShouldBe("error");
        }

        [Fact]
        public async Task ModifyPassword_Should_Return_2001_When_User_Is_Not_Exist()
        {
            A.CallTo(() => _repo.GetUserInfoByUserNameAsync(A<string>._)).Returns(Task.FromResult<CoreLayer.Domains.UserInfo>(null));

            var dto = new ModifyPasswordDto { UserName = "catcher", Password = "12" };

            var (code, msg) = await _biz.ModifyPasswordAsync(dto);

            code.ShouldBe(2001);
            msg.ShouldBe("can not find user");
        }

        [Fact]
        public async Task ModifyPassword_Should_Return_1001_When_Password_Is_Weak()
        {
            A.CallTo(() => _repo.GetUserInfoByUserNameAsync(A<string>._)).Returns(Task.FromResult(new CoreLayer.Domains.UserInfo { IsDel = false }));
          

            var dto = new ModifyPasswordDto { UserName = "catcher", Password = "12" };

            var (code, msg) = await _biz.ModifyPasswordAsync(dto);

            code.ShouldBe(1001);
            msg.ShouldBe("password is too weak");
        }


        [Fact]
        public async Task ModifyPassword_Should_Return_0_When_Modify_Succeed()
        {
            A.CallTo(() => _repo.GetUserInfoByUserNameAsync(A<string>._)).Returns(Task.FromResult(new CoreLayer.Domains.UserInfo { IsDel = false }));
            A.CallTo(() => _repo.ModifyUserAsync(A<CoreLayer.Domains.UserInfo>._)).Returns(Task.FromResult(true));

            var dto = new ModifyPasswordDto { UserName = "catcher", Password = "123456" };

            var (code, msg) = await _biz.ModifyPasswordAsync(dto);

            code.ShouldBe(0);
            msg.ShouldBe("ok");
        }


        [Fact]
        public async Task ModifyPassword_Should_Return_9000_When_Modify_Failed()
        {
            A.CallTo(() => _repo.GetUserInfoByUserNameAsync(A<string>._)).Returns(Task.FromResult(new CoreLayer.Domains.UserInfo { IsDel = false }));
            A.CallTo(() => _repo.ModifyUserAsync(A<CoreLayer.Domains.UserInfo>._)).Returns(Task.FromResult(false));

            var dto = new ModifyPasswordDto { UserName = "catcher", Password = "123456" };

            var (code, msg) = await _biz.ModifyPasswordAsync(dto);

            code.ShouldBe(9000);
            msg.ShouldBe("error");
        }
 
        private void FakeCreateUserAsyncReturnFalse()
        {
            A.CallTo(() => _repo.CreateUserAsync(A<CoreLayer.Domains.UserInfo>._)).Returns(Task.FromResult(false));
        }

        private void FakeCreateUserAsyncReturnTrue()
        {
            A.CallTo(() => _repo.CreateUserAsync(A<CoreLayer.Domains.UserInfo>._)).Returns(Task.FromResult(true));
        }

    }
}
