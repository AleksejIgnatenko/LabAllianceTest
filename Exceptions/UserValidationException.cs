namespace LabAllianceTest.Exceptions
{
    public class UserValidationException : Exception
    {
        public Dictionary<string, string> Errors { get; set; }

        public UserValidationException(Dictionary<string, string> errors)
        {
            Errors = errors;
        }
    }
}
