using System.Text;

public static class ELExtensions
{
	public static string ReplaceAtIndex(this string text, int index, char c)
	{
		return new StringBuilder(text)
		{
			[index] = c
		}.ToString();
	}
}
