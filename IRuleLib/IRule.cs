namespace IRuleLib
{
	public interface IRule : ICloneable
	{
		string Name { get; }

		string Label { get; }

		string Rename(string origin);

		IRule Parse(string data);
	}
}