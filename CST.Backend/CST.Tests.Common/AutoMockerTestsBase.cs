namespace CST.Tests.Common;

public abstract class AutoMockerTestsBase<TTarget> : AutoMockerUnitTestBase
	where TTarget : class
{
	private TTarget _target;
	public TTarget Target => _target ??= _mocker.CreateInstance<TTarget>();
}