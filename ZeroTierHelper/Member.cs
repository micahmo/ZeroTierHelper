namespace ZeroTierHelper
{
    /// <summary>
    /// Lightweight class representing a ZeroTier network member
    /// </summary>
    public class Member
    {
        /// <summary>
        /// The name of the member
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the member
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The ID of the member
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Whether the member is online
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// A list of IPs assigned to the member
        /// </summary>
        public System.Collections.Generic.List<string> IPAssignmentsList { get; set; } = new System.Collections.Generic.List<string>();

        /// <summary>
        /// A string represnetatio of all IPs assigned to the member
        /// </summary>
        public string IPAssignments => string.Join(System.Environment.NewLine, IPAssignmentsList);
    }
}
