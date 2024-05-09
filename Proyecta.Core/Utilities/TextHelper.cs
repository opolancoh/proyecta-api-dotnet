namespace Proyecta.Core.Utilities;

public class TextHelper
{
    public static class RegExp
    {
        public static string NameRegExp => @"^[a-zA-Z0-9 .,`'()ÁÉÍÓÚáéíóúñÑ\/\-']*$";
        
        public static string UserNameRegExp => @"^[a-zA-Z0-9_@.\-']*$";
    }
}