﻿//----------------------------------------------------------------------- 
// PDS.Witsml.Studio, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using Energistics.DataAccess;
using System.Net;
using Energistics.Common;

namespace PDS.Witsml.Studio.Core.Connections
{
    /// <summary>
    /// Defines static helper methods that can be used to configure WITSML store connections.
    /// </summary>
    public static class ConnectionExtensions
    {
        /// <summary>
        /// Creates a WITSMLWebServiceConnection for the current connection uri and WITSML version.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="version">The WITSML version.</param>
        /// <returns>A <see cref="WITSMLWebServiceConnection"/> instance.</returns>
        public static WITSMLWebServiceConnection CreateProxy(this Connection connection, WMLSVersion version)
        {
            //_log.DebugFormat("A new Proxy is being created with URI: {0}; WitsmlVersion: {1};", connection.Uri, version);
            var proxy = new WITSMLWebServiceConnection(connection.Uri, version);
            proxy.Timeout *= 5;

            if (!string.IsNullOrWhiteSpace(connection.Username))
            {
                proxy.Username = connection.Username;
                proxy.SetSecurePassword(connection.SecurePassword);
            }

            return proxy;
        }

        /// <summary>
        /// Sets the server certificate validation.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public static void SetServerCertificateValidation(this Connection connection)
        {
            if (connection.AcceptInvalidCertificates)
                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;
            else
                ServicePointManager.ServerCertificateValidationCallback = null;
        }

        /// <summary>
        /// Gets the ETP server capabilities URL for the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns>The well-known server capabilities URL.</returns>
        public static string GetEtpServerCapabilitiesUrl(this Connection connection)
        {
            if (string.IsNullOrWhiteSpace(connection?.Uri))
                return string.Empty;

            return $"http{connection.Uri.Substring(2)}/.well-known/etp-server-capabilities";
        }

        /// <summary>
        /// Updates the ETP settings based on the connection settings.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="headers">The headers.</param>
        public static void UpdateEtpSettings(this Connection connection, IDictionary<string, string> headers)
        {
            if (!string.IsNullOrWhiteSpace(connection.SubProtocol))
                EtpSettings.EtpSubProtocolName = connection.SubProtocol;

            if (!string.IsNullOrWhiteSpace(connection.EtpEncoding))
                headers[EtpSettings.EtpEncodingHeader] = connection.EtpEncoding;
        }
    }
}
