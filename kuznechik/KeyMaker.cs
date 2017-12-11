using System;
namespace kuznechik
{
	public class KeyMaker
	{
		public static string KeyGenerator()
		{
			/*
			string[] intCharKey = new string[64];
			Random r = new Random(DateTime.Now.Millisecond);

			for (int j = 0; j < intCharKey.Length; j++) {

				int curentValue = r.Next(15);
				intCharKey[j] = curentValue.ToString("X");
			}
			string keyString = string.Join("", intCharKey);
			*/
			string keyString = "8899aabbccddeeff0011223344556677fedcba98765432100123456789abcdef";

			return keyString;
		}
	}
}
