#region Usings

using System.Collections.Generic;
using WebHelper;
using JSONHelper;

#endregion

namespace ZeroTierAPI
{
    /// <summary>
    /// Class containing wrapper methods which call the ZeroTier API
    /// </summary>
    public class Requests
    {
        #region Public static methods

        /// <summary>
        /// Given an API token, return all visible networks.
        /// Throws an Exception if there is any problem accessing the ZeroTier API.
        /// </summary>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public static IEnumerable<Network> GetNetworks(string apiToken)
        {
            List<Network> networks = new List<Network>();

            IEnumerable<string> jsonNetworks = Serialization.JSONListToEnumerable(WebRequestHelper.DoRequest(BASE_URL + GET_NETWORKS_COMMAND, apiToken));
            foreach (var jsonNetwork in jsonNetworks)
            {
                JSONProperty idProperty = new JSONProperty("id", 1, nameof(Network.ID));
                JSONProperty nameProperty = new JSONProperty("name", 2, nameof(Network.Name));
                Network network = new Network();

                Serialization.GetPropertiesFromJSON(jsonNetwork.ToString(), idProperty, nameProperty);
                Serialization.PopulateObjectWithJSONProperties(network, false, idProperty, nameProperty);

                networks.Add(network);
            }

            return networks;
        }

        /// <summary>
        /// Given an API token and a list of networks, returns all of the members
        /// Throws an Exception if there is any problem accessing the ZeroTier API.
        /// </summary>
        /// <param name="apiToken"></param>
        /// <param name="networks"></param>
        /// <returns></returns>
        public static IDictionary<Network, IList<Member>> GetMembers(string apiToken, IEnumerable<Network> networks)
        {
            Dictionary<Network, IList<Member>> networkMembers = new Dictionary<Network, IList<Member>>();

            foreach (Network network in networks)
            {
                networkMembers[network] = new List<Member>();

                IEnumerable<string> jsonNetworkMembers = Serialization.JSONListToEnumerable(WebRequestHelper.DoRequest(BASE_URL + string.Format(GET_NETWORK_MEMBERS_COMMAND, network.ID), apiToken));
                foreach (var jsonMember in jsonNetworkMembers)
                {
                    JSONProperty nameProperty = new JSONProperty("name", 1, nameof(Member.Name));
                    JSONProperty descriptionProperty = new JSONProperty("description", 1, nameof(Member.Description));
                    JSONProperty nodeIdProeprty = new JSONProperty("nodeId", 1, nameof(Member.ID));
                    JSONProperty onlineProperty = new JSONProperty("online", 1, nameof(Member.Online));
                    JSONProperty ipAssignmentsProperty = new JSONProperty("ipAssignments", 2, nameof(Member.IPAssignmentsList));
                    Member member = new Member();

                    Serialization.GetPropertiesFromJSON(jsonMember.ToString(), nameProperty, descriptionProperty, nodeIdProeprty, onlineProperty, ipAssignmentsProperty);
                    Serialization.PopulateObjectWithJSONProperties(member, false, nameProperty, descriptionProperty, nodeIdProeprty, onlineProperty, ipAssignmentsProperty);

                    networkMembers[network].Add(member);
                }
            }

            return networkMembers;
        }

        #endregion

        #region Private consts

        private const string BASE_URL = "https://my.zerotier.com/api/";

        private const string GET_NETWORKS_COMMAND = "network";

        // {0} is the network ID
        private const string GET_NETWORK_INFO_COMMAND = "network/{0}";

        // {0} is the network ID
        private const string GET_NETWORK_MEMBERS_COMMAND = "network/{0}/member";

        #endregion
    }
}
