﻿//Copyright 2012 Spin Services Limited

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportingSolutions.Udapi.Sdk.Clients;
using SportingSolutions.Udapi.Sdk.Interfaces;
using SportingSolutions.Udapi.Sdk.Model;
using log4net;

namespace SportingSolutions.Udapi.Sdk
{
    public class Service : Endpoint, IService
    {
        internal Service(RestItem restItem, IConnectClient connectClient)
            : base(restItem, connectClient)
        {
            Logger = LogManager.GetLogger(typeof(Service).ToString());
            Logger.DebugFormat("Instantiated service={0}", restItem.Name);
        }

        public string Name
        {
            get { return State.Name; }
        }

        public List<IFeature> GetFeatures()
        {
            var loggingStringBuilder = new StringBuilder();
            loggingStringBuilder.Append("Get all available features - ");

            var restItems = FindRelationAndFollow("http://api.sportingsolutions.com/rels/features/list", "GetFeatures Http error", loggingStringBuilder);
            Logger.Debug(loggingStringBuilder);
            return restItems.Select(restItem => new Feature(restItem, ConnectClient)).Cast<IFeature>().ToList();
        }

        public IFeature GetFeature(string name)
        {
            var loggingStringBuilder = new StringBuilder();
            loggingStringBuilder.AppendFormat("Get feature={0} - ", name);

            var restItems = FindRelationAndFollow("http://api.sportingsolutions.com/rels/features/list", "GetFeature Http error", loggingStringBuilder);
            Logger.Debug(loggingStringBuilder);
            return (from restItem in restItems where restItem.Name == name select new Feature(restItem, ConnectClient)).FirstOrDefault();
        }
    }
}
