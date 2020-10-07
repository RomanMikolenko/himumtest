namespace HiMum.ApiGateway.Domain.Models
{
    public class KeyRotationConfiguration
    {
        public const string SectionName = "KeyRotationConfiguration";

        public int RotateTimeSeconds { get; set; }
    }
}
