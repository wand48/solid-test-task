namespace Solid.TestTask.Exceptions
{
    public class DataStoreException : Exception
    {
        public DataStoreException(Exception innerException)
            : base("Произошло исключение хранилища данных.", innerException)
        {

        }
    }
}
