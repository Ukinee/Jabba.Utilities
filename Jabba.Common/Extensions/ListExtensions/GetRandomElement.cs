namespace Utilities.Extensions.ListExtensions
{
    public static partial class ListExtension
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            ArgumentOutOfRangeException.ThrowIfZero(list.Count);
            
            return list[Random.Shared.Next(list.Count)];
        }

        public static List<T> GetRandomElements<T>(this List<T> list, int count) where T : class
        {
            ArgumentOutOfRangeException.ThrowIfZero(list.Count);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

            List<T> result = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                T randomElement = list.GetRandomElement();

                result.Add(randomElement);
            }

            return result;
        }
        
        public static HashSet<T> GetRandomUniqueElements<T>(this List<T> list, int count) where T : class
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(list.Count, count);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
            
            HashSet<T> result = new HashSet<T>(count);

            while (result.Count < count)
            {
                T randomElement = list.GetRandomElement();
                result.Add(randomElement);
            }

            return result;
        }
    }
}
