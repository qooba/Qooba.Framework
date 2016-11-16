using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Expressions.Abstractions;
using Qooba.Framework.Specification.Abstractions;
using Xunit;

namespace Qooba.Framework.Tests
{
    public class BootstrapperTests
    {
        [Fact]
        public void PassingTest()
        {
            Bootstrapper.Bootstrapp();
            var container = ContainerManager.Current.Resolve<IContainer>();
            var t = container.Resolve<IExpressionHelper>();
            var f = container.Resolve<IFactory<IExpressionHelper>>();
            var s = f.Create();
            f = container.Resolve<IFactory<IExpressionHelper>>();
            s = f.Create();
            var s1 = container.Resolve<S>();


            ContainerManager.Current.RegisterType<ISpecification<S>, Qooba.Framework.Specification.Specification<S>>();
            var ts = container.Resolve<ISpecification<S>>();
            ContainerManager.Current.RegisterType<IS, S>();
            

            ContainerManager.Current.RegisterType<IM, M>("test1");
            ContainerManager.Current.RegisterType<IM, M>("test2");
            MultiResolve();
            var m = ContainerManager.Current.Resolve<IM>("test1");
            var hello = m.Hello();
            Assert.Equal(4, Add(2, 2));
        }

        private void MultiResolve()
        {
            for(var i = 0; i< 100000; i++)
            {
                ContainerManager.Current.Resolve<IM>("test1");
            }
        }
        
        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public void MyFirstTheory(int value)
        {
            Assert.True(IsOdd(value));
        }
        
        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }
    }

    public class M : IM
    {
        private IS s;

        public M(IS s)
        {
            this.s = s;
        }

        public string Hello()
        {
            return this.s.Hello();
        }
    }

    public interface IM
    {
        string Hello();
    }

    public class S : IS
    {
        public S(IExpressionHelper ex)
        {
            this.Ex = ex;
        }

        public IExpressionHelper Ex { get; private set; }

        public string Hello()
        {
            return "Hello world";
        }
    }

    public interface IS
    {
        string Hello();
    }
}
