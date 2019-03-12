namespace Common.Container
{
	public interface IContainer
	{
		IContainer CreateChild();

		IContainer RegisterInstance<TAs>(TAs instance, ReuseScope reuseScope);

		IContainer RegisterType<TAs, TOf>(ReuseScope reuseScope) where TOf : TAs;

		IContainer RegisterNamed<TAs, TOf>(string name, ReuseScope reuseScope) where TOf : TAs;

		TAs Resolve<TAs>();

		TAs ResolveNamed<TAs>(string name);
	}
}