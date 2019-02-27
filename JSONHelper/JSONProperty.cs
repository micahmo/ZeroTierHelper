namespace JSONHelper
{
    /// <summary>
    /// Represents a node or property in a JSON serialization tree
    /// </summary>
    public class JSONProperty
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="level"></param>
        /// <param name="propertyName"></param>
        public JSONProperty(string name, int level, string propertyName = "")
        {
            Name = name;
            Level = level;
            PropertyName = propertyName;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The name of the JSON property
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The level at which this property resides
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The value of this JSON property
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// The name of the object property to which this JSON property maps
        /// </summary>
        public string PropertyName { get; set; }

        #endregion
    }
}
