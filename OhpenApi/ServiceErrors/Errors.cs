using ErrorOr;

namespace OhpenApi.ServiceErrors
{
    /// <summary>
    /// This class lists all the different errors thrown for our application
    /// An error for when the given job cannot be found
    /// A validation error for when the given Job type in the request body is not valid
    /// </summary>
    public static class Errors
    {
        public static Error JobNotFound(Guid id) => Error.NotFound(code: "Job.NotFound", description: $"Job {id} does not exist");

        public static Error JobTypeValidation() => Error.Validation(code: "JobType.Validation", description: "Incorrect format for Job Type, Job Type should either be bulk or batch");

    }
}
