namespace MauiMoneyMate.Utils
{
    internal static class Extensions
    {
        public static T GetAncestor<T>(this Element element) where T : class
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            var tmp = element;
            while (tmp.GetType() != typeof(T))
            {
                tmp = tmp.Parent;
                if (tmp == null) return null;
            }
            return tmp as T;
        }
    }
}
