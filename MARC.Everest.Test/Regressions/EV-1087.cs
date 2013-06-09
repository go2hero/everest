﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MARC.Everest.Test.Regressions
{
    using MARC.Everest.Attributes;
    using MARC.Everest.DataTypes;
    using MARC.Everest.Design;
    using MARC.Everest.Interfaces;
    using MARC.Everest.RMIM.UV.NE2010.RIM;
    using MARC.Everest.RMIM.UV.NE2010.Vocabulary;
    using MARC.Everest.RMIM.UV.NE2010.COCT_MT910000UV;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using MARC.Everest.Formatters.XML.ITS1;
    using MARC.Everest.Xml;
    using System.IO;
    using System.Xml;
    using MARC.Everest.Formatters.XML.Datatypes.R1;
    using MARC.Everest.RMIM.UV.NE2010.Interactions;

    [Serializable, GeneratedCode("", ""), Structure(Name = "RelatedPerson", Model = "COCT_MT910000NL", StructureType = StructureAttribute.StructureAttributeType.MessageType, IsEntryPoint = false), Description("RelatedPerson")]
    public class RelatedPerson : MARC.Everest.RMIM.UV.NE2010.COCT_MT910000UV.RelatedPerson, IGraphable, IEquatable<RelatedPerson>, ICloneable
    {
        public RelatedPerson()
        {
        }

        public RelatedPerson(SET<PN> name, SET<TEL> telecom, CE<string> administrativeGenderCode, TS birthTime, TS deceasedTime, SET<AD> addr, OtherIDs asOtherIDs)
            : base(name, telecom, administrativeGenderCode, birthTime, deceasedTime, addr, asOtherIDs)
        {
        }

        public new object Clone()
        {
            return base.MemberwiseClone();
        }

        public bool Equals(RelatedPerson other)
        {
            bool flag = true;
            if (other == null)
            {
                return false;
            }
            flag &= (this.DeceasedInd != null) ? this.DeceasedTime.Equals(other.DeceasedInd) : (other.DeceasedInd == null);
            return flag;
        }

        public override bool Equals(object obj)
        {
            if (obj is RelatedPerson)
            {
                return this.Equals(obj as RelatedPerson);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int num = 1;
            return ((0x11 * num) + ((this.DeceasedInd == null) ? 0 : this.DeceasedInd.GetHashCode()));
        }

        public override bool Validate()
        {
            bool flag = base.Validate();
            if (!flag)
            {
                return false;
            }
            if (this.ClassCode == null)
            {
                return false;
            }
            MethodInfo method = this.ClassCode.GetType().GetMethod("Validate");
            flag &= (method != null) ? ((bool)method.Invoke(this.ClassCode, null)) : true;
            if (this.DeterminerCode == null)
            {
                return false;
            }
            MethodInfo info2 = this.DeterminerCode.GetType().GetMethod("Validate");
            return (flag & ((info2 != null) ? ((bool)info2.Invoke(this.DeterminerCode, null)) : true));
        }

        [TypeConverter(typeof(ExpandableObjectConverter)), Editor(typeof(NewInstanceTypeEditor), typeof(UITypeEditor)), Property(Name = "deceasedInd", PropertyType = PropertyAttribute.AttributeAttributeType.NonStructural, Conformance = PropertyAttribute.AttributeConformanceType.Optional, SortKey = 6, DefaultUpdateMode = UpdateMode.Unknown), Description("deceasedTime"), Category("Optional")]
        public BL DeceasedInd { get; set; }
    }

    [TestClass]
    public class EV_1087
    {
        [TestMethod]
        public void EV_1087XsiTypeTest()
        {
            REPC_IN002120UV01 testInstance = new REPC_IN002120UV01();
            testInstance.controlActProcess = new RMIM.UV.NE2010.MCAI_MT700201UV01.ControlActProcess<RMIM.UV.NE2010.REPC_MT002000UV01.CareProvisionRequest>();
            testInstance.controlActProcess.Subject.Add(new RMIM.UV.NE2010.MCAI_MT700201UV01.Subject2<RMIM.UV.NE2010.REPC_MT002000UV01.CareProvisionRequest>());
            testInstance.controlActProcess.Subject[0].act = new RMIM.UV.NE2010.REPC_MT002000UV01.CareProvisionRequest();
            testInstance.controlActProcess.Subject[0].act.PertinentInformation3.Add(new RMIM.UV.NE2010.REPC_MT002000UV01.PertinentInformation5());

            var careStatement = new RMIM.UV.NE2010.REPC_MT000100UV01.Procedure()
            {
                Subject = new List<RMIM.UV.NE2010.REPC_MT000100UV01.Subject4>() {
                    new RMIM.UV.NE2010.REPC_MT000100UV01.Subject4()
                }
            };
            RelatedPerson person = new RelatedPerson(
                SET<PN>.CreateSET(new PN(EntityNameUse.Legal, "John Smith")),
                SET<TEL>.CreateSET((TEL)"mailto:john@smith.com"),
                "F",
                DateTime.Now,
                DateTime.Today,
                SET<AD>.CreateSET(AD.CreateAD(PostalAddressUse.Direct, new ADXP("123 Main Street West, Hamilton, ON"))),
                null);
            person.DeceasedInd = true;

            careStatement.Subject[0].SetSubjectChoice(new PersonalRelationship()
            {
                RelationshipHolder = person
            });
            testInstance.controlActProcess.Subject[0].act.PertinentInformation3[0].SetCareStatement(careStatement);


            // This fails
            XmlIts1Formatter fmtr = new XmlIts1Formatter();
            fmtr.GraphAides.Add(new DatatypeFormatter());
            StringWriter sw = new StringWriter();
            using (XmlStateWriter writer = new XmlStateWriter(XmlWriter.Create(sw)))
                fmtr.Graph(writer, testInstance);

            fmtr = new XmlIts1Formatter();
            fmtr.GraphAides.Add(new DatatypeFormatter());
            StringReader rdr = new StringReader(sw.ToString());
            using (XmlStateReader reader = new XmlStateReader(XmlReader.Create(rdr)))
            {
                var result = fmtr.Parse(reader, typeof(REPC_IN002120UV01).Assembly);
                //Assert.AreEqual(testInstance, result.Structure);
                Assert.AreEqual(person.GetType(), (((result.Structure as REPC_IN002120UV01).controlActProcess.Subject[0].act.PertinentInformation3[0].CareStatement as RMIM.UV.NE2010.REPC_MT000100UV01.Procedure).Subject[0].SubjectChoice as PersonalRelationship).RelationshipHolder.GetType());
            }
        }

        [TestMethod]
        public void EV_1087Test()
        {

            RelatedPerson person = new RelatedPerson(
                SET<PN>.CreateSET(new PN(EntityNameUse.Legal, "John Smith")),
                SET<TEL>.CreateSET((TEL)"mailto:john@smith.com"),
                "F",
                DateTime.Now,
                DateTime.Today,
                SET<AD>.CreateSET(AD.CreateAD(PostalAddressUse.Direct, new ADXP("123 Main Street West, Hamilton, ON"))),
                null);
            person.DeceasedInd = true;

            // This fails
            try
            {
                XmlIts1Formatter fmtr = new XmlIts1Formatter();
                fmtr.Settings = SettingsType.DefaultLegacy;
                fmtr.GraphAides.Add(new DatatypeFormatter());
                StringWriter sw = new StringWriter();
                using (XmlStateWriter writer = new XmlStateWriter(XmlWriter.Create(sw)))
                {
                    writer.WriteStartElement("test", "urn:hl7-org:v3");
                    fmtr.Graph(writer, person);
                    writer.WriteEndElement();
                }

            }
            catch (Exception ex)
            {
                Assert.Fail();
            }

        }
    }
}
