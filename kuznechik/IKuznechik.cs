namespace kuznechik
{
    
    public interface IKuznechik
    {
  

        
        void SetKey (byte[] key);

        
        byte[] Encrypt (byte[] data);

	
		byte[] Decrypt(byte[] data);
    }
}