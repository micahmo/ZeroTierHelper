#region Usings

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace JSONHelper
{
    /// <summary>
    /// Method to assist with JSON serialization and deserialization
    /// </summary>
    public class Serialization
    {
        #region Public static methods

        /// <summary>
        /// Given a JSON string and a list of JSON properties, reads the properties and populates the <see cref="JSONProperty.Value"/>.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="jsonProperties"></param>
        public static void GetPropertiesFromJSON(string json, params JSONProperty[] jsonProperties)
        {
            dynamic dynamicObject = JObject.Parse(json);
            foreach (JProperty jproperty in dynamicObject)
            {
                foreach (JSONProperty property in jsonProperties.Where(property => property.Level == 1 && property.Name == jproperty.Name))
                {
                    property.Value = jproperty.Value;
                }

                // Pass properties with a level higher than 1 to the next level
                List<JSONProperty> nextLevelProperties = jsonProperties.Where(property => property.Level > 1).ToList();

                try
                {
                    GetPropertiesFromJSON(jproperty.Value.ToString(), nextLevelProperties
                    .Select(property => { property.Level--; return property; }) // Decrement the level so that in the recursive call, the highest level is 1 again
                    .ToArray());
                }
                catch
                {
                    // If we can't parse this property as a child, we don't care.
                }
                finally
                {
                    // Restore the property levels
                    nextLevelProperties.Select(property => { property.Level++; return property; }).ToList(); // Have to call ToList to force the enumerator to execute
                }

                // Before looping again, see if we're done
                if (AllPropertiesPopulated(jsonProperties)) return;
            }
        }

        /// <summary>
        /// Given an object and an array of jsonProperties, the object will be populated with the <see cref="JSONProperty.Value"/>
        /// wherever the object has a property of matching <see cref="JSONProperty.PropertyName"/>. The <see cref="JSONProperty.Value"/> will be attempted to be cast
        /// to the type of the object's property. TODO param for throw exception on type error???
        /// </summary>
        /// <param name="objectToPopulate"></param>
        /// <param name="jsonProperties"></param>
        /// <param name="typeMismatchIsFailure">
        /// If true, a type mismatch between the value of the JSONProperty and the object's property will be considered a failure, and an exception will be thrown.
        /// If false, a type mismatch will be ignored, and the object's property will remain null.
        /// </param>
        /// <returns>Returns true if all JSONProperties were successfully assigned to the object. False if not.</returns>
        public static bool PopulateObjectWithJSONProperties(object objectToPopulate, bool typeMismatchIsFailure = false, params JSONProperty[] jsonProperties)
        {
            bool success = true;

            foreach (JSONProperty jsonProperty in jsonProperties)
            {
                // Find a property in the object which matches the JSONProperty's name
                PropertyInfo propertyInfo = objectToPopulate.GetType().GetProperty(jsonProperty.PropertyName, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    try
                    {
                        object valueToSet;

                        // Special case if it's a list
                        if (typeof(JArray).IsAssignableFrom(jsonProperty.Value.GetType()) && typeof(IList).IsAssignableFrom(propertyInfo.PropertyType))
                        {
                            Type enumerableType = propertyInfo.PropertyType.GetGenericArguments()[0];
                            var values = (jsonProperty.Value as JArray)
                                                         .ToObject<IList<object>>()
                                                         .Select(property => Convert.ChangeType(property, enumerableType))
                                                         .ToArray();

                            Type listType = typeof(List<>).MakeGenericType(enumerableType);
                            var typedList = (IList)Activator.CreateInstance(listType);

                            foreach (var value in values)
                                typedList.Add(value);

                            valueToSet = typedList;
                        }
                        else
                        {
                            // Otherwise, it's just the value of the JSONProperty
                            valueToSet = jsonProperty.Value;
                        }

                        propertyInfo.SetValue(objectToPopulate, Convert.ChangeType(valueToSet, propertyInfo.PropertyType));
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        if (typeMismatchIsFailure)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Takes a string JSON list of JSON objects and turns it into a <see cref="IEnumerable"/> of string JSON objects
        /// </summary>
        /// <param name="jsonList"></param>
        /// <returns></returns>
        public static IEnumerable<string> JSONListToEnumerable(string jsonList)
        {
            return JsonConvert.DeserializeObject<List<object>>(jsonList).Select(jobject => jobject.ToString());
        }

        #endregion

        #region Private static methods

        /// <summary>
        /// Checks whether all given properties have a non-null <see cref="JSONProperty.Value"/>. Returns true if so, false if at least one property has a null value.
        /// </summary>
        /// <returns></returns>
        private static bool AllPropertiesPopulated(params JSONProperty[] jsonProperties)
        {
            bool result = true;

            foreach (JSONProperty jsonProperty in jsonProperties)
            {
                if (jsonProperty.Value == null)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        #endregion
    }    
}
