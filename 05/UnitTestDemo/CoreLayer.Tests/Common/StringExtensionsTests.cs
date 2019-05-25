namespace CoreLayer.Tests.Common
{
    using CoreLayer.Common;
    using Shouldly;
    using System;
    using Xunit;

    public class StringExtensionsTests
    {
        [Theory]        
        [InlineData(null)]        
        public void GetLength_With_Null_Input_Should_Throw_ArgumentNullException(string input)
        {            
            Should.Throw<ArgumentNullException>(() => 
            {
                var len = input.GetLength();
            });
        }

        [Fact]
        public void GetLength_With_Not_Null_Input_Should_Succeed()
        {
            var input = "catcher";

            var len = input.GetLength();

            len.ShouldBe<int>(7);
        }
    }
}
