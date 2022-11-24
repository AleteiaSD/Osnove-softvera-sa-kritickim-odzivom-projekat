﻿using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus read holding registers functions/requests.
    /// </summary>
    public class ReadHoldingRegistersFunction : ModbusFunction
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadHoldingRegistersFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public ReadHoldingRegistersFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
		{
			CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
		}

		/// <inheritdoc />
		public override byte[] PackRequest()
		{
            byte[] ret = new byte[12];
            ret[1] = (byte)(CommandParameters.TransactionId);
            ret[0] = (byte)(CommandParameters.TransactionId >> 8);
            ret[3] = (byte)(CommandParameters.ProtocolId);
            ret[2] = (byte)(CommandParameters.ProtocolId >> 8);
            ret[5] = (byte)(CommandParameters.Length);
            ret[4] = (byte)(CommandParameters.Length >> 8);
            ret[6] = CommandParameters.UnitId;
            ret[7] = CommandParameters.FunctionCode;
            ret[9] = (byte)(((ModbusReadCommandParameters)CommandParameters).StartAddress);
            ret[8] = (byte)(((ModbusReadCommandParameters)CommandParameters).StartAddress >> 8);
            ret[11] = (byte)(((ModbusReadCommandParameters)CommandParameters).Quantity);
            ret[10] = (byte)(((ModbusReadCommandParameters)CommandParameters).Quantity >> 8);
            return ret;
        }

        /// <inheritdoc />
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
        {
            Dictionary<Tuple<PointType, ushort>, ushort> ret = new Dictionary<Tuple<PointType, ushort>, ushort>();

            ushort ADR = ((ModbusReadCommandParameters)CommandParameters).StartAddress;

            ushort val = (ushort)((response[9] << 8) | response[10]);

            ret.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, ADR), val);

            Console.WriteLine(response);
            return ret;
        }
	}
}