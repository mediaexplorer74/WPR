// System.Security.Cryptography "imitation"
// More info: https://www.youtube.com/watch?v=X1V6_OyQKLw (Rus.)
// https://blog.jsinh.in/secure-your-data-with-protecteddata-and-protectedmemory-using-csharp/#.ZEqBXXZBzrc (Rus.)
// http://www.codedigest.com/Articles/Framework/69_Data_Encryption_and_Decryption_using_DPAPI_classes_in_NET.aspx
// https://www.demo2s.com/csharp/csharp-protecteddata-tutorial-with-examples.html
// https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.protecteddata?view=windowsdesktop-8.0

using System;
using System.Security.Cryptography;

namespace WPR.WindowsCompability
{
    // projection: System.Security.Cryptography
    public static class ProtectedData //RnD : static
    {        
        static ProtectedData()//Cryptography()
        {
        }

        //RnD : static
        public static byte[] Protect(byte[] byteArrayOfOriginalData, byte[] additionalEntropyOrSalt)
        {       
            byte[] result = System.Security.Cryptography
                .ProtectedData.Protect(byteArrayOfOriginalData, 
                additionalEntropyOrSalt, 
                DataProtectionScope.CurrentUser);
            return result;
        }

        //RnD : static
        public static byte[] Unprotect(byte[] byteArrayOfOriginalData, byte[] additionalEntropyOrSalt)
        {
            byte[] result = System.Security.Cryptography
               .ProtectedData.Unprotect(byteArrayOfOriginalData, 
               additionalEntropyOrSalt, 
               DataProtectionScope.CurrentUser);
            return result;
        }
    }
}
