using System;

namespace ZeroTierHelper
{
    class Member
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ID { get; set; }

        public bool Online { get; set; }

        public System.Collections.Generic.List<string> IPAssignmentsList { get; set; } = new System.Collections.Generic.List<string>();

        public string IPAssignments => string.Join(Environment.NewLine, IPAssignmentsList);
    }
}
