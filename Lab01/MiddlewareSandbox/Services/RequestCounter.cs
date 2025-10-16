using System.Threading;

namespace MiddlewareSandbox.Services;

public class RequestCounter
{
	private int _count = 0;

	public int Increment() => Interlocked.Increment(ref _count);

	public int Value => _count;
}
