namespace Proyecta.Core.Utilities;

public static class TextHelper
{
    public static class RegExp
    {
        public static string NameRegExp => @"^[a-zA-Z0-9 _.,`'()ÁÉÍÓÚáéíóúñÑ\/\-']*$";
        
        public static string UserNameRegExp => @"^[a-zA-Z0-9_@.\-']*$";
    }
}