using AutoFixture;
using AutoMapper;

using Moq;
using Moq.AutoMock;

namespace CST.Tests.Common;

public abstract class AutoMockerUnitTestBase
{
	private protected readonly AutoMocker _mocker = new();

	public Fixture Fixture { get; }

	public AutoMockerUnitTestBase()
	{
		Fixture = FixtureInitializer.InitializeFixture();
	}

	public Mock<T> GetMock<T>() where T : class =>
		_mocker.GetMock<T>();

	public T GetService<T>() where T : class =>
		_mocker.Get<T>();

	public void Use<T>(T instance) where T : class
	{
		_mocker.Use(instance);
	}

	public IMapper UseMapperWithProfiles(params Profile[] profiles)
	{
		var mapper = new Mapper(new MapperConfiguration(cfg =>
		{
			cfg.AddProfiles(profiles);
		}));

		Use<IMapper>(mapper);

		return mapper;
	}
}