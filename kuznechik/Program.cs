using System;
using System.Collections.Generic;
using System.IO;

namespace kuznechik
{
	public class MainClass
	{
		private static void Encryption(string keyFile, string inputFile, string outputFile)
		{
			byte[] masterKey = KeyImport(keyFile);
			string[] inputText = BlockSplitter.BlockSplitterContainer128(InputString(inputFile, false));
			string[] outputBlocks = new string[inputText.Length];
			string result;

			KuznyechikEncrypt encryptor = new KuznyechikEncrypt();
			encryptor.SetKey(masterKey);

			for (int i = 0; i < inputText.Length; i++)
			{
				outputBlocks[i] = ByteToHex.ByteToHexConverter(encryptor.Encrypt(HexToByte.HexToByteConvert(inputText[i])));
			}

			result = BlockBuilder.BlockBuilderContainerFrom128(outputBlocks);

			Console.WriteLine("\nЗашифрованный текст записан в файл " + outputFile);
			Console.WriteLine(result.ToUpper());

			TextWriter cr;

			try
			{
				cr = new StreamWriter(outputFile,false);
			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message + "\n Неудается создать файл.");
				return;
			}
			try
			{
				cr.WriteLine(result);
			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message + "\n Неудается записать в файл.");
				return;
			}

			cr.Close();


		}

		private static void Decryption(string keyFile, string inputFile, string outputFile)
		{
			byte[] masterKey = KeyImport(keyFile);
			string[] inputText = BlockSplitter.BlockSplitterContainer128(InputString(inputFile, true));
			string[] outputBlocks = new string[inputText.Length];
			string result;

			KuznyechikEncrypt decryptor = new KuznyechikEncrypt();
			decryptor.SetKey(masterKey);

			for (int i = 0; i < inputText.Length; i++)
			{
				outputBlocks[i] = ByteToHex.ByteToHexConverter(decryptor.Decrypt(HexToByte.HexToByteConvert(inputText[i])));
			}

			result = BlockBuilder.BlockBuilderContainerFrom128(outputBlocks);

			Console.WriteLine("\nРасшифрованный текст записан в файл " + outputFile);
			Console.WriteLine(result.ToUpper());

			TextWriter cr;

			try
			{
				cr = new StreamWriter(outputFile, false);
			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message + "\n Неудается создать файл.");
				return;
			}
			try
			{
				cr.WriteLine(result);
			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message + "\n Неудается записать в файл.");
				return;
			}

			cr.Close();


		}


		private static void ArgsExeption()
		{
			Console.WriteLine("Неверный параметр ввода аргумента. " +
					"\nДля справки запустите программу с ключем -help");
		}

		private static string InputString(string pathInputArg, bool decrypt)
		{
			string importedInputText;
			TextReader pr;

			try
			{
				pr = new StreamReader(pathInputArg);
			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message + "\n Неудается открыть файл.");
				return null;
			}
			try
			{
				importedInputText = pr.ReadLine();
				pr.Close();
			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message + "\n Неудается импортировать массив байт.");
				return null;
			}

			if (decrypt == true)
			{
				Console.WriteLine("\nЗашифрованный текст: " + importedInputText);
			}
			else {
				Console.WriteLine("\nОткрытый текст: " + importedInputText);
			}
			return importedInputText;
		}

		private static byte[] KeyImport(string pathToKey)
		{
			byte[] importedKey = new byte[32];
			BinaryReader br;

			try
			{
				br = new BinaryReader(File.Open(pathToKey, FileMode.Open));
			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message + "\n Неудается открыть файл.");
				return null;
			}
			try
			{
				br.Read(importedKey, 0, importedKey.Length);
			}
			catch (IOException e)
			{
				Console.WriteLine(e.Message + "\n Неудается импортировать массив байт.");
				return null;
			}
			Console.WriteLine("\nИмпортированный ключ: " + "\n"+ 
			                  ByteToHex.ByteToHexConverter(importedKey).ToUpper());
			br.Close();
			return importedKey;

		}

		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{

				Console.WriteLine("C# Энкриптор/Декриптор алгоритма шифрования Кузнечик в режиме ECB" +
								  "\nДля справки запустите программу с параметром -help");

				Console.WriteLine("\nРежим самотестирования:");
				string masterKey = "8899aabbccddeeff0011223344556677fedcba98765432100123456789abcdef";

				Console.WriteLine("Открытый текст: 1122334455667700FFEEDDCCBBAA9988");

				Console.WriteLine("Ключ: " + masterKey.ToUpper());

				Console.WriteLine("Зашифрованный текст: 7F679D90BEBC24305A468D42B9D4EDCD");

				//byte[] testBytes = HexToByte.HexToByteConvert("8899aabbccddeeff0011223344556677"); -it works

				byte[] test = HexToByte.HexToByteConvert(masterKey);


				byte[] _ref_plain = HexToByte.HexToByteConvert("1122334455667700FFEEDDCCBBAA9988");
				byte[] _ref_cipher = HexToByte.HexToByteConvert("7F679D90BEBC24305A468D42B9D4EDCD");

				//Console.WriteLine(keyTest);

				//string[] testblockString = BlockSplitter.BlockSplitterContainer128(masterKey);

				//Console.WriteLine(BlockBuilder.BlockBuilderContainerFrom128(testblockString).ToUpper()); -it works

				KuznyechikEncrypt cipher = new KuznyechikEncrypt();
				cipher.SetKey(test);
				string cipherTestEncrypt = ByteToHex.ByteToHexConverter(cipher.Encrypt(_ref_plain));
				string cipherTestDecrypt = ByteToHex.ByteToHexConverter(cipher.Decrypt(_ref_cipher));
				Console.WriteLine("\nРезультат работы энкриптора: " + cipherTestEncrypt.ToUpper());
				Console.WriteLine("\nРезультат работы декриптора: " + cipherTestDecrypt.ToUpper());

			}
			else
			{
				if (args.Length == 1)
				{

					//BinaryReader br;
					if (args[0] == "-genkey")
					{
						BinaryWriter bw;
						string generatedMasterKey = KeyMaker.KeyGenerator();
						Console.WriteLine("Генерация мастер-ключа");
						Console.WriteLine("Мастер-ключ сохранён в бинарном виде в файле 'key'");
						Console.WriteLine(generatedMasterKey);
						byte[] generatedKey = HexToByte.HexToByteConvert(generatedMasterKey);


						try
						{
							bw = new BinaryWriter(new FileStream("key", FileMode.OpenOrCreate));
						}
						catch (IOException e)
						{
							Console.WriteLine(e.Message + "\n Неудается создать файл.");
							return;
						}

						//writing into the file
						try
						{
							bw.Write(generatedKey);

						}

						catch (IOException e)
						{
							Console.WriteLine(e.Message + "\n Неудается записать в файл.");
							return;
						}
						bw.Close();


					}
					else {
						if (args[0] == "-help")
						{

							Console.WriteLine("Справка: \nЗапуск программы осушествляется из консоли в виде  " +
											  "\n \nkuznechik.exe [type] -key 'путь до файла ключа' -i 'путь до файла открытого текста' \n-o 'имя и путь для создания файла зашифрованного текста'" +
											 "\n \nгде [type] - режим работы программы: \n -encrypt - шифрование \n -decrypt - расшифровка");



						}
						else {
							if (args[0] == "-set_test_key")
							{

								BinaryWriter bw;

								string testMasterKey = "8899aabbccddeeff0011223344556677fedcba98765432100123456789abcdef";
								Console.WriteLine("Тестовый мастер-ключ сохранён в бинарном виде в файле 'key'");

								byte[] translatedKey = HexToByte.HexToByteConvert(testMasterKey);


								try
								{
									bw = new BinaryWriter(new FileStream("key", FileMode.OpenOrCreate));
								}
								catch (IOException e)
								{
									Console.WriteLine(e.Message + "\n Неудается создать файл.");
									return;
								}


								try
								{
									bw.Write(translatedKey);

								}

								catch (IOException e)
								{
									Console.WriteLine(e.Message + "\n Неудается записать в файл.");
									return;
								}
								bw.Close();

							}
							else { ArgsExeption(); }
						}
					}
				}
				else {
					if (args.Length == 7)
					{
						string mode = args[0];
						string keyKey = args[1];
						string pathKey = args[2];
						string inputKey = args[3];
						string pathInput = args[4];
						string outputKey = args[5];
						string pathOutput = args[6];

						if (mode == "-encrypt" && keyKey == "-key" && pathKey != null && inputKey == "-i"
							&& pathInput != null && outputKey == "-o" && pathOutput != null)
						{
							Encryption(pathKey, pathInput, pathOutput);
						}
						else {
							if (mode == "-decrypt" && keyKey == "-key" && pathKey != null && inputKey == "-i"
							&& pathInput != null && outputKey == "-o" && pathOutput != null) {
							Decryption(pathKey, pathInput, pathOutput);
							}
							else { ArgsExeption(); } 
						}
							}
							else {
								ArgsExeption();
							}
						}
					}
				}
			}
		}



