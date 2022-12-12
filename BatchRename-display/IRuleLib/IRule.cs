namespace IRuleLib
{
	public interface IRule : ICloneable
	{
		string Name { get; }

		string Label { get; }

		//bool isActive { get; set; }

		string Rename(string origin);

		IRule Parse(string data);
	}
}