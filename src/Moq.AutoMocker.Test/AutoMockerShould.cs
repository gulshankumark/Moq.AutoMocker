namespace Moq.AutoMocker.Test
{
    public class AutoMockerShould
    {
        [Fact]
        public void SuccessfullyCreateInstanceByMockingDependencies()
        {
            var expectedResult = 100;
            var testClass = AutoMock.AutoMocker.GetInstance<TestClass>();
            AutoMock.AutoMocker.GetMock<IA>().Setup(x => x.RetData(It.IsAny<int>())).Returns(expectedResult);

            Assert.Equal(expectedResult, testClass.TestMethod(It.IsAny<int>()));
        }
    }

    public interface IA
    {
        int RetData(int x);
    }

    public interface IB
    {

    }

    public interface IC
    {

    }
    public class TestClass
    {
        private readonly IA _a;
        private readonly IB _b;
        private readonly IC _c;

        public TestClass(IA a, IB b, IC c)
        {
            _a = a;
            _b = b;
            _c = c;
        }

        public int TestMethod(int x)
        {
            return _a.RetData(x);
        }
    }
}