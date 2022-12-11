namespace Ludwig.Common.Rest
{
    public class MetaObject<T>
    {
        public T Value { get; set; }

        public HttpMetadata Metadata { get; set; }

        public static implicit operator T(MetaObject<T> value)
        {
            return value.Value;
        }

        public static implicit operator MetaObject<T>(T value)
        {
            return new MetaObject<T>
            {
                Value = value,
                Metadata = HttpMetadata.Empty
            };
        }

        public static implicit operator bool(MetaObject<T> value)
        {
            return value.Value != null;
        }
    }
}