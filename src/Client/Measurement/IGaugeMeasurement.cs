
namespace LibratoSharp.Client.Measurement
{
	public interface IGaugeMeasurement : IMeasurement
	{
		object Count { get; }
		object Sum { get; }
		object Max { get; }
		object Min { get; }
		object SumSquares { get; }
	}
}