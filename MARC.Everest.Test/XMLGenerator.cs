﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MARC.Everest.Interfaces;
using MARC.Everest.Connectors;
using System.Reflection;
using System.Diagnostics;

namespace MARC.Everest.Test
{
    internal static class XMLGenerator
    {

        private const string instanceDir = "Instances";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal static bool GenerateInstance(Type instanceType, Stream outputStream)
        {
            IResultDetail[] results = null;
            return GenerateInstance(instanceType, outputStream, out results);
        }

        internal static bool GenerateInstance(Type instanceType, Stream outputStream, out IResultDetail[] details)
        {

            string resourceName = String.Format(instanceType.FullName).Replace(".", "");

            var formatter = new MARC.Everest.Formatters.XML.ITS1.XmlIts1Formatter();
            formatter.Settings = MARC.Everest.Formatters.XML.ITS1.SettingsType.DefaultUniprocessor;
            formatter.ValidateConformance = false;
            // Testing pregen
            formatter.GraphAides.Add(new MARC.Everest.Formatters.XML.Datatypes.R1.DatatypeFormatter());
            IGraphable result = TypeCreator.GetCreator(instanceType).CreateInstance() as IGraphable;

            Trace.WriteLine("Starting Serialization");
            DateTime start = DateTime.Now;
            bool gresult = formatter.GraphObject(outputStream, result) == MARC.Everest.Connectors.ResultCode.Accepted;
            Trace.WriteLine(String.Format("            ->{0}", DateTime.Now.Subtract(start).TotalMilliseconds));
            Trace.WriteLine(String.Format("            ->{0} bytes", outputStream.Length));
            List<IResultDetail> dlist = new List<IResultDetail>();
            dlist.AddRange(formatter.Details);
            details = dlist.ToArray();

            return gresult;
        }

    }
}