#region Usings

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using WebHelper;

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

            List<object> jsonNetworks = JsonConvert.DeserializeObject<List<object>>(WebRequestHelper.DoRequest(BASE_URL + GET_NETWORKS_COMMAND, apiToken));
            foreach (var jsonNetwork in jsonNetworks)
            {
                dynamic network = JObject.Parse(jsonNetwork.ToString());
                string id = string.Empty;
                string name = string.Empty;
                foreach (JProperty jproperty in network)
                {
                    if (jproperty.Name == "id")
                    {
                        id = jproperty.Value.ToString();
                    }
                    else if (jproperty.Name == "config")
                    {
                        foreach (JProperty childjproperty in (dynamic)JObject.Parse(jproperty.Value.ToString()))
                        {
                            if (childjproperty.Name == "name")
                            {
                                name = childjproperty.Value.ToString();
                            }
                        }
                    }
                }

                networks.Add(new Network { ID = id, Name = name });
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

                List<object> jsonNetworkMembers = JsonConvert.DeserializeObject<List<object>>(WebRequestHelper.DoRequest(BASE_URL + string.Format(GET_NETWORK_MEMBERS_COMMAND, network.ID), apiToken));
                foreach (var jsonMember in jsonNetworkMembers)
                {
                    dynamic member = JObject.Parse(jsonMember.ToString());
                    string name = string.Empty;
                    string description = string.Empty;
                    string id = string.Empty;
                    bool online = default(bool);
                    List<string> ips = new List<string>();

                    foreach (JProperty jproperty in member)
                    {
                        if (jproperty.Name == "name")
                        {
                            name = jproperty.Value.ToString();
                        }
                        else if (jproperty.Name == "description")
                        {
                            description = jproperty.Value.ToString();
                        }
                        else if (jproperty.Name == "nodeId")
                        {
                            id = jproperty.Value.ToString();
                        }
                        else if (jproperty.Name == "online")
                        {
                            online = bool.Parse(jproperty.Value.ToString());
                        }
                        else if (jproperty.Name == "config")
                        {
                            foreach (JProperty childjproperty in (dynamic)JObject.Parse(jproperty.Value.ToString()))
                            {
                                if (childjproperty.Name == "ipAssignments")
                                {
                                    List<object> ipAssignments = JsonConvert.DeserializeObject<List<object>>(childjproperty.Value.ToString());
                                    ips = ipAssignments.Cast<string>().ToList();
                                }
                            }
                        }
                    }

                    networkMembers[network].Add(new Member
                    {
                        Name = name,
                        Description = description,
                        Online = online,
                        IPAssignmentsList = ips,
                        ID = id
                    });
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
