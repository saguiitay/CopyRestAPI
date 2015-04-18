namespace CopyRestAPI.Helpers
{
    public static class BooleanExtension
    {

        public static string ToLowerString(this bool input)
        {
            return input.ToString().ToLower();
        }
    }
}
