namespace Ekstazz.Tools
{
    using UnityEngine;

    public static class StringLogExtensions
    {
        public static string ToCyan(this string value) => value.ToColor(Color.cyan);
        public static string ToMagenta(this string value) => value.ToColor(Color.magenta);
        public static string ToYellow(this string value) => value.ToColor(Color.yellow);
        public static string ToRed(this string value) => value.ToColor(Color.red);
        public static string ToGreen(this string value) => value.ToColor(Color.green);
        public static string ToBlue(this string value) => value.ToColor(Color.blue);

        
        public static string ToColor(this string value, Color rgbColor)
        {
            var color = new Vector3Int(
                (byte) (rgbColor.r * 255f),
                (byte) (rgbColor.g * 255f),
                (byte) (rgbColor.b * 255f)
            );
            return $"<color=#{color.x:X2}{color.y:X2}{color.z:X2}>{value}</color>";
        }
    }
}